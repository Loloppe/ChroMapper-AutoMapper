﻿using Automapper.Items;
using Beatmap.Base;
using Beatmap.Helper;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using static Automapper.Items.Enumerator;
using Color = UnityEngine.Color;
using Options = Automapper.Items.Options.Light;

namespace Automapper.Methods
{
    internal static class Light
    {
        internal static List<BaseEvent> CreateLight(List<BaseNote> Notes, List<BaseNote> Selection)
        {
            // Bunch of var to keep timing in check
            float last = new float();
            float[] time = new float[4];
            int[] light = new int[3];
            float offset = Notes[0].JsonTime;
            float firstNote = 0;
            float timer = 0;

            //Light counter, stop at maximum.
            int count;

            // For laser speed
            int currentSpeed = 3;

            // Rhythm check
            float lastSpeed = 0;

            // To not light up Double twice
            float nextDouble = 0;

            // Slider stuff
            bool firstSlider = false;
            float nextSlider = new float();
            List<int> sliderLight = new List<int>() { 4, 1, 0 };
            int sliderIndex = 0;
            float sliderNoteCount = 0;
            bool wasSlider = false;

            // Pattern for specific rhythm
            List<int> pattern = new List<int>(Enumerable.Range(0, 5));
            int patternIndex = 0;
            int patternCount = 20;

            // The new events
            List<BaseEvent> eventTempo = new List<BaseEvent>();

            // Is the section currently using Boost Event
            bool boost = true;
            float boostSwap = Options.ColorBoostSwap;
            float boostIncrement = 0;

            // If double notes lights are on
            bool doubleOn = false;

            // Make sure this is the right timing for color swap with Boost Event
            float ColorOffset = Options.ColorOffset;
            float ColorSwap = Options.ColorSwap;

            // To make sure that slider doesn't apply as double
            List<BaseNote> sliderTiming = new List<BaseNote>();

            // Order note, necessary if we're converting V3 bomb from notes
            Notes = Notes.OrderBy(o => o.JsonTime).ToList();
            Selection = Selection.OrderBy(o => o.JsonTime).ToList();

            // Find color based on waveform
            var colors = new List<(float beat, Color color)>();
            if(Options.Chroma)
            {
                var results = Onset.GetOnsets("song.ogg", BeatSaberSongContainer.Instance.Song.BeatsPerMinute);
                
                foreach(var result in results)
                {
                    var X = result.Item2;
                    int red = Math.Min((int)(X * 256), 255);
                    int green = Math.Min((int)((X * 256 - red) * 256), 255);
                    int blue = Math.Min((int)(((X * 256 - red) * 256 - green) * 256), 255);

                    Color color = new Color((float)red / 255, (float)green / 255, (float)blue / 255, 1);
                    colors.Add((result.Item1, color));
                }
            }

            void ResetTimer() //Pretty much reset everything necessary.
            {
                firstNote = Notes[0].JsonTime;
                offset = firstNote;
                boostIncrement = firstNote;
                count = 1;
                for (int i = 0; i < 2; i++)
                {
                    time[i] = 0.0f;
                    light[i] = 0;
                }
                time[2] = 0.0f;
                time[3] = 0.0f;
            }

            ResetTimer();

            bool found = false;

            // Place all spin/zoom/boost
            foreach(BaseNote note in Notes)
            {
                float now = note.JsonTime;
                time[0] = now;

                //Here we process Spin and Zoom
                if (now == firstNote && time[1] == 0.0D) //If we are processing the first note, add spin + zoom + boost to it.
                {
                    eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.SPIN, Value = 0});
                    eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.ZOOM, Value = 0});
                    if (Options.UseBoostColor)
                    {
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.BOOST, Value = 0});
                        boost = false;
                    }
                }
                else if (now >= Options.ColorOffset + Options.ColorSwap + offset && now > firstNote) //If we are reaching the next threshold of the timer
                {
                    int calc = (int)((int)(now - offset) / Options.ColorSwap);

                    for (int i = 0; i < calc; i++)
                    {
                        offset += Options.ColorSwap;

                        //Add a spin at timer.
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.SPIN, Value = 0});
                        if (count == 0) //Only add zoom every 2 spin.
                        {
                            eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.ZOOM, Value = 0});
                            count = 1;
                        }
                        else
                        {
                            count--;
                        }
                    }
                }
                //If there's a quarter between two float parallel notes and timer didn't pass the check.
                else if (time[1] - time[2] == 0.25 && time[3] == time[2] && time[1] == now && timer < offset)
                {
                    eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.SPIN, Value = 0});
                }

                // Boost Event
                if (now >= Options.ColorOffset + Options.ColorBoostSwap + boostIncrement && now > firstNote && Options.UseBoostColor)
                {
                    int calc = (int)((int)(now - boostIncrement) / Options.ColorBoostSwap);

                    for (int i = 0; i < calc; i++)
                    {
                        boostIncrement += Options.ColorBoostSwap;

                        if (boost)
                        {
                            eventTempo.Add(new BaseEvent{ JsonTime = boostIncrement, Type = EventType.BOOST, Value = 0});
                            boost = false;
                        }
                        else
                        {
                            eventTempo.Add(new BaseEvent{ JsonTime = boostIncrement, Type = EventType.BOOST, Value = 0});
                            boost = true;
                        }
                    }
                }

                for (int i = 3; i > 0; i--) //Keep the timing of up to three notes before.
                {
                    time[i] = time[i - 1];
                }
            }

            ResetTimer();

            // Find all sliders
            for (int i = 1; i < Selection.Count; i++)
            {
                // Between 1/8 and 0, same cut direction or dots
                if (Notes[i].JsonTime - Notes[i - 1].JsonTime <= 0.125 && Notes[i].JsonTime - Notes[i - 1].JsonTime > 0 && (Notes[i].CutDirection == Notes[i - 1].CutDirection || Notes[i].CutDirection == 8 || Notes[i - 1].CutDirection == 8))
                {
                    sliderTiming.Add(Notes[i - 1]);
                    found = true;
                }
                else if (found)
                {
                    sliderTiming.Add(Notes[i - 1]);
                    found = false;
                }
            }

            foreach (BaseNote note in Selection) //Process specific light using time.
            {
                float now = note.JsonTime;
                time[0] = now;

                if (!Options.NerfStrobes && doubleOn && now != last) //Off event
                {
                    if (now - last >= 1)
                    {
                        eventTempo.Add(new BaseEvent{ JsonTime = now - (now - last) / 2, Type = EventType.BACK, Value = 0 });
                        eventTempo.Add(new BaseEvent{ JsonTime = now - (now - last) / 2, Type = EventType.RING, Value = 0 });
                        eventTempo.Add(new BaseEvent{ JsonTime = now - (now - last) / 2, Type = EventType.SIDE, Value = 0 });
                        eventTempo.Add(new BaseEvent{ JsonTime = now - (now - last) / 2, Type = EventType.LEFT, Value = 0 });
                        eventTempo.Add(new BaseEvent{ JsonTime = now - (now - last) / 2, Type = EventType.RIGHT, Value = 0 });
                    }
                    else
                    {
                        // Will be fused with some events, but we will sort that out later on.
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.BACK, Value = 0 });
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.RING, Value = 0 });
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.SIDE, Value = 0 });
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.LEFT, Value = 0 });
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.RIGHT, Value = 0 });
                    }

                    doubleOn = false;
                }

                //If not same note, same beat and not slider, apply once.
                if ((now == time[1] || (now - time[1] <= 0.02 && time[1] != time[2])) && (time[1] != 0.0D && now != last) && !sliderTiming.Exists(e => e.JsonTime == now))
                {
                    int color = FindColor(Notes.First().JsonTime, time[0]);
                    if (Options.Chroma)
                    {
                        var chroma = colors.LastOrDefault(x => x.beat <= now).color;
                        eventTempo.Add(ChromaGen(now, EventType.BACK, color, chroma)); //Back Top Laser
                        eventTempo.Add(ChromaGen(now, EventType.RING, color, chroma)); //Track Ring Neons
                        eventTempo.Add(ChromaGen(now, EventType.SIDE, color, chroma)); //Side Light
                        eventTempo.Add(ChromaGen(now, EventType.LEFT, color, chroma)); //Left Laser
                        eventTempo.Add(ChromaGen(now, EventType.RIGHT, color, chroma)); //Right Laser
                    }
                    else
                    {
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.BACK, Value = color}); //Back Top Laser
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.RING, Value = color}); //Track Ring Neons
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.SIDE, Value = color}); //Side Light
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.LEFT, Value = color}); //Left Laser
                        eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.RIGHT, Value = color}); //Right Laser
                    }

                    // Laser speed based on rhythm
                    if (time[0] - time[1] < 0.25)
                    {
                        currentSpeed = 7;
                    }
                    else if (time[0] - time[1] >= 0.25 && time[0] - time[1] < 0.5)
                    {
                        currentSpeed = 5;
                    }
                    else if (time[0] - time[1] >= 0.5 && time[0] - time[1] < 1)
                    {
                        currentSpeed = 3;
                    }
                    else
                    {
                        currentSpeed = 1;
                    }

                    eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.LEFT_ROT, Value = currentSpeed}); //Left Rotation
                    eventTempo.Add(new BaseEvent{ JsonTime = now, Type = EventType.RIGHT_ROT, Value = currentSpeed}); //Right Rotation

                    doubleOn = true;
                    last = now;
                }

                for (int i = 3; i > 0; i--) //Keep the timing of up to three notes before.
                {
                    time[i] = time[i - 1];
                }
            }

            nextSlider = new float();

            // Convert quick light color swap
            if (Options.NerfStrobes)
            {
                float lastTimeTop = 100;
                float lastTimeNeon = 100;
                float lastTimeSide = 100;
                float lastTimeLeft = 100;
                float lastTimeRight = 100;

                foreach (var x in eventTempo)
                {
                    if (x.Type == EventType.BACK)
                    {
                        if (x.JsonTime - lastTimeTop <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeTop = x.JsonTime;
                    }
                    else if (x.Type == EventType.RING)
                    {
                        if (x.JsonTime - lastTimeNeon <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeNeon = x.JsonTime;
                    }
                    else if (x.Type == EventType.SIDE)
                    {
                        if (x.JsonTime - lastTimeSide <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeSide = x.JsonTime;
                    }
                    else if (x.Type == EventType.LEFT)
                    {
                        if (x.JsonTime - lastTimeLeft <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeLeft = x.JsonTime;
                    }
                    else if (x.Type == EventType.RIGHT)
                    {
                        if (x.JsonTime - lastTimeRight <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeRight = x.JsonTime;
                    }
                }
            }

            ResetTimer();

            foreach (BaseNote note in Selection) //Process all note using time.
            {
                time[0] = note.JsonTime;

                if (wasSlider)
                {
                    if (sliderNoteCount != 0)
                    {
                        sliderNoteCount--;

                        for (int i = 3; i > 0; i--) //Keep the timing of up to three notes before.
                        {
                            time[i] = time[i - 1];
                        }
                        continue;
                    }
                    else
                    {
                        wasSlider = false;
                    }
                }

                if (firstSlider)
                {
                    firstSlider = false;
                    continue;
                }

                // Find the next double
                if (time[0] >= nextDouble)
                {
                    for (int i = Selection.FindIndex(n => n == note); i < Selection.Count - 1; i++)
                    {
                        if (i != 0)
                        {
                            if (Selection[i].JsonTime == Selection[i - 1].JsonTime)
                            {
                                nextDouble = Selection[i].JsonTime;
                                break;
                            }
                        }
                    }
                }

                // Find the next slider (1/8 minimum) or chain
                if (time[0] >= nextSlider)
                {
                    sliderNoteCount = 0;

                    for (int i = Selection.FindIndex(n => n == note); i < Selection.Count - 1; i++)
                    {
                        if (i != 0 && i < Selection.Count)
                        {
                            // Between 1/8 and 0, same cut direction or dots
                            if (Selection[i].JsonTime - Selection[i - 1].JsonTime <= 0.125 && Selection[i].JsonTime - Selection[i - 1].JsonTime > 0 && (Selection[i].CutDirection == Selection[i - 1].CutDirection || Selection[i].CutDirection == 8))
                            {
                                // Search for the last note of the slider
                                if (sliderNoteCount == 0)
                                {
                                    // This is the first note of the slider
                                    nextSlider = Selection[i - 1].JsonTime;
                                }
                                sliderNoteCount++;
                            }
                            else if (sliderNoteCount != 0)
                            {
                                break;
                            }
                        }
                    }
                }

                // It's the next slider or chain
                if (nextSlider == note.JsonTime)
                {
                    // Take a light between neon, side or backlight and strobes it via On/Flash
                    if (sliderIndex == -1)
                    {
                        sliderIndex = 2;
                    }

                    // Place light
                    int color = FindColor(Notes.First().JsonTime, time[0]);
                    if(Options.Chroma)
                    {
                        var chroma = colors.LastOrDefault(x => x.beat <= time[0]).color;
                        eventTempo.Add(ChromaGen(time[0], sliderLight[sliderIndex], color - 2, chroma));
                        eventTempo.Add(ChromaGen(time[0] + 0.125f, sliderLight[sliderIndex], color - 1, chroma));
                        eventTempo.Add(ChromaGen(time[0] + 0.25f, sliderLight[sliderIndex], color - 2, chroma));
                        eventTempo.Add(ChromaGen(time[0] + 0.375f, sliderLight[sliderIndex], color - 1, chroma));
                        eventTempo.Add(ChromaGen(time[0] + 0.5f, sliderLight[sliderIndex], color - 0, chroma));
                    }
                    else
                    {
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0], Type = sliderLight[sliderIndex], Value = color - 2 });
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0] + 0.125f, Type = sliderLight[sliderIndex], Value = color - 1 });
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0] + 0.25f, Type = sliderLight[sliderIndex], Value = color - 2 });
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0] + 0.375f, Type = sliderLight[sliderIndex], Value = color - 1 });
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0] + 0.5f, Type = sliderLight[sliderIndex], Value = 0 });
                    }

                    sliderIndex--;

                    // Spin goes brrr
                    eventTempo.Add(new BaseEvent{ JsonTime = time[0], Type = EventType.SPIN, Value = 0});
                    for (int i = 0; i < 8; i++)
                    {
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0] + 0.5f - (0.5f / 8f * i), Type = EventType.SPIN, Value = 0 });
                    }

                    wasSlider = true;
                }
                // Not a double
                else if (time[0] != nextDouble)
                {
                    if (time[1] - time[2] >= lastSpeed + 0.02 || time[1] - time[2] <= lastSpeed - 0.02 || patternCount == 20) // New speed or 20 notes of the same pattern
                    {
                        int old = 0;
                        // New pattern
                        if (patternIndex != 0)
                        {
                            old = pattern[patternIndex - 1];
                        }
                        else
                        {
                            old = pattern[4];
                        }

                        do
                        {
                            pattern.Shuffle();
                        } while (pattern[0] == old);
                        patternIndex = 0;
                        patternCount = 0;
                    }

                    // Place the next light
                    if (Options.Chroma)
                    {
                        var chroma = colors.LastOrDefault(x => x.beat <= time[0]).color;
                        eventTempo.Add(ChromaGen(time[0], pattern[patternIndex], FindColor(Notes.First().JsonTime, time[0]), chroma));
                    }
                    else
                    {
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0], Type = pattern[patternIndex], Value = FindColor(Notes.First().JsonTime, time[0]) });
                    }

                    // Speed based on rhythm
                    if (time[0] - time[1] < 0.25)
                    {
                        currentSpeed = 7;
                    }
                    else if (time[0] - time[1] >= 0.25 && time[0] - time[1] < 0.5)
                    {
                        currentSpeed = 5;
                    }
                    else if (time[0] - time[1] >= 0.5 && time[0] - time[1] < 1)
                    {
                        currentSpeed = 3;
                    }
                    else
                    {
                        currentSpeed = 1;
                    }

                    // Add laser rotation if necessary
                    if (pattern[patternIndex] == 2)
                    {
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0], Type = EventType.LEFT_ROT, Value = currentSpeed });
                    }
                    else if (pattern[patternIndex] == 3)
                    {
                        eventTempo.Add(new BaseEvent{ JsonTime = time[0], Type = EventType.RIGHT_ROT, Value = currentSpeed });
                    }

                    // Place off event
                    if (Selection[Selection.Count - 1].JsonTime != note.JsonTime)
                    {
                        if (Selection[Selection.FindIndex(n => n == note) + 1].JsonTime == nextDouble)
                        {
                            if (Selection[Selection.FindIndex(n => n == note) + 1].JsonTime - time[0] <= 2)
                            {
                                float value = (Selection[Selection.FindIndex(n => n == note) + 1].JsonTime - Selection[Selection.FindIndex(n => n == note)].JsonTime) / 2;
                                eventTempo.Add(new BaseEvent{ JsonTime = Selection[Selection.FindIndex(n => n == note)].JsonTime + value, Type = pattern[patternIndex], Value = 0 });
                            }
                        }
                        else
                        {
                            eventTempo.Add(new BaseEvent{ JsonTime = Selection[Selection.FindIndex(n => n == note) + 1].JsonTime, Type = pattern[patternIndex], Value = 0 });
                        }
                    }

                    // Pattern have 5 notes in total (5 lights available)
                    if (patternIndex < 4)
                    {
                        patternIndex++;
                    }
                    else
                    {
                        patternIndex = 0;
                    }

                    patternCount++;
                    lastSpeed = time[0] - time[1];
                }

                for (int i = 3; i > 0; i--) //Keep the timing of up to three notes before.
                {
                    time[i] = time[i - 1];
                }
            }

            eventTempo = eventTempo.OrderBy(o => o.JsonTime).ToList();

            // Remove fused or move off event between
            eventTempo = RemoveFused(eventTempo);

            // Sort lights
            eventTempo = eventTempo.OrderBy(o => o.JsonTime).ToList();

            return eventTempo;
        }

        static public List<BaseEvent> RemoveFused(List<BaseEvent> events)
        {
            float? closest = 0f;

            // Get all fused events of a specific type
            for (int i = 0; i < events.Count; i++)
            {
                BaseEvent e = events[i];

                BaseEvent MapEvent = events.Find(o => o.Type == e.Type && (o.JsonTime - e.JsonTime >= -0.02 && o.JsonTime - e.JsonTime <= 0.02) && o != e);
                if (MapEvent != null)
                {
                    BaseEvent MapEvent2 = events.Find(o => o.Type == MapEvent.Type && (o.JsonTime - MapEvent.JsonTime >= -0.02 && o.JsonTime - MapEvent.JsonTime <= 0.02) && o != MapEvent);

                    if (MapEvent2 != null)
                    {
                        BaseEvent temp = events.FindLast(o => o.JsonTime < e.JsonTime && e.JsonTime > closest && o.Value != 0);

                        if (temp != null)
                        {
                            closest = temp.JsonTime;

                            if (MapEvent2.Value == EventLightValue.OFF)
                            {
                                // Move off event between fused note and last note
                                events[(events.FindIndex(o => o.JsonTime == MapEvent2.JsonTime && o.Value == MapEvent2.Value && o.Type == MapEvent2.Type))].JsonTime = (float)(MapEvent2.JsonTime - ((MapEvent2.JsonTime - closest) / 2));
                            }
                            else
                            {
                                // Move off event between fused note and last note
                                if (MapEvent.Value == EventLightValue.OFF || MapEvent.Value == EventLightValue.BLUE_TRANSITION || MapEvent.Value == EventLightValue.RED_TRANSITION)
                                {
                                    events[(events.FindIndex(o => o.JsonTime == MapEvent.JsonTime && o.Value == MapEvent.Value && o.Type == MapEvent.Type))].JsonTime = (float)(MapEvent.JsonTime - ((MapEvent.JsonTime - closest) / 2));
                                }
                                else // Delete event
                                {
                                    events.RemoveAt(events.FindIndex(o => o.JsonTime == MapEvent.JsonTime && o.Value == MapEvent.Value && o.Type == MapEvent.Type));
                                }
                            }
                        }
                    }
                }
            }

            return events;
        }

        static public int FindColor(float first, float current)
        {
            int color = EventLightValue.RED_FADE;

            for (int i = 0; i < ((current - first + Options.ColorOffset) / Options.ColorSwap); i++) //For each time that it need to swap.
            {
                color = Utils.Inverse(color); //Swap color
            }

            if(first == current)
            {
                color = EventLightValue.BLUE_FADE;
            }

            return color;
        }

        static public BaseEvent ChromaGen(float beat, int type, int value, Color color)
        {
            var data = new BaseEvent { JsonTime = beat, Type = type, Value = value };
            if (color == null || color == new Color(0, 0, 0, 0))
            {
                return data;
            }
            data.CustomColor = color;
            data.GetOrCreateCustom()[data.CustomKeyColor] = (new JSONArray()).WriteColor(color, true);
            data.RefreshCustom();
            return data;
        }
    }
}
