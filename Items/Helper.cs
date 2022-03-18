using System.Collections.Generic;
using System.Linq;
using static Automapper.Items.Enumerator;
using static Automapper.Items.Utils;

namespace Automapper.Items
{
    internal class Helper
    {
        static public (BeatmapNote, BeatmapNote) FixDoublePlacement(BeatmapNote red, BeatmapNote blue)
        {
            // Both on same layer
            if (red.LineLayer == blue.LineLayer && red.LineIndex != blue.LineIndex)
            {
                // Change the layer of one of the note
                if (SwingType.Horizontal.Contains(red.CutDirection))
                {
                    if (red.LineLayer == Layer.BOTTOM)
                    {
                        if (red.LineIndex == Line.LEFT)
                        {
                            red.LineLayer++;
                        }
                        else if (red.LineIndex == Line.MIDDLE_LEFT || red.LineIndex == Line.MIDDLE_RIGHT)
                        {
                            red.LineLayer = Layer.TOP;
                        }
                        else
                        {
                            if (SwingType.Up.Contains(blue.CutDirection))
                            {
                                blue.LineLayer = Layer.TOP;
                            }
                            else
                            {
                                red.LineLayer++;
                            }
                        }
                    }
                    else if (red.LineLayer == Layer.TOP)
                    {
                        if (red.LineIndex == Line.LEFT)
                        {
                            red.LineLayer--;
                        }
                        else if (red.LineIndex == Line.MIDDLE_LEFT || red.LineIndex == Line.MIDDLE_RIGHT)
                        {
                            red.LineLayer = Layer.BOTTOM;
                        }
                        else
                        {
                            red.LineLayer--;
                        }
                    }
                }
                else if (SwingType.Horizontal.Contains(blue.CutDirection))
                {
                    if (blue.LineLayer == Layer.BOTTOM)
                    {
                        if (blue.LineIndex == Line.RIGHT)
                        {
                            blue.LineLayer++;
                        }
                        else if (blue.LineIndex == Line.MIDDLE_LEFT || blue.LineIndex == Line.MIDDLE_RIGHT)
                        {
                            blue.LineLayer = Layer.TOP;
                        }
                        else
                        {
                            if (SwingType.Up.Contains(red.CutDirection))
                            {
                                red.LineLayer = Layer.TOP;
                            }
                            else
                            {
                                blue.LineLayer++;
                            }
                        }
                    }
                    else if (blue.LineLayer == Layer.TOP)
                    {
                        if (blue.LineIndex == Line.RIGHT)
                        {
                            blue.LineLayer--;
                        }
                        else if (blue.LineIndex == Line.MIDDLE_LEFT || blue.LineIndex == Line.MIDDLE_RIGHT)
                        {
                            blue.LineLayer = Layer.BOTTOM;
                        }
                        else if (blue.LineLayer == Layer.TOP)
                        {
                            if (blue.LineIndex == Line.LEFT)
                            {
                                blue.LineLayer--;
                            }
                            else if (blue.LineIndex == Line.MIDDLE_LEFT || blue.LineIndex == Line.MIDDLE_RIGHT)
                            {
                                blue.LineLayer = Layer.BOTTOM;
                            }
                            else
                            {
                                blue.LineLayer--;
                            }
                        }
                    }
                }
            }
            // Both on the same line
            else if (red.LineIndex == blue.LineIndex && red.LineLayer != blue.LineLayer)
            {
                if (SwingType.Vertical.Contains(red.CutDirection))
                {
                    // Change the line of one of the notes
                    if (red.LineIndex > 1)
                    {
                        if (red.LineLayer != Layer.MIDDLE)
                        {
                            if (red.LineIndex != Line.LEFT)
                            {
                                red.LineIndex--;
                            }
                            else
                            {
                                red.LineIndex++;
                            }
                        }
                        else if (blue.LineLayer == 0)
                        {
                            red.LineLayer = 2;
                            if (red.LineIndex != Line.LEFT)
                            {
                                red.LineIndex--;
                            }
                            else
                            {
                                red.LineIndex++;
                            }
                        }
                        else if (blue.LineLayer == 2)
                        {
                            red.LineLayer = 0;
                            if (red.LineIndex != Line.LEFT)
                            {
                                red.LineIndex--;
                            }
                            else
                            {
                                red.LineIndex++;
                            }
                        }
                    }
                    else
                    {
                        if (blue.LineIndex != Line.RIGHT)
                        {
                            blue.LineIndex++;
                        }
                        else if (red.LineIndex != Line.LEFT)
                        {
                            red.LineIndex--;
                        }
                    }
                }
                else if (SwingType.Vertical.Contains(blue.CutDirection))
                {
                    if (blue.LineIndex < 2)
                    {
                        if (blue.LineLayer != Layer.MIDDLE)
                        {
                            if (blue.LineIndex != Line.RIGHT)
                            {
                                blue.LineIndex++;
                            }
                            else
                            {
                                blue.LineIndex--;
                            }
                        }
                        else if (red.LineLayer == 0)
                        {
                            blue.LineLayer = 2;
                            if (blue.LineIndex != Line.RIGHT)
                            {
                                blue.LineIndex++;
                            }
                            else
                            {
                                blue.LineIndex--;
                            }
                        }
                        else if (red.LineLayer == 2)
                        {
                            blue.LineLayer = 0;
                            if (blue.LineIndex != Line.RIGHT)
                            {
                                blue.LineIndex++;
                            }
                            else
                            {
                                blue.LineIndex--;
                            }
                        }
                    }
                    else
                    {
                        if (red.LineIndex != Line.LEFT)
                        {
                            red.LineIndex--;
                        }
                        else if (blue.LineIndex != Line.RIGHT)
                        {
                            blue.LineIndex++;
                        }
                    }
                }
            }

            // Diagonal
            if (SwingType.Diagonal.Contains(red.CutDirection) || SwingType.Diagonal.Contains(blue.CutDirection))
            {
                if (red.LineIndex == blue.LineIndex - 1 && red.LineLayer == blue.LineLayer - 1)
                {
                    if (blue.LineLayer != 2)
                    {
                        blue.LineLayer++;
                    }
                    else
                    {
                        red.LineLayer--;
                    }
                }
                else if (red.LineIndex == blue.LineIndex - 1 && red.LineLayer == blue.LineLayer + 1)
                {
                    if (blue.LineLayer != 0)
                    {
                        blue.LineLayer--;
                    }
                    else
                    {
                        red.LineLayer++;
                    }
                }
                else if (red.LineIndex == blue.LineIndex + 1 && red.LineLayer == blue.LineLayer + 1)
                {
                    if (blue.LineLayer != 2)
                    {
                        (red.LineIndex, blue.LineIndex) = (blue.LineIndex, red.LineIndex);
                        (red.LineLayer, blue.LineLayer) = (blue.LineLayer, red.LineLayer);
                        blue.LineLayer++;
                    }
                    else
                    {
                        (red.LineIndex, blue.LineIndex) = (blue.LineIndex, red.LineIndex);
                        (red.LineLayer, blue.LineLayer) = (blue.LineLayer, red.LineLayer);
                        red.LineLayer--;
                    }
                }
                else if (red.LineIndex == blue.LineIndex + 1 && red.LineLayer == blue.LineLayer - 1)
                {
                    if (blue.LineLayer != 0)
                    {
                        (red.LineIndex, blue.LineIndex) = (blue.LineIndex, red.LineIndex);
                        (red.LineLayer, blue.LineLayer) = (blue.LineLayer, red.LineLayer);
                        blue.LineLayer--;
                    }
                    else
                    {
                        (red.LineIndex, blue.LineIndex) = (blue.LineIndex, red.LineIndex);
                        (red.LineLayer, blue.LineLayer) = (blue.LineLayer, red.LineLayer);
                        red.LineLayer++;
                    }
                }
            }

            return (red, blue);
        }

        /// <summary>
        /// Verify if the note placement match the situation and return the value
        /// </summary>
        /// <param name="lastLine">Last note line</param>
        /// <param name="lastLayer">Last note layer</param>
        /// <param name="type">Note color</param>
        /// <returns>Line, Layer</returns>
        static public (int, int) PlacementCheck(int direction, int type, BeatmapNote lastNote)
        {
            // Next possible line and layer
            int line = -1;
            int layer = -1;
            int rand;

            do
            {
                if (type == ColorType.RED)
                {
                    rand = RandNumber(0, PossibleRedPlacement.placement[direction].Count());
                    line = PossibleRedPlacement.placement[direction][rand][0];
                    layer = PossibleRedPlacement.placement[direction][rand][1];
                }
                else if (type == ColorType.BLUE)
                {
                    rand = RandNumber(0, PossibleBluePlacement.placement[direction].Count());
                    line = PossibleBluePlacement.placement[direction][rand][0];
                    layer = PossibleBluePlacement.placement[direction][rand][1];
                }

                // Fix possible fused notes
                if (lastNote.LineIndex == line && lastNote.LineLayer == layer)
                {
                    continue;
                }

                return (line, layer);
            } while (true);
        }

        /// <summary>
        /// Verify if the next direction selected match the situation (flow free) and return the value
        /// </summary>
        /// <param name="last">Last direction</param>
        /// <param name="swing">Up or down swing (wrist)</param>
        /// <param name="hand">Current hand</param>
        /// <param name="speed"></param>
        /// <returns>Direction</returns>
        static public int NextDirection(int last, int swing, int hand, float speed, bool limiter)
        {
            // Store all the possible next cut direction, we will use some logic to find if the next direction match
            int[] possibleNext = { 0, 0 };
            // Type of flow
            int flow = 0;
            // Next direction
            int next;

            // Get the direction based on speed
            if (hand == 0)
            {
                if (speed < 0.5) // Under half a beat
                {
                    possibleNext = PossibleFlow.normalRed[last];
                    flow = 2;
                }
                else if (speed >= 0.5 && speed < 1) // Half to under a beat
                {
                    possibleNext = PossibleFlow.techRed[last];
                    flow = 1;
                }
                else // Anything above a beat is pretty slow usually
                {
                    possibleNext = PossibleFlow.extremeRed[last];
                    flow = 0;
                }
            }
            else if (hand == 1)
            {
                if (speed < 0.5) // Under half a beat
                {
                    possibleNext = PossibleFlow.normalBlue[last];
                    flow = 2;
                }
                else if (speed >= 0.5 && speed < 1) // Half to under a beat
                {
                    possibleNext = PossibleFlow.techBlue[last];
                    flow = 1;
                }
                else // Anything above a beat is pretty slow usually
                {
                    possibleNext = PossibleFlow.extremeBlue[last];
                    flow = 0;
                }
            }


            do
            {
                next = Utils.RandNumber(0, 8);

                if (hand == 0)
                {
                    if (PossibleFlow.extremeRed[last].Contains(next) && !PossibleFlow.techRed[last].Contains(next)) // Extreme roll again
                    {
                        next = Utils.RandNumber(0, 8);
                    }
                    if ((PossibleFlow.extremeRed[last].Contains(next) || PossibleFlow.techRed[last].Contains(next)) && !PossibleFlow.normalRed[last].Contains(next)) // Extreme and tech roll again
                    {
                        next = Utils.RandNumber(0, 8);
                    }
                }
                else if (hand == 1)
                {
                    if (PossibleFlow.extremeBlue[last].Contains(next) && !PossibleFlow.techBlue[last].Contains(next)) // Extreme roll again
                    {
                        next = Utils.RandNumber(0, 8);
                    }
                    if ((PossibleFlow.extremeBlue[last].Contains(next) || PossibleFlow.techBlue[last].Contains(next)) && !PossibleFlow.normalBlue[last].Contains(next)) // Extreme and tech roll again
                    {
                        next = Utils.RandNumber(0, 8);
                    }
                }




                // We check if the possible next direction match with the last one before any logic.
                if (possibleNext.Contains(next))
                {
                    // Each hand and type of swing have to be treated differently
                    if (hand == 0) // Red
                    {
                        if (swing == 0) // Up Swing
                        {
                            if (limiter && (next == CutDirection.LEFT || next == CutDirection.UP_RIGHT || next == CutDirection.UP)) // Too far
                            {
                                continue;
                            }

                            if (last == CutDirection.LEFT)
                            {
                                if (next == CutDirection.DOWN && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }

                            if (last == CutDirection.DOWN || last == CutDirection.DOWN_LEFT) // Down, maximum range of the wrist from the left (extreme)
                            {
                                if (next == CutDirection.LEFT || next == CutDirection.UP_LEFT) // Impossible
                                {
                                    continue;
                                }
                            }
                            else if (last == CutDirection.RIGHT || last == CutDirection.UP_RIGHT) // Right, maximum range of the wrist from the right (extreme)
                            {
                                if (next == CutDirection.UP || next == CutDirection.UP_LEFT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.UP)
                            {
                                if ((next == CutDirection.RIGHT) && flow != 0) // Not extreme
                                {
                                    continue;
                                }
                                if (next == CutDirection.DOWN_LEFT && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }
                        }
                        else if (swing == 1) // Down Swing
                        {
                            if (limiter && (next == CutDirection.RIGHT || next == CutDirection.UP_RIGHT || next == CutDirection.DOWN || next == CutDirection.DOWN_LEFT)) // Too far
                            {
                                continue;
                            }

                            if (last == CutDirection.RIGHT) //Meh
                            {
                                if (next == CutDirection.UP && flow != 0)
                                {
                                    continue;
                                }
                            }

                            if (last == CutDirection.UP || last == CutDirection.UP_RIGHT) // Up, maximum range of the wrist from the left (extreme)
                            {
                                if (next == CutDirection.RIGHT || next == CutDirection.DOWN_RIGHT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.LEFT || last == CutDirection.DOWN_LEFT) // Left, maximum range of the wrist from the right (extreme)
                            {
                                if (next == CutDirection.DOWN || next == CutDirection.DOWN_RIGHT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.DOWN)
                            {
                                if ((next == CutDirection.RIGHT || next == CutDirection.UP_RIGHT) && flow != 0) // Not extreme
                                {
                                    continue;
                                }
                                if (next == CutDirection.LEFT && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    else if (hand == 1) // Blue
                    {
                        if (swing == 0) // Up Swing
                        {
                            if (limiter && (next == CutDirection.RIGHT || next == CutDirection.UP || next == CutDirection.UP_LEFT)) // Too far
                            {
                                continue;
                            }

                            if (last == CutDirection.RIGHT)
                            {
                                if (next == CutDirection.DOWN && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }

                            if (last == CutDirection.DOWN || last == CutDirection.DOWN_RIGHT) // Down, maximum range of the wrist from the right (extreme)
                            {
                                if (next == CutDirection.RIGHT || next == CutDirection.UP_RIGHT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.LEFT || last == CutDirection.UP_LEFT) // Left, maximum range of the wrist from the left (extreme)
                            {
                                if (next == CutDirection.UP || next == CutDirection.UP_RIGHT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.UP)
                            {
                                if (next == CutDirection.LEFT && flow != 0) // Not extreme
                                {
                                    continue;
                                }
                                if (next == CutDirection.DOWN_RIGHT && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }
                        }
                        else if (swing == 1) // Down Swing
                        {
                            if (limiter && (next == CutDirection.LEFT || next == CutDirection.UP_LEFT || next == CutDirection.DOWN || next == CutDirection.DOWN_RIGHT)) // Too far
                            {
                                continue;
                            }

                            if (last == CutDirection.LEFT) //Meh
                            {
                                if (next == CutDirection.UP && flow != 0)
                                {
                                    continue;
                                }
                            }

                            if (last == CutDirection.UP || last == CutDirection.UP_LEFT) // Up, maximum range of the wrist from the right (extreme)
                            {
                                if (next == CutDirection.LEFT || next == CutDirection.DOWN_LEFT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.RIGHT || last == CutDirection.DOWN_RIGHT) // Right, maximum range of the wrist from the left (extreme)
                            {
                                if (next == CutDirection.DOWN || next == CutDirection.DOWN_LEFT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.DOWN)
                            {
                                if ((next == CutDirection.LEFT || next == CutDirection.UP_LEFT) && flow != 0) // Not extreme
                                {
                                    continue;
                                }
                                if (next == CutDirection.RIGHT && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    return next;
                }
            } while (true);
        }

        /// <summary>
        /// Method to find specific pattern and remove the notes of the pattern from the main list of note
        /// </summary>
        /// <param name="notes">List of ColorNote</param>
        /// <returns>List of list of ColorNote (Pattern) and modified List of ColorNote</returns>
        static public (List<List<BeatmapNote>>, List<BeatmapNote>) FindPattern(List<BeatmapNote> notes)
        {
            // List of list to keep thing like sliders/stack/window/tower etc
            List<List<BeatmapNote>> patterns = new List<List<BeatmapNote>>();

            // Stock pattern notes
            List<BeatmapNote> pattern = new List<BeatmapNote>();

            // To know if a pattern was found
            bool found = false;

            // Find all notes sliders/stack/window/tower
            for (int i = 0; i < notes.Count; i++)
            {
                if (i == notes.Count - 1)
                {
                    if (found)
                    {
                        BeatmapNote n = new BeatmapNote();
                        n.Time = notes[i].Time;
                        n.LineIndex = notes[i].LineIndex;
                        n.LineLayer = notes[i].LineLayer;
                        n.Type = notes[i].Type;
                        n.CutDirection = notes[i].CutDirection;
                        pattern.Add(n);
                        notes.RemoveAt(i);
                        patterns.Add(new List<BeatmapNote>(pattern));
                        found = false;
                    }
                    break;
                }

                BeatmapNote now = notes[i];
                BeatmapNote next = notes[i + 1];

                if (next.Time - now.Time >= 0 && next.Time - now.Time < 0.1)
                {
                    if (!found)
                    {
                        pattern = new List<BeatmapNote>();
                        found = true;
                    }
                    BeatmapNote n = new BeatmapNote();
                    n.Time = notes[i].Time;
                    n.LineIndex = notes[i].LineIndex;
                    n.LineLayer = notes[i].LineLayer;
                    n.Type = notes[i].Type;
                    n.CutDirection = notes[i].CutDirection;
                    pattern.Add(n);
                    notes.RemoveAt(i);
                    i--;
                }
                else
                {
                    if (found)
                    {
                        BeatmapNote n = new BeatmapNote();
                        n.Time = notes[i].Time;
                        n.LineIndex = notes[i].LineIndex;
                        n.LineLayer = notes[i].LineLayer;
                        n.Type = notes[i].Type;
                        n.CutDirection = notes[i].CutDirection;
                        pattern.Add(n);
                        notes.RemoveAt(i);
                        i--;
                        patterns.Add(new List<BeatmapNote>(pattern));
                    }

                    found = false;
                }
            }

            return (patterns, notes);
        }
    }
}
