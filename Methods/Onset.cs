using Automapper.Items;
using Automapper.Onset;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Automapper.Methods
{
    internal class Onset
    {
        static public AudioAnalysis audioAnalysis = new AudioAnalysis();

        static public List<BeatmapNote> GetMap(string audioPath, float bpm)
        {
            // New list of notes and chain, will be filled via PatternCreator.cs
            List<BeatmapNote> notes = new List<BeatmapNote>();
            List<float> timings = new List<float>();

            double offset = 0;
            if (Path.GetExtension(audioPath) == ".mp3")
            {
                offset = 0.125;
            }
            else
            {
                offset = 0.05;
            }

            try
            {
                // 0 = Average between 0 and Max. 1 = Average between Min and Max
                // indistinguishableRange = Modify the number of set of samples to ignore after an onset
                AnalyseSong(audioPath, 1);

                // List with all the onsets (even the one that we don't want)
                List<float> onsets = audioAnalysis.GetOnsets().ToList();

                
                int number = 0;
                double lastBeat = -1;
                double lastOnset = -1;


                bool wasDouble = false;

                // Convert SampleSize and SampleRate into time
                double milisec = audioAnalysis.GetTimePerSample();

                foreach (float onset in onsets)
                {
                    number++;
                    // If it match, get timing and create a new note with it
                    if (onset >= 0.01)
                    {
                        double time = number * milisec; // Get the time for the current onset
                        double beat = (time * bpm / 60) - offset; // Convert the time into beat

                        if (beat - lastBeat >= Options.Mapper.MaxSpeed)
                        {
                            // Add a new timing
                            timings.Add((float)beat);

                            if (onset >= Options.Mapper.DoubleThreshold)
                            {
                                // Add another timing to create a double note on same beat
                                timings.Add((float)beat);
                                wasDouble = true;
                            }
                            else
                            {
                                // New timing wasn't a double
                                wasDouble = false;
                            }

                            lastBeat = beat;
                            lastOnset = onset;
                        }
                        else
                        {
                            if (onset > lastOnset) // Higher onset take priority
                            {
                                // Modify the timing of the last note
                                timings.Remove(timings.Last());
                                timings.Add((float)beat);

                                if (wasDouble)
                                {
                                    // Modify the timing of the last double note to match the current new timing
                                    timings[timings.Count - 2] = (float)beat;
                                }
                                else if (onset >= Options.Mapper.DoubleThreshold)
                                {
                                    // Add a new timing to create a double since last timing wasn't a double
                                    timings.Add((float)beat);
                                    wasDouble = true;
                                }

                                lastBeat = beat;
                                lastOnset = onset;
                            }
                        }
                    }
                }

                // Find double on same beat with note closer than DOUBLE_LIMITER before or after the double and remove a note of the double (to fix burst issue)
                for (int i = 2; i < timings.Count - 1; i++)
                {
                    if (timings[i - 1] == timings[i] && (timings[i - 1] - timings[i - 2] <= Options.Mapper.MaxDoubleSpeed || timings[i + 1] - timings[i] <= Options.Mapper.MaxDoubleSpeed))
                    {
                        timings.RemoveAt(i);
                        i--;
                    }
                }
                // Method to generate the map pattern
                notes = NoteGenerator.AutoMapper(timings, bpm, 1, null, null);
                
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                audioAnalysis.DisposeAudioAnalysis();
            }

            return notes;
        }

        static void AnalyseSong(string filePath, int type)
        {
            // Load the audio file from the given file path
            audioAnalysis.LoadAudioFromFile(filePath);
            // Find the onsets
            audioAnalysis.DetectOnsets(Options.Mapper.OnsetSensitivity, Options.Mapper.IndistinguishableRange);
            // Normalize the intensity of the onsets
            audioAnalysis.NormalizeOnsets(type);
        }
    }
}
