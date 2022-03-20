using Automapper.Items;
using System.Collections.Generic;
using System.Linq;
using static Automapper.Items.Enumerator;
using Options = Automapper.Items.Options.Light;

namespace Automapper.Methods
{
    internal static class Light
    {
        internal static List<MapEvent> CreateLight(List<BeatmapNote> Notes, List<BeatmapNote> Selection, float First)
        {
            // Bunch of var to keep timing in check
            float last = new float();
            float[] time = new float[4];
            int[] light = new int[3];
            float offset = Notes[0].Time;
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
            List<MapEvent> eventTempo = new List<MapEvent>();

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
            List<BeatmapNote> sliderTiming = new List<BeatmapNote>();

            // Order note, necessary if we're converting V3 bomb from notes
            Notes = Notes.OrderBy(o => o.Time).ToList();

            void ResetTimer() //Pretty much reset everything necessary.
            {
                firstNote = Notes[0].Time;
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
            foreach(BeatmapNote note in Notes)
            {
                float now = note.Time;
                time[0] = now;

                //Here we process Spin and Zoom
                if (now == firstNote && time[1] == 0.0D) //If we are processing the first note, add spin + zoom + boost to it.
                {
                    eventTempo.Add(new MapEvent(now, EventType.SPIN, 0, 1));
                    eventTempo.Add(new MapEvent(now, EventType.ZOOM, 0, 1));
                    if (Options.AllowBoostColor)
                    {
                        eventTempo.Add(new MapEvent(now, EventType.BOOST, 0));
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
                        eventTempo.Add(new MapEvent(offset, EventType.SPIN, 0, 1));
                        if (count == 0) //Only add zoom every 2 spin.
                        {
                            eventTempo.Add(new MapEvent(offset, EventType.ZOOM, 0, 1));
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
                    eventTempo.Add(new MapEvent(now, EventType.SPIN, 0, 1));
                }

                // Boost Event
                if (now >= Options.ColorOffset + Options.ColorBoostSwap + boostIncrement && now > firstNote && Options.AllowBoostColor)
                {
                    int calc = (int)((int)(now - boostIncrement) / Options.ColorBoostSwap);

                    for (int i = 0; i < calc; i++)
                    {
                        boostIncrement += Options.ColorBoostSwap;

                        if (boost)
                        {
                            eventTempo.Add(new MapEvent(boostIncrement, EventType.BOOST, 0));
                            boost = false;
                        }
                        else
                        {
                            eventTempo.Add(new MapEvent(boostIncrement, EventType.BOOST, 1));
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
                if (Notes[i].Time - Notes[i - 1].Time <= 0.125 && Notes[i].Time - Notes[i - 1].Time > 0 && (Notes[i].CutDirection == Notes[i - 1].CutDirection || Notes[i].CutDirection == 8 || Notes[i - 1].CutDirection == 8))
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

            foreach (BeatmapNote note in Selection) //Process specific light using time.
            {
                float now = note.Time;
                time[0] = now;

                if (!Options.NerfStrobes && doubleOn) //Off event
                {
                    if (now - last >= 1)
                    {
                        eventTempo.Add(new MapEvent((now - (now - last) / 2), EventType.BACK, 0, 1));
                        eventTempo.Add(new MapEvent((now - (now - last) / 2), EventType.RING, 0, 1));
                        eventTempo.Add(new MapEvent((now - (now - last) / 2), EventType.SIDE, 0, 1));
                        eventTempo.Add(new MapEvent((now - (now - last) / 2), EventType.LEFT, 0, 1));
                        eventTempo.Add(new MapEvent((now - (now - last) / 2), EventType.RIGHT, 0, 1));
                    }
                    else
                    {
                        // Will be fused with some events, but we will sort that out later on.
                        eventTempo.Add(new MapEvent(now, EventType.BACK, 0, 1));
                        eventTempo.Add(new MapEvent(now, EventType.RING, 0, 1));
                        eventTempo.Add(new MapEvent(now, EventType.SIDE, 0, 1));
                        eventTempo.Add(new MapEvent(now, EventType.LEFT, 0, 1));
                        eventTempo.Add(new MapEvent(now, EventType.RIGHT, 0, 1));
                    }

                    doubleOn = false;
                }

                //If not same note, same beat and not slider, apply once.
                if ((now == time[1] || (now - time[1] <= 0.02 && time[1] != time[2])) && (time[1] != 0.0D && now != last) && !sliderTiming.Exists(e => e.Time == now))
                {
                    eventTempo.Add(new MapEvent(now, EventType.BACK, FindColor(First, time[0]), 1)); //Back Top Laser
                    eventTempo.Add(new MapEvent(now, EventType.RING, FindColor(First, time[0]), 1)); //Track Ring Neons
                    eventTempo.Add(new MapEvent(now, EventType.SIDE, FindColor(First, time[0]), 1)); //Side Light
                    eventTempo.Add(new MapEvent(now, EventType.LEFT, FindColor(First, time[0]), 1)); //Left Laser
                    eventTempo.Add(new MapEvent(now, EventType.RIGHT, FindColor(First, time[0]), 1)); //Right Laser

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

                    eventTempo.Add(new MapEvent(now, EventType.LEFT_ROT, currentSpeed, 1)); //Left Rotation
                    eventTempo.Add(new MapEvent(now, EventType.RIGHT_ROT, currentSpeed, 1)); //Right Rotation

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
                        if (x.Time - lastTimeTop <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeTop = x.Time;
                    }
                    else if (x.Type == EventType.RING)
                    {
                        if (x.Time - lastTimeNeon <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeNeon = x.Time;
                    }
                    else if (x.Type == EventType.SIDE)
                    {
                        if (x.Time - lastTimeSide <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeSide = x.Time;
                    }
                    else if (x.Type == EventType.LEFT)
                    {
                        if (x.Time - lastTimeLeft <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeLeft = x.Time;
                    }
                    else if (x.Type == EventType.RIGHT)
                    {
                        if (x.Time - lastTimeRight <= 0.5)
                        {
                            x.Value = Utils.Swap(x.Value);
                        }
                        lastTimeRight = x.Time;
                    }
                }
            }

            ResetTimer();

            foreach (BeatmapNote note in Selection) //Process all note using time.
            {
                time[0] = note.Time;

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
                            if (Selection[i].Time == Selection[i - 1].Time)
                            {
                                nextDouble = Selection[i].Time;
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
                            if (Selection[i].Time - Selection[i - 1].Time <= 0.125 && Selection[i].Time - Selection[i - 1].Time > 0 && (Selection[i].CutDirection == Selection[i - 1].CutDirection || Selection[i].CutDirection == 8))
                            {
                                // Search for the last note of the slider
                                if (sliderNoteCount == 0)
                                {
                                    // This is the first note of the slider
                                    nextSlider = Selection[i - 1].Time;
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
                if (nextSlider == note.Time)
                {
                    // Take a light between neon, side or backlight and strobes it via On/Flash
                    if (sliderIndex == -1)
                    {
                        sliderIndex = 2;
                    }

                    // Place light
                    eventTempo.Add(new MapEvent(time[0], sliderLight[sliderIndex], FindColor(First, time[0]) - 2, 1));
                    eventTempo.Add(new MapEvent(time[0] + 0.125f, sliderLight[sliderIndex], FindColor(First, time[0]) - 1, 1));
                    eventTempo.Add(new MapEvent(time[0] + 0.25f, sliderLight[sliderIndex], FindColor(First, time[0]) - 2, 1));
                    eventTempo.Add(new MapEvent(time[0] + 0.375f, sliderLight[sliderIndex], FindColor(First, time[0]) - 1, 1));
                    eventTempo.Add(new MapEvent(time[0] + 0.5f, sliderLight[sliderIndex], 0, 1));

                    sliderIndex--;

                    // Spin goes brrr
                    eventTempo.Add(new MapEvent(time[0], EventType.SPIN, 0, 1));
                    for (int i = 0; i < 8; i++)
                    {
                        eventTempo.Add(new MapEvent(time[0] + 0.5f - (0.5f / 8f * i), EventType.SPIN, 0, 1));
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
                    eventTempo.Add(new MapEvent(time[0], pattern[patternIndex], FindColor(First, time[0]), 1));

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
                        eventTempo.Add(new MapEvent(time[0], EventType.LEFT_ROT, currentSpeed, 1));
                    }
                    else if (pattern[patternIndex] == 3)
                    {
                        eventTempo.Add(new MapEvent(time[0], EventType.RIGHT_ROT, currentSpeed, 1));
                    }

                    // Place off event
                    if (Selection[Selection.Count - 1].Time != note.Time)
                    {
                        if (Selection[Selection.FindIndex(n => n == note) + 1].Time == nextDouble)
                        {
                            if (Selection[Selection.FindIndex(n => n == note) + 1].Time - time[0] <= 2)
                            {
                                float value = (Selection[Selection.FindIndex(n => n == note) + 1].Time - Selection[Selection.FindIndex(n => n == note)].Time) / 2;
                                eventTempo.Add(new MapEvent(Selection[Selection.FindIndex(n => n == note)].Time + value, pattern[patternIndex], 0, 1));
                            }
                        }
                        else
                        {
                            eventTempo.Add(new MapEvent(Selection[Selection.FindIndex(n => n == note) + 1].Time, pattern[patternIndex], 0, 1));
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

            eventTempo = eventTempo.OrderBy(o => o.Time).ToList();

            // Remove fused or move off event between
            eventTempo = RemoveFused(eventTempo);

            // Sort lights
            eventTempo = eventTempo.OrderBy(o => o.Time).ToList();

            return eventTempo;
        }

        static public List<MapEvent> RemoveFused(List<MapEvent> events)
        {
            float? closest = 0f;

            // Get all fused events of a specific type
            for (int i = 0; i < events.Count; i++)
            {
                MapEvent e = events[i];

                MapEvent MapEvent = events.Find(o => o.Type == e.Type && (o.Time - e.Time >= -0.02 && o.Time - e.Time <= 0.02) && o != e);
                if (MapEvent != null)
                {
                    MapEvent MapEvent2 = events.Find(o => o.Type == MapEvent.Type && (o.Time - MapEvent.Time >= -0.02 && o.Time - MapEvent.Time <= 0.02) && o != MapEvent);

                    if (MapEvent2 != null)
                    {
                        MapEvent temp = events.FindLast(o => o.Time < e.Time && e.Time > closest && o.Value != 0);

                        if (temp != null)
                        {
                            closest = temp.Time;

                            if (MapEvent2.Value == EventLightValue.OFF)
                            {
                                // Move off event between fused note and last note
                                events[(events.FindIndex(o => o.Time == MapEvent2.Time && o.Value == MapEvent2.Value && o.Type == MapEvent2.Type))].Time = (float)(MapEvent2.Time - ((MapEvent2.Time - closest) / 2));
                            }
                            else
                            {
                                // Move off event between fused note and last note
                                if (MapEvent.Value == EventLightValue.OFF || MapEvent.Value == EventLightValue.BLUE_TRANSITION || MapEvent.Value == EventLightValue.RED_TRANSITION)
                                {
                                    events[(events.FindIndex(o => o.Time == MapEvent.Time && o.Value == MapEvent.Value && o.Type == MapEvent.Type))].Time = (float)(MapEvent.Time - ((MapEvent.Time - closest) / 2));
                                }
                                else // Delete event
                                {
                                    events.RemoveAt(events.FindIndex(o => o.Time == MapEvent.Time && o.Value == MapEvent.Value && o.Type == MapEvent.Type));
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
    }
}