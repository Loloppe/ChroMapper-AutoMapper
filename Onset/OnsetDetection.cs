using NAudio.Vorbis;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/Teh-Lemon/Onset-Detection
namespace Automapper.Onset
{
    // https://github.com/Teh-Lemon/Onset-Detection
    // Anthony Lee 11010841
    // Main meat of the project
    // Load's the given audio file into memory
    // Plays/Pauses the audio file
    // Performs analysis on the audio file
    class AudioAnalysis
    {
        const int SAMPLE_SIZE = 1024;

        // Audio stream fed into the sound playback device
        //BlockAlignReductionStream stream;
        // Instance of sound playback device
        public WaveOutEvent outputDevice;

        // Fast Fourier Transform library
        FastFT fft;

        /// <summary>
        /// Raw audio data
        /// </summary>
        public dynamic PCMStream { get; set; }

        // Onset Detection
        OnsetDetection onsetDetection;
        public float[] OnsetsFound { get; set; }
        public float TimePerSample { get; set; }

        // Constructor
        public AudioAnalysis()
        {
            SetUpFFT();
        }

        ~AudioAnalysis()
        {
            DisposeOutputDevice();
        }

        // Used to set up the sound device
        private void InitialiseOutputDevice()
        {
            DisposeOutputDevice();

            outputDevice = new WaveOutEvent();
            //outputDevice.Init(new WaveChannel32(stream));
            outputDevice.Init(PCMStream);
        }

        public void LoadAudioFromFile(string filePath)
        {
            // MP3
            if (filePath.EndsWith(".mp3"))
            {
                PCMStream = new AudioFileReader(filePath);
            }
            // WAV
            else if (filePath.EndsWith(".wav"))
            {
                PCMStream = new AudioFileReader(filePath);
            }
            // OGG/EGG
            else if (filePath.EndsWith(".ogg") || filePath.EndsWith(".egg"))
            {
                PCMStream = new VorbisWaveReader(Automapper._beatSaberSongContainer.Song.Directory + "\\" + Automapper._beatSaberSongContainer.Song.SongFilename);
            }

            if (PCMStream != null)
            {
                // Throw an error is the audio has more channels than stereo
                if (PCMStream.WaveFormat.Channels > 2)
                {
                    throw new FormatException("Only Mono and Stereo are supported");
                }

                InitialiseOutputDevice();
                OnsetsFound = null;
            }
            else
            {
                throw new FormatException("Invalid audio file");
            }
        }

        // Play out the loaded audio file
        // Returns whether function was successful
        public bool PlayAudio()
        {
            if (PCMStream != null)
            {
                // If audio was previously stopped
                // Or audio has reached the end of the track
                // Reset the playback position to the beginning
                if (outputDevice.PlaybackState == PlaybackState.Stopped
                    || PCMStream.Position == PCMStream.Length)
                {
                    PCMStream.Position = 0;
                }

                outputDevice.Play();

                return true;
            }

            return false;
        }

        // Pause the audio file
        public bool PauseAudio()
        {
            if (PCMStream != null)
            {
                outputDevice.Pause();

                return true;
            }

            return false;
        }

        // Stop the audio file
        public bool StopAudio()
        {
            if (PCMStream != null)
            {
                outputDevice.Stop();
                PCMStream.Position = 0;

                return true;
            }

            return false;
        }

        // Track Position getter/setter
        public long GetTrackPosition()
        {
            return PCMStream.Position;
        }
        public void SetTrackPosition(long position)
        {
            PCMStream.Position = position;
        }

        public void DetectOnsets(float sensitivity = 1.5f, float indistinguishableRange = 0.01f)
        {
            onsetDetection = new OnsetDetection(PCMStream, 1024);
            // Has finished reading in the audio file
            bool finished = false;
            // Set the pcm data back to the beginning
            SetTrackPosition(0);

            do
            {
                // Read in audio data and find the flux values until end of audio file
                finished = onsetDetection.AddFlux(ReadMonoPCM());
            }
            while (!finished);

            // Find peaks
            onsetDetection.FindOnsets(sensitivity, 1, indistinguishableRange);
        }

        public void NormalizeOnsets(int type)
        {
            onsetDetection.NormalizeOnsets(type);
        }

        public float[] GetOnsets()
        {
            return onsetDetection.Onsets;
        }

        public double GetTimePerSample()
        {
            return onsetDetection.TimePerSample();
        }

        #region Internals

        // Read in a sample and convert it to mono
        float[] ReadMonoPCM()
        {
            int size = SAMPLE_SIZE;

            // If stereo
            if (PCMStream.WaveFormat.Channels == 2)
            {
                size *= 2;
            }

            float[] output = new float[size];

            // Read in a sample
            if (PCMStream.Read(output, 0, size) == 0)
            {
                // If end of audio file
                return null;
            }

            // If stereo, convert to mono
            if (PCMStream.WaveFormat.Channels == 2)
            {
                return ConvertStereoToMono(output);
            }
            else
            {
                return output;
            }
        }

        // Averages the 2 channels into 1
        float[] ConvertStereoToMono(float[] input)
        {
            float[] output = new float[input.Length / 2];
            int outputIndex = 0;

            float leftChannel = 0.0f;
            float rightChannel = 0.0f;

            // Go through each pair of samples
            // Average out the pair
            // Save to output
            for (int i = 0; i < input.Length; i += 2)
            {
                leftChannel = input[i];
                rightChannel = input[i + 1];

                // Average the two channels
                output[outputIndex] = (leftChannel + rightChannel) / 2;
                outputIndex++;
            }

            return output;
        }

        // Starts up the Fast Fourier Transform class
        void SetUpFFT()
        {
            fft = new FastFT();

            //Determine how phase works on the forward and inverse transforms. 
            // (0, 1) default
            // (1, -1) for signal processing
            fft.A = 0;
            fft.B = 1;
        }

        // Properly clean up sound output device
        public void DisposeOutputDevice()
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
            }
        }

        public void DisposeAudioAnalysis()
        {
            DisposeOutputDevice();

            if (PCMStream != null)
            {
                PCMStream.Dispose();
                PCMStream = null;
            }

            OnsetsFound = null;
            TimePerSample = 0.0f;
        }

        #endregion
    }

    class OnsetDetection
    {
        FastFT fft = new FastFT();
        dynamic PCM;
        int SampleSize;

        public float[] Onsets { get; set; }
        float[] LowOnsets { get; set; }
        float[] MidOnsets { get; set; }
        float[] HighOnsets { get; set; }

        float[] previousSpectrum;
        float[] spectrum;

        bool rectify;

        List<float> fluxes;


        // Constructor
        public OnsetDetection(dynamic pcm, int sampleWindow)
        {
            PCM = pcm;
            SampleSize = sampleWindow;

            spectrum = new float[sampleWindow / 2 + 1];
            previousSpectrum = new float[spectrum.Length];
            rectify = true;
            fluxes = new List<float>();
        }

        /// <summary>
        ///  Perform Spectral Flux onset detection on loaded audio file
        ///  <para>Recommended onset detection algorithm for most needs</para>  
        /// </summary>
        ///  <param name="hamming">Apply hamming window before FFT function. 
        ///  <para>Smooths out the noise in between peaks.</para> 
        ///  <para>Small improvement but isn't too costly.</para> 
        ///  <para>Default: true</para></param>
        public bool AddFlux(float[] samples, bool hamming = true)
        {
            // Find the spectral flux of the audio
            if (samples != null)
            {
                // Perform Fast Fourier Transform on the audio samples
                fft.RealFFT(samples, hamming);

                // Update spectrums
                Array.Copy(spectrum, previousSpectrum, spectrum.Length);
                Array.Copy(fft.GetPowerSpectrum(), spectrum, spectrum.Length);

                fluxes.Add(CompareSpectrums(spectrum, previousSpectrum, rectify));
                return false;
            }
            // End of audio file
            else
            {
                return true;
            }
        }

        /// <param name="thresholdTimeSpan">Amount of data used during threshold averaging, in seconds.
        /// <para>Default: 1</para></param>
        /// <param name="sensitivity">Sensitivivity of onset detection.
        /// <para>Lower increases the sensitivity</para>
        /// <para>Recommended: 1.3 - 1.6</para>
        /// <para>Default: 1.5</para></param>
        // Use threshold average to find the onsets from the spectral flux
        public void FindOnsets(float sensitivity = 1.5f, float thresholdTimeSpan = 1f, float indistinguishableRange = 0.01f)
        {
            float[] thresholdAverage = GetThresholdAverage(fluxes, SampleSize,
            thresholdTimeSpan, sensitivity);

            Onsets = GetPeaks(fluxes, thresholdAverage, SampleSize, indistinguishableRange);
        }

        /// <summary>
        ///  Normalize the beats found.
        /// </summary>
        /// <param name="type">Type of normaliztion.
        /// <para>0 = Normalize onsets between 0 and max onset</para>
        /// <para>1 = Normalize onsets between min onset and max onset.</para></param>
        public void NormalizeOnsets(int type)
        {
            if (Onsets != null)
            {
                float max = 0;
                float min = 0;
                float difference = 0;

                // Find strongest/weakest onset
                for (int i = 0; i < Onsets.Length; i++)
                {
                    max = Math.Max(max, Onsets[i]);
                    min = Math.Min(min, Onsets[i]);
                }
                difference = max - min;

                // Normalize the onsets
                switch (type)
                {
                    case 0:
                        for (int i = 0; i < Onsets.Length; i++)
                        {
                            Onsets[i] /= max;
                        }
                        break;
                    case 1:
                        for (int i = 0; i < Onsets.Length; i++)
                        {
                            if (Onsets[i] == min)
                            {
                                Onsets[i] = 0.01f;
                            }
                            else
                            {
                                Onsets[i] -= min;
                                Onsets[i] /= difference;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return;
        }



        #region Internals
        float CompareSpectrums(float[] spectrum, float[] previousSpectrum, bool rectify)
        {
            // Find difference between each respective bins of each spectrum
            // Sum up these differences
            float flux = 0;
            for (int i = 0; i < spectrum.Length; i++)
            {
                float value = (spectrum[i] - previousSpectrum[i]);
                // If ignoreNegativeEnergy is true
                // Only interested in rise in energy, ignore negative values
                if (!rectify || value > 0)
                {
                    flux += value;
                }
            }

            return flux;
        }

        // Finds the peaks in the flux above the threshold average
        float[] GetPeaks(List<float> data, float[] dataAverage, int sampleCount, float indistinguishableRange = 0.01f)
        {
            // Number of set of samples to ignore after an onset
            int immunityPeriod = (int)((double)sampleCount
                / (double)PCM.WaveFormat.SampleRate
                / indistinguishableRange);

            // Results
            float[] peaks = new float[data.Count];

            // For each sample
            for (int i = 0; i < data.Count; i++)
            {
                // Add the peak if above the average, else 0
                if (data[i] >= dataAverage[i])
                {
                    peaks[i] = data[i] - dataAverage[i];
                }
                else
                {
                    peaks[i] = 0.0f;
                }
            }

            // Prune the peaks list
            peaks[0] = 0.0f;
            for (int i = 1; i < peaks.Length - 1; i++)
            {
                // If the next value is lower than the current value, that means it is end of the peak
                if (peaks[i] < peaks[i + 1])
                {
                    peaks[i] = 0.0f;
                    continue;
                }

                // Remove peaks too close to each other
                if (peaks[i] > 0.0f)
                {
                    for (int j = i + 1; j < i + immunityPeriod; j++)
                    {
                        if (j < peaks.Length)
                        {
                            if (peaks[j] > 0)
                            {
                                peaks[j] = 0.0f;
                            }
                        }
                    }
                }
            }

            return peaks;
        }

        // Find the running average of the given list
        float[] GetThresholdAverage(List<float> data, int sampleWindow,
            float thresholdTimeSpan, float thresholdMultiplier)
        {
            List<float> thresholdAverage = new List<float>();

            // How many spectral fluxes to look at, at a time (approximation is fine)
            double sourceTimeSpan = (double)(sampleWindow) / (double)(PCM.WaveFormat.SampleRate);
            int windowSize = (int)(thresholdTimeSpan / sourceTimeSpan / 2);

            for (int i = 0; i < data.Count; i++)
            {
                // Max/Min Prevent index out of bounds error
                // Look at values to the left and right of the current spectral flux
                int start = Math.Max(i - windowSize, 0);
                int end = Math.Min(data.Count, i + windowSize);
                // Current average
                float mean = 0;

                // Sum up the surrounding values
                for (int j = start; j < end; j++)
                {
                    mean += data[j];
                }

                // Find the average
                mean /= (end - start);

                // Multiply mean to increase the sensitivity
                thresholdAverage.Add(mean * thresholdMultiplier);
            }

            return thresholdAverage.ToArray();
        }

        public double TimePerSample()
        {
            // Length of time per sample
            return (double)SampleSize / (double)PCM.WaveFormat.SampleRate; // 1024 and 44100 or 48000. This return 0.0232.. seconds for example.
        }

        #endregion

    }

    // https://github.com/Teh-Lemon/Onset-Detection
    // Anthony Lee 11010841
    // Adds additional functionality to base LomontFFT
    // Stores the complex numbers rather than just mutating them
    // Adds features to find the power spectrum of the complex numbers
    // Applies a hamming window to the data before doing the FFT
    class FastFT : LomontFFT
    {
        float[] real;
        float[] imag;
        float[] spectrum;

        // Constructor
        public FastFT() : base() { }

        /// <summary>
        /// Finds the absolute values of the complex numbers
        /// </summary>
        public float[] GetPowerSpectrum()
        {
            if (real != null)
            {
                FillSpectrum(ref spectrum);
                return spectrum;
            }
            else
            {
                return null;
            }
        }

        /// <summary>                                                                                            
        /// Compute the forward or inverse Fourier Transform of data, with                                       
        /// data containing real valued data only. The length must be a power                                       
        /// of 2.                                                                                                
        /// </summary>                                                                                           
        /// <param name="data">The real parts of the complex data</param>                                                                          
        /// <param name="forward">true for a forward transform, false for                                        
        /// inverse transform</param>    
        /// <param name="hamming">Whether to apply a hamming window before FFT</param>
        /// <returns>The output is complex                                         
        /// valued after the first two entries, stored in alternating real                                       
        /// and imaginary parts. The first two returned entries are the real                                     
        /// parts of the first and last value from the conjugate symmetric                                       
        /// output, which are necessarily real.</returns>
        public void RealFFT(float[] data, bool hamming = true)
        {
            // Copy data to a local array
            // Local array is stored so it can be used by other functions of the class
            double[] complexNumbers = new double[data.Length];
            data.CopyTo(complexNumbers, 0);

            if (hamming)
            {
                ApplyHammingWindow(complexNumbers);
            }

            // Perform FFT on local data
            base.RealFFT(complexNumbers, true);

            SeparateComplexNumbers(complexNumbers);
        }

        #region Internals

        // Applies a hamming window to the data provided before FFT
        void ApplyHammingWindow(double[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= (0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (data.Length - 1)));
            }
        }

        // Sorts out the symmetry of the FFT results into separate real and imaginary numbers
        void SeparateComplexNumbers(double[] complexNumbers)
        {
            real = new float[complexNumbers.Length / 2 + 1];
            imag = new float[complexNumbers.Length / 2 + 1];
            // Location of the last purely real number
            int midPoint = complexNumbers.Length / 2;

            // First bin is purely real
            real[0] = (float)complexNumbers[0];
            imag[0] = 0.0f;

            // Fill in ascending complex numbers
            for (int i = 2; i < complexNumbers.Length - 1; i += 2)
            {
                real[i / 2] = (float)complexNumbers[i];
                imag[i / 2] = (float)complexNumbers[i + 1];
            }

            // Last of the purely real bins
            real[midPoint] = (float)complexNumbers[1];
            imag[midPoint] = 0.0f;
        }

        // Populates the spectrum array with the amplitudes of the data in real and imaginary
        void FillSpectrum(ref float[] data)
        {
            data = new float[real.Length];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (float)Math.Sqrt((real[i] * real[i]) + (imag[i] * imag[i]));
            }
        }
        #endregion
    }

    // Code to implement decently performing FFT for complex and real valued                                         
    // signals. See www.lomont.org for a derivation of the relevant algorithms                                       
    // from first principles. Copyright Chris Lomont 2010-2012.                                                      
    // This code and any ports are free for all to use for any reason as long                                        
    // as this header is left in place.                                                                              
    // Version 1.1, Sept 2011       

    // https://github.com/Teh-Lemon/Onset-Detection
    // Largely untouched - Anthony Lee                                                                                 

    /* History:                                                                                                      
     * Sep 2011 - v1.1 - added parameters to support various sign conventions                                        
     *                   set via properties A and B.                                                                 
     *                 - Removed dependencies on LINQ and generic collections.                                       
     *                 - Added unit tests for the new properties.                                                    
     *                 - Switched UnitTest to static.                                                                
     * Jan 2010 - v1.0 - Initial release                                                                             
     */

    /// <summary>                                                                                                
    /// Represent a class that performs real or complex valued Fast Fourier                                      
    /// Transforms. Instantiate it and use the FFT or TableFFT methods to                                        
    /// compute complex to complex FFTs. Use FFTReal for real to complex                                         
    /// FFTs which are much faster than standard complex to complex FFTs.                                        
    /// Properties A and B allow selecting various FFT sign and scaling                                          
    /// conventions.                                                                                             
    /// </summary>   
    public class LomontFFT
    {
        /// <summary>                                                                                            
        /// Compute the forward or inverse Fourier Transform of data, with                                       
        /// data containing complex valued data as alternating real and                                          
        /// imaginary parts. The length must be a power of 2. The data is                                        
        /// modified in place.                                                                                   
        /// </summary>                                                                                           
        /// <param name="data">The complex data stored as alternating real                                       
        /// and imaginary parts</param>                                                                          
        /// <param name="forward">true for a forward transform, false for                                        
        /// inverse transform</param>                                                                            
        public void FFT(double[] data, bool forward)
        {
            var n = data.Length;
            // checks n is a power of 2 in 2's complement format                                                 
            if ((n & (n - 1)) != 0)
                throw new ArgumentException(
                    "data length " + n + " in FFT is not a power of 2");
            n /= 2;    // n is the number of samples                                                             

            Reverse(data, n); // bit index data reversal                                                         

            // do transform: so single point transforms, then doubles, etc.                                      
            double sign = forward ? B : -B;
            var mmax = 1;
            while (n > mmax)
            {
                var istep = 2 * mmax;
                var theta = sign * Math.PI / mmax;
                double wr = 1, wi = 0;
                var wpr = Math.Cos(theta);
                var wpi = Math.Sin(theta);
                for (var m = 0; m < istep; m += 2)
                {
                    for (var k = m; k < 2 * n; k += 2 * istep)
                    {
                        var j = k + istep;
                        var tempr = wr * data[j] - wi * data[j + 1];
                        var tempi = wi * data[j] + wr * data[j + 1];
                        data[j] = data[k] - tempr;
                        data[j + 1] = data[k + 1] - tempi;
                        data[k] = data[k] + tempr;
                        data[k + 1] = data[k + 1] + tempi;
                    }
                    var t = wr; // trig recurrence                                                               
                    wr = wr * wpr - wi * wpi;
                    wi = wi * wpr + t * wpi;
                }
                mmax = istep;
            }

            // perform data scaling as needed                                                                    
            Scale(data, n, forward);
        }

        /// <summary>                                                                                            
        /// Compute the forward or inverse Fourier Transform of data, with data                                  
        /// containing complex valued data as alternating real and imaginary                                     
        /// parts. The length must be a power of 2. This method caches values                                    
        /// and should be slightly faster on than the FFT method for repeated uses.                              
        /// It is also slightly more accurate. Data is transformed in place.                                     
        /// </summary>                                                                                           
        /// <param name="data">The complex data stored as alternating real                                       
        /// and imaginary parts</param>                                                                          
        /// <param name="forward">true for a forward transform, false for                                        
        /// inverse transform</param>                                                                            
        public void TableFFT(double[] data, bool forward)
        {
            var n = data.Length;
            // checks n is a power of 2 in 2's complement format                                                 
            if ((n & (n - 1)) != 0)
                throw new ArgumentException(
                    "data length " + n + " in FFT is not a power of 2"
                    );
            n /= 2;    // n is the number of samples                                                             

            Reverse(data, n); // bit index data reversal                                                         

            // make table if needed                                                                              
            if ((cosTable == null) || (cosTable.Length != n))
                Initialize(n);

            // do transform: so single point transforms, then doubles, etc.                                      
            double sign = forward ? B : -B;
            var mmax = 1;
            var tptr = 0;
            while (n > mmax)
            {
                var istep = 2 * mmax;
                for (var m = 0; m < istep; m += 2)
                {
                    var wr = cosTable[tptr];
                    var wi = sign * sinTable[tptr++];
                    for (var k = m; k < 2 * n; k += 2 * istep)
                    {
                        var j = k + istep;
                        var tempr = wr * data[j] - wi * data[j + 1];
                        var tempi = wi * data[j] + wr * data[j + 1];
                        data[j] = data[k] - tempr;
                        data[j + 1] = data[k + 1] - tempi;
                        data[k] = data[k] + tempr;
                        data[k + 1] = data[k + 1] + tempi;
                    }
                }
                mmax = istep;
            }


            // perform data scaling as needed                                                                    
            Scale(data, n, forward);
        }

        /// <summary>                                                                                            
        /// Compute the forward or inverse Fourier Transform of data, with                                       
        /// data containing real valued data only. The output is complex                                         
        /// valued after the first two entries, stored in alternating real                                       
        /// and imaginary parts. The first two returned entries are the real                                     
        /// parts of the first and last value from the conjugate symmetric                                       
        /// output, which are necessarily real. The length must be a power                                       
        /// of 2.                                                                                                
        /// </summary>                                                                                           
        /// <param name="data">The complex data stored as alternating real                                       
        /// and imaginary parts</param>                                                                          
        /// <param name="forward">true for a forward transform, false for                                        
        /// inverse transform</param>                                                                            
        public void RealFFT(double[] data, bool forward)
        {
            var n = data.Length; // # of real inputs, 1/2 the complex length                                     
            // checks n is a power of 2 in 2's complement format                                                 
            if ((n & (n - 1)) != 0)
                throw new ArgumentException(
                    "data length " + n + " in FFT is not a power of 2"
                    );

            var sign = -1.0; // assume inverse FFT, this controls how algebra below works                        
            if (forward)
            { // do packed FFT. This can be changed to FFT to save memory                                        
                TableFFT(data, true);
                sign = 1.0;
                // scaling - divide by scaling for N/2, then mult by scaling for N                               
                if (A != 1)
                {
                    var scale = Math.Pow(2.0, (A - 1) / 2.0);
                    for (var i = 0; i < data.Length; ++i)
                        data[i] *= scale;
                }
            }

            var theta = B * sign * 2 * Math.PI / n;
            var wpr = Math.Cos(theta);
            var wpi = Math.Sin(theta);
            var wjr = wpr;
            var wji = wpi;

            for (var j = 1; j <= n / 4; ++j)
            {
                var k = n / 2 - j;
                var tkr = data[2 * k];    // real and imaginary parts of t_k  = t_(n/2 - j)                      
                var tki = data[2 * k + 1];
                var tjr = data[2 * j];    // real and imaginary parts of t_j                                     
                var tji = data[2 * j + 1];

                var a = (tjr - tkr) * wji;
                var b = (tji + tki) * wjr;
                var c = (tjr - tkr) * wjr;
                var d = (tji + tki) * wji;
                var e = (tjr + tkr);
                var f = (tji - tki);

                // compute entry y[j]                                                                            
                data[2 * j] = 0.5 * (e + sign * (a + b));
                data[2 * j + 1] = 0.5 * (f + sign * (d - c));

                // compute entry y[k]                                                                            
                data[2 * k] = 0.5 * (e - sign * (b + a));
                data[2 * k + 1] = 0.5 * (sign * (d - c) - f);

                var temp = wjr;
                // todo - allow more accurate version here? make option?                                         
                wjr = wjr * wpr - wji * wpi;
                wji = temp * wpi + wji * wpr;
            }

            if (forward)
            {
                // compute final y0 and y_{N/2}, store in data[0], data[1]                                       
                var temp = data[0];
                data[0] += data[1];
                data[1] = temp - data[1];
            }
            else
            {
                var temp = data[0]; // unpack the y0 and y_{N/2}, then invert FFT                                
                data[0] = 0.5 * (temp + data[1]);
                data[1] = 0.5 * (temp - data[1]);
                // do packed inverse (table based) FFT. This can be changed to regular inverse FFT to save memory
                TableFFT(data, false);
                // scaling - divide by scaling for N, then mult by scaling for N/2                               
                //if (A != -1) // todo - off by factor of 2? this works, but something seems weird               
                {
                    var scale = Math.Pow(2.0, -(A + 1) / 2.0) * 2;
                    for (var i = 0; i < data.Length; ++i)
                        data[i] *= scale;
                }
            }
        }

        /// <summary>                                                                                            
        /// Determine how scaling works on the forward and inverse transforms.                                   
        /// For size N=2^n transforms, the forward transform gets divided by                                     
        /// N^((1-a)/2) and the inverse gets divided by N^((1+a)/2). Common                                      
        /// values for (A,B) are                                                                                 
        ///     ( 0, 1)  - default                                                                               
        ///     (-1, 1)  - data processing                                                                       
        ///     ( 1,-1)  - signal processing                                                                     
        /// Usual values for A are 1, 0, or -1                                                                   
        /// </summary>                                                                                           
        public int A { get; set; }

        /// <summary>                                                                                            
        /// Determine how phase works on the forward and inverse transforms.                                     
        /// For size N=2^n transforms, the forward transform uses an                                             
        /// exp(B*2*pi/N) term and the inverse uses an exp(-B*2*pi/N) term.                                      
        /// Common values for (A,B) are                                                                          
        ///     ( 0, 1)  - default                                                                               
        ///     (-1, 1)  - data processing                                                                       
        ///     ( 1,-1)  - signal processing                                                                     
        /// Abs(B) should be relatively prime to N.                                                              
        /// Setting B=-1 effectively corresponds to conjugating both input and                                   
        /// output data.                                                                                         
        /// Usual values for B are 1 or -1.                                                                      
        /// </summary>                                                                                           
        public int B { get; set; }

        public LomontFFT()
        {
            A = 0;
            B = 1;
        }

        #region Internals                                                                                        

        /// <summary>                                                                                            
        /// Scale data using n samples for forward and inverse transforms as needed                              
        /// </summary>                                                                                           
        /// <param name="data"></param>                                                                          
        /// <param name="n"></param>                                                                             
        /// <param name="forward"></param>                                                                       
        void Scale(double[] data, int n, bool forward)
        {
            // forward scaling if needed                                                                         
            if ((forward) && (A != 1))
            {
                var scale = Math.Pow(n, (A - 1) / 2.0);
                for (var i = 0; i < data.Length; ++i)
                    data[i] *= scale;
            }

            // inverse scaling if needed                                                                         
            if ((!forward) && (A != -1))
            {
                var scale = Math.Pow(n, -(A + 1) / 2.0);
                for (var i = 0; i < data.Length; ++i)
                    data[i] *= scale;
            }
        }

        void Scale(float[] data, int n, bool forward)
        {
            // forward scaling if needed                                                                         
            if ((forward) && (A != 1))
            {
                var scale = Math.Pow(n, (A - 1) / 2.0);
                for (var i = 0; i < data.Length; ++i)
                    data[i] *= (float)scale;
            }

            // inverse scaling if needed                                                                         
            if ((!forward) && (A != -1))
            {
                var scale = Math.Pow(n, -(A + 1) / 2.0);
                for (var i = 0; i < data.Length; ++i)
                    data[i] *= (float)scale;
            }
        }

        /// <summary>                                                                                            
        /// Call this with the size before using the TableFFT version                                            
        /// Fills in tables for speed. Done automatically in TableFFT                                            
        /// </summary>                                                                                           
        /// <param name="size">The size of the FFT in samples</param>                                            
        void Initialize(int size)
        {
            // NOTE: if you port to non garbage collected languages                                              
            // like C# or Java be sure to free these correctly                                                   
            cosTable = new double[size];
            sinTable = new double[size];

            // forward pass                                                                                      
            var n = size;
            int mmax = 1, pos = 0;
            while (n > mmax)
            {
                var istep = 2 * mmax;
                var theta = Math.PI / mmax;
                double wr = 1, wi = 0;
                var wpi = Math.Sin(theta);
                // compute in a slightly slower yet more accurate manner                                         
                var wpr = Math.Sin(theta / 2);
                wpr = -2 * wpr * wpr;
                for (var m = 0; m < istep; m += 2)
                {
                    cosTable[pos] = wr;
                    sinTable[pos++] = wi;
                    var t = wr;
                    wr = wr * wpr - wi * wpi + wr;
                    wi = wi * wpr + t * wpi + wi;
                }
                mmax = istep;
            }
        }

        /// <summary>                                                                                            
        /// Swap data indices whenever index i has binary                                                        
        /// digits reversed from index j, where data is                                                          
        /// two doubles per index.                                                                               
        /// </summary>                                                                                           
        /// <param name="data"></param>                                                                          
        /// <param name="n"></param>                                                                             
        static void Reverse(double[] data, int n)
        {
            // bit reverse the indices. This is exercise 5 in section                                            
            // 7.2.1.1 of Knuth's TAOCP the idea is a binary counter                                             
            // in k and one with bits reversed in j                                                              
            int j = 0, k = 0; // Knuth R1: initialize                                                            
            var top = n / 2;  // this is Knuth's 2^(n-1)                                                         
            while (true)
            {
                // Knuth R2: swap - swap j+1 and k+2^(n-1), 2 entries each                                       
                var t = data[j + 2];
                data[j + 2] = data[k + n];
                data[k + n] = t;
                t = data[j + 3];
                data[j + 3] = data[k + n + 1];
                data[k + n + 1] = t;
                if (j > k)
                { // swap two more                                                                               
                    // j and k                                                                                   
                    t = data[j];
                    data[j] = data[k];
                    data[k] = t;
                    t = data[j + 1];
                    data[j + 1] = data[k + 1];
                    data[k + 1] = t;
                    // j + top + 1 and k+top + 1                                                                 
                    t = data[j + n + 2];
                    data[j + n + 2] = data[k + n + 2];
                    data[k + n + 2] = t;
                    t = data[j + n + 3];
                    data[j + n + 3] = data[k + n + 3];
                    data[k + n + 3] = t;
                }
                // Knuth R3: advance k                                                                           
                k += 4;
                if (k >= n)
                    break;
                // Knuth R4: advance j                                                                           
                var h = top;
                while (j >= h)
                {
                    j -= h;
                    h /= 2;
                }
                j += h;
            } // bit reverse loop                                                                                
        }

        static void Reverse(float[] data, int n)
        {
            // bit reverse the indices. This is exercise 5 in section                                            
            // 7.2.1.1 of Knuth's TAOCP the idea is a binary counter                                             
            // in k and one with bits reversed in j                                                              
            int j = 0, k = 0; // Knuth R1: initialize                                                            
            var top = n / 2;  // this is Knuth's 2^(n-1)                                                         
            while (true)
            {
                // Knuth R2: swap - swap j+1 and k+2^(n-1), 2 entries each                                       
                var t = data[j + 2];
                data[j + 2] = data[k + n];
                data[k + n] = t;
                t = data[j + 3];
                data[j + 3] = data[k + n + 1];
                data[k + n + 1] = t;
                if (j > k)
                { // swap two more                                                                               
                    // j and k                                                                                   
                    t = data[j];
                    data[j] = data[k];
                    data[k] = t;
                    t = data[j + 1];
                    data[j + 1] = data[k + 1];
                    data[k + 1] = t;
                    // j + top + 1 and k+top + 1                                                                 
                    t = data[j + n + 2];
                    data[j + n + 2] = data[k + n + 2];
                    data[k + n + 2] = t;
                    t = data[j + n + 3];
                    data[j + n + 3] = data[k + n + 3];
                    data[k + n + 3] = t;
                }
                // Knuth R3: advance k                                                                           
                k += 4;
                if (k >= n)
                    break;
                // Knuth R4: advance j                                                                           
                var h = top;
                while (j >= h)
                {
                    j -= h;
                    h /= 2;
                }
                j += h;
            } // bit reverse loop                                                                                
        }

        /// <summary>                                                                                            
        /// Pre-computed sine/cosine tables for speed                                                            
        /// </summary>                                                                                           
        double[] cosTable;
        double[] sinTable;

        #endregion

    }
}