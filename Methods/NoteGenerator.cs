using Automapper.Items;
using System.Collections.Generic;
using static Automapper.Items.Enumerator;
using static Automapper.Items.Helper;

namespace Automapper.Methods
{
    internal class NoteGenerator
    {
        /// <summary>
        /// Method to generate new BeatmapNote and BurstSliderData from timings (in beat).
        /// This will create a new Beat Saber map from scratch (minus the timings).
        /// </summary>
        /// <param name="timings">Time (in beat) to generate the note</param>
        /// <param name="bpm">Main BPM of the song</param>
        /// <param name="limiter">Allow backhanded when off</param>
        /// <returns>Notes and Chains (for now)</returns>
        static public List<BeatmapNote> AutoMapper(List<float> timings, float bpm, int hand, BeatmapNote lastRed, BeatmapNote lastBlue)
        {
            // Our main list where we will store the generated Notes and Chains.
            List<BeatmapNote> notes = new List<BeatmapNote>();

            if(!Options.Mapper.GenerateAsTiming)
            {
                // Keep the player wrist rotation via direction.
                // LEFT SIDE
                // Upper limit (tech): 0 (Up), lower limit (tech): 2 (Left)
                // Upper limit: 3 (Right), lower limit: 6 (Down-Left)
                // RIGHT SIDE (vertical mirror)
                // Upper limit (tech): 0 (Up), lower limit (tech): 3 (Right)
                // Upper limit: 2 (Left), lower limit: 7 (Down-Right)
                int leftDirection = 1;
                int rightDirection = 1;
                // The last swing. Upswing = 0, Downswing = 1
                int leftSwing = 1;
                int rightSwing = 1;

                // The current direction being selected for the next note.
                int direction = -1;

                // The expected speed, used to choose between tech or normal type of flow (in beat). 1+ beat = extreme, 0.5 - 1 beat = tech, 0.5- = normal.
                // Based 
                float speed;
                float lastLeft = 0;
                float lastRight = 0;

                // Selected line and layer
                int line = 0;
                int layer = 0;

                if (lastRed != null)
                {
                    if (SwingType.Down.Contains(lastRed.CutDirection))
                    {
                        leftSwing = 1;
                    }
                    else if (SwingType.Up.Contains(lastRed.CutDirection))
                    {
                        leftSwing = 0;
                    }
                    else if (SwingType.Left.Contains(lastRed.CutDirection))
                    {
                        leftSwing = 0;
                    }
                    else if (SwingType.Right.Contains(lastRed.CutDirection))
                    {
                        leftSwing = 1;
                    }
                    lastLeft = lastRed.Time;
                    leftDirection = lastRed.CutDirection;
                }
                if (lastBlue != null)
                {
                    if (SwingType.Down.Contains(lastBlue.CutDirection))
                    {
                        rightSwing = 1;
                    }
                    else if (SwingType.Up.Contains(lastBlue.CutDirection))
                    {
                        rightSwing = 0;
                    }
                    else if (SwingType.Left.Contains(lastBlue.CutDirection))
                    {
                        rightSwing = 1;
                    }
                    else if (SwingType.Right.Contains(lastBlue.CutDirection))
                    {
                        rightSwing = 0;
                    }
                    lastRight = lastBlue.Time;
                    rightDirection = lastBlue.CutDirection;
                }

                // Select all directions
                for (int i = 0; i < timings.Count; i++)
                {
                    float timing = timings[i];

                    if (notes.Count == 0 && lastBlue == null)
                    {
                        BeatmapNote n = new BeatmapNote(timing, 2, 0, 1, 1);
                        notes.Add(n);
                        lastRight = timing;
                        rightSwing = 1;
                        continue;
                    }
                    else if (notes.Count == 1 && lastRed == null)
                    {
                        BeatmapNote n = new BeatmapNote(timing, 1, 0, 0, 1);
                        notes.Add(n);
                        lastLeft = timing;
                        leftSwing = 1;
                        continue;
                    }

                    // Direction are separated for each hand and each timing in step of 2.
                    if (hand == 0) // Red
                    {
                        // Get the current expected speed
                        speed = timing - lastLeft;
                        // If the BPM is above 250, we want to start restricting the speed
                        if (bpm >= 250)
                        {
                            speed = 250 / bpm * speed;
                        }

                        direction = NextDirection(leftDirection, leftSwing, hand, speed, Options.Mapper.Limiter);

                        // We track the data for the next note
                        if (leftSwing == 0)
                        {
                            leftSwing = 1;
                        }
                        else if (leftSwing == 1)
                        {
                            leftSwing = 0;
                        }
                        leftDirection = direction;
                        lastLeft = timing;
                    }
                    else if (hand == 1) // Blue
                    {
                        // Get the current expected speed
                        speed = timing - lastRight;
                        // If the BPM is above 250, we want to start restricting the speed
                        if (bpm >= 250)
                        {
                            speed = 250 / bpm * speed;
                        }

                        direction = NextDirection(rightDirection, rightSwing, hand, speed, Options.Mapper.Limiter);

                        // We track the data for the next note
                        if (rightSwing == 0)
                        {
                            rightSwing = 1;
                        }
                        else if (rightSwing == 1)
                        {
                            rightSwing = 0;
                        }
                        rightDirection = direction;
                        lastRight = timing;
                    }

                    // Create the note and add it to the list
                    if (hand == 1)
                    {
                        BeatmapNote note = new BeatmapNote(timing, 2, 0, hand, direction);
                        notes.Add(note);
                        hand = 0; // Switch hand for the next note
                    }
                    else
                    {
                        BeatmapNote note = new BeatmapNote(timing, 1, 0, hand, direction);
                        notes.Add(note);
                        hand = 1; // Switch hand for the next note
                    }
                }

                // Select all lines and layers (should probably be done together)
                for (int i = 0; i < notes.Count; i++)
                {
                    if (notes[i].Type == 0)
                    {
                        (line, layer) = PlacementCheck(notes[i].CutDirection, 0);
                    }
                    else if (notes[i].Type == 1)
                    {
                        (line, layer) = PlacementCheck(notes[i].CutDirection, 1);
                    }

                    notes[i].LineIndex = line;
                    notes[i].LineLayer = layer;

                    if(i > 0)
                    {
                        if (notes[i].Time - notes[i - 1].Time >= -0.02 && notes[i].Time - notes[i - 1].Time <= 0.02)
                        {
                            if (notes[i].Type == 0)
                            {
                                (notes[i], notes[i - 1]) = FixDoublePlacement(notes[i], notes[i - 1]);
                            }
                            else if (notes[i].Type == 1)
                            {
                                (notes[i - 1], notes[i]) = FixDoublePlacement(notes[i - 1], notes[i]);
                            }
                        }

                        if (Options.Mapper.BottomRowOnly)
                        {
                            if (notes[i].Time - notes[i - 1].Time >= -0.02 && notes[i].Time - notes[i - 1].Time <= 0.02)
                            {
                                notes[i - 1].LineLayer = 0;
                            }
                            notes[i].LineLayer = 0;
                        }
                        if (Options.Mapper.RandomizeLine)
                        {
                            if (notes[i].Time - notes[i - 1].Time >= -0.02 && notes[i].Time - notes[i - 1].Time <= 0.02)
                            {
                                if (notes[i - 1].LineLayer != 1)
                                {
                                    notes[i - 1].LineIndex = Utils.RandNumber(0, 4);
                                }
                            }
                            if (notes[i].LineLayer != 1)
                            {
                                notes[i].LineIndex = Utils.RandNumber(0, 4);
                            }
                        }
                        if (Options.Mapper.GenerateFused)
                        {
                            if (notes[i].Time - notes[i - 1].Time >= -0.02 && notes[i].Time - notes[i - 1].Time <= 0.02)
                            {
                                if (Utils.RandNumber(0, 2) == 0)
                                {
                                    notes[i].LineIndex = notes[i - 1].LineIndex;
                                    notes[i].LineLayer = notes[i - 1].LineLayer;
                                }
                                else
                                {
                                    notes[i - 1].LineIndex = notes[i].LineIndex;
                                    notes[i - 1].LineLayer = notes[i].LineLayer;
                                }
                            }
                        }
                    }
                }
            }
            else if(Options.Mapper.GenerateAsTiming)
            {
                foreach(float t in timings)
                {
                    BeatmapNote beatmapNote;

                    if (notes.Exists(o => o.Time == t))
                    {
                        beatmapNote = new BeatmapNote(t, 1, 0, 1, 8);
                    }
                    else
                    {
                        beatmapNote = new BeatmapNote(t, 0, 0, 0, 8);
                    }

                    notes.Add(beatmapNote);
                }
            }

            // We're done
            return notes;
        }
    }
}
