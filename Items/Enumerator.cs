using System.Collections.Generic;

namespace Automapper.Items
{
    internal class Enumerator
    {
        internal static class PossibleFlow
        {
            // List of angle and their possible next direction
            // First array is direction, second array is possible flow
            /*static public readonly int[][] extremeRed = new int[][] { new int[] { 1, 2, 3, 6, 7, 8 }, new int[] { 0, 2, 3, 4, 5, 8 }, new int[] { 0, 1, 3, 5, 7, 8 }, new int[] { 0, 1, 2, 4, 6, 8 },
               new int[] { 1, 3, 5, 7, 8 }, new int[] { 1, 2, 6, 7, 8 }, new int[] { 0, 3, 4, 5, 8 }, new int[] { 0, 2, 4, 5, 8 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }};

            static public readonly int[][] techRed = new int[][] { new int[] { 1, 2, 3, 7, 8 }, new int[] { 0, 2, 3, 4, 8 }, new int[] { 0, 1, 3, 5, 7, 8 }, new int[] { 0, 1, 2, 4, 6, 8 },
               new int[] { 1, 3, 7, 8 }, new int[] { 1, 2, 6, 8 }, new int[] { 0, 3, 5, 8 }, new int[] { 0, 2, 4, 8 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }};
            */
            static public readonly int[][] normalRed = new int[][] { new int[] { 1, 7, 8 }, new int[] { 0, 4, 8 }, new int[] { 3, 5, 7, 8 }, new int[] { 2, 4, 6, 8 },
               new int[] { 1, 3, 7, 8 }, new int[] { 1, 2, 6, 8 }, new int[] { 0, 3, 5, 8 }, new int[] { 0, 2, 4, 8 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }};
            /*
            static public readonly int[][] extremeBlue = new int[][] { new int[] { 1, 2, 3, 6, 7, 8 }, new int[] { 0, 2, 3, 4, 5, 8 }, new int[] { 0, 1, 3, 5, 7, 8 }, new int[] { 0, 1, 2, 4, 6, 8 },
               new int[] { 1, 3, 5, 7, 8 }, new int[] { 1, 2, 6, 7, 8 }, new int[] { 0, 3, 4, 5, 8 }, new int[] { 0, 2, 4, 5, 8 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }};

            static public readonly int[][] techBlue = new int[][] { new int[] { 1, 2, 3, 6, 8 }, new int[] { 0, 2, 3, 5, 8 }, new int[] { 0, 1, 3, 5, 7, 8 }, new int[] { 0, 1, 2, 4, 6, 8 },
               new int[] { 1, 3, 7, 8 }, new int[] { 1, 2, 6, 8 }, new int[] { 0, 3, 5, 8 }, new int[] { 0, 2, 4, 8 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }};
            */
            static public readonly int[][] normalBlue = new int[][] { new int[] { 1, 6, 8 }, new int[] { 0, 5, 8 }, new int[] { 3, 5, 7, 8 }, new int[] { 2, 4, 6, 8 },
               new int[] { 1, 3, 7, 8 }, new int[] { 1, 2, 6, 8 }, new int[] { 0, 3, 5, 8 }, new int[] { 0, 2, 4, 8 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }};
        }

        internal static class PossibleRedPlacement
        {
            // List of angle and their possible next placement
            // First array is direction, second array is possible placement, line and layer
            static public readonly int[][][] placement = new int[][][] { new int[][]
            { new int[] { 0, 1 }, new int[] { 0, 2 }, new int[] { 1, 2 } }, // Up
            new int[][] { new int[] { 0, 0 }, new int[] { 1, 0 } },  // Down
            new int[][] { new int[] { 0, 0 }, new int[] { 0, 1 } }, // Left
            new int[][] { new int[] { 2, 0 }}, // Right
            new int[][] { new int[] { 0, 1 }, new int[] { 0, 2 } }, // Up-Left
            new int[][] { new int[] { 2, 2 }}, // Up-Right
            new int[][] { new int[] { 0, 0 }, new int[] { 0, 1 } }, // Down-Left
            new int[][] { new int[] { 2, 0 }} // Down-Right
            };
        }

        internal static class PossibleBluePlacement
        {
            // List of angle and their possible next placement
            // First array is direction, second array is possible placement, line and layer
            static public readonly int[][][] placement = new int[][][] { new int[][]
            { new int[] { 3, 1 }, new int[] { 3, 2 }, new int[] { 2, 2 } }, // Up
            new int[][] { new int[] { 3, 0 }, new int[] { 2, 0 } },  // Down
            new int[][] { new int[] { 1, 0 } }, // Left
            new int[][] { new int[] { 3, 0 }, new int[] { 3, 1 } }, // Right
            new int[][] { new int[] { 1, 2 } }, // Up-Left
            new int[][] { new int[] { 3, 1 }, new int[] { 3, 2 } }, // Up-Right
            new int[][] { new int[] { 1, 0 } }, // Down-Left
            new int[][] { new int[] { 3, 0 }, new int[] { 3, 1 } } // Down-Right
            };
        }

        internal static class DistributionParamType
        {
            public const int WAVE = 1;
            public const int STEP = 2;
        }

        internal static class RotationDirection
        {
            public const int AUTOMATIC = 0;
            public const int CLOCKWISE = 1;
            public const int COUNTER_CLOCKWISE = 2;
        }

        internal static class Axis
        {
            public const int X = 0;
            public const int Y = 1;
        }

        internal static class EaseType
        {
            public const int NONE = -1;
            public const int LINEAR = 0;
            public const int IN_QUAD = 1;
            public const int OUT_QUAD = 2;
            public const int IN_OUT_QUAD = 3;
        }

        internal static class TransitionType
        {
            public const int INSTANT = 0;
            public const int INTERPOLATE = 1;
            public const int EXTEND = 2;
        }

        internal static class IndexFilterType
        {
            public const int DIVISION = 1;
            public const int STEP_AND_OFFSET = 2;
        }

        internal static class LaserType
        {
            // Further away from the center
            public const int LEFT_BOTTOM_VERTICAL = 0;
            public const int RIGHT_BOTTOM_VERTICAL = 1;
            public const int LEFT_TOP_VERTICAL = 2;
            public const int RIGHT_TOP_VERTICAL = 3;
            // Same as those above, but close to the center
            public const int LEFT_BOTTOM_CENTER_VERTICAL = 4;
            public const int RIGHT_BOTTOM_CENTER_VERTICAL = 5;
            public const int LEFT_TOP_CENTER_VERTICAL = 6;
            public const int RIGHT_TOP_CENTER_VERTICAL = 7;
            // Two horizontal layer on the left and two on the right
            public const int LEFT_BOTTOM_HORIZONTAL = 8;
            public const int RIGHT_BOTTOM_HORIZONTAL = 9;
            public const int LEFT_TOP_HORIZONTAL = 10;
            public const int RIGHT_TOP_HORIZONTAL = 11;
            // At the very back, point directly toward player
            public const int TOP_CENTER = 12;
            public const int BOTTOM_CENTER = 13;
            public const int LEFT_CENTER = 14;
            public const int RIGHT_CENTER = 15;
        }

        internal static class Line
        {
            public const int LEFT = 0;
            public const int MIDDLE_LEFT = 1;
            public const int MIDDLE_RIGHT = 2;
            public const int RIGHT = 3;
        }

        internal static class Layer
        {
            public const int BOTTOM = 0;
            public const int MIDDLE = 1;
            public const int TOP = 2;
        }

        internal static class CutDirection
        {
            public const int UP = 0;
            public const int DOWN = 1;
            public const int LEFT = 2;
            public const int RIGHT = 3;
            public const int UP_LEFT = 4;
            public const int UP_RIGHT = 5;
            public const int DOWN_LEFT = 6;
            public const int DOWN_RIGHT = 7;
            public const int ANY = 8;
        }

        internal static class ColorType
        {
            public const int RED = 0;
            public const int BLUE = 1;
        }

        internal static class NoteType
        {
            public const int RED = 0;
            public const int BLUE = 1;
            public const int BOMB = 3;
        }

        internal static class ObstacleType
        {
            public const int WALL = 0;
            public const int CEILING = 1;
        }

        internal static class EventType
        {
            public const int BACK = 0;
            public const int RING = 1;
            public const int LEFT = 2;
            public const int RIGHT = 3;
            public const int SIDE = 4;
            public const int BOOST = 5;
            public const int LIGHT_LEFT_EXTRA_LIGHT = 6;
            public const int LIGHT_RIGHT_EXTRA_LIGHT = 7;
            public const int SPIN = 8;
            public const int ZOOM = 9;
            public const int LIGHT_LEFT_EXTRA2_LIGHT = 10;
            public const int LIGHT_RIGHT_EXTRA2_LIGHT = 11;
            public const int LEFT_ROT = 12;
            public const int RIGHT_ROT = 13;
            public const int ROTATION_EARLY_LANE = 14;
            public const int ROTATION_LATE_LANE = 15;
            public const int EXTRA_EVENT1 = 16;
            public const int EXTRA_EVENT2 = 17;
            public const int BPM = 100;
        }

        internal static class EventLightValue
        {
            public const int OFF = 0;
            public const int BLUE_ON = 1;
            public const int BLUE_FLASH = 2;
            public const int BLUE_FADE = 3;
            public const int BLUE_TRANSITION = 4;
            public const int RED_ON = 5;
            public const int RED_FLASH = 6;
            public const int RED_FADE = 7;
            public const int RED_TRANSITION = 8;
        }

        internal static class SliderMidAnchorMode
        {
            public const int STRAIGHT = 0;
            public const int CLOCKWISE = 1;
            public const int COUNTER_CLOCKWISE = 2;
        }

        internal static class EnvironmentEvent
        {
            public static List<int> LightEventType = new List<int>() { 0, 1, 2, 3, 4 };
            public static List<int> TrackRingEventType = new List<int>() { 8, 9 };
            public static List<int> LaserRotationEventType = new List<int>() { 12, 13 };
            public static List<int> AllEventType = new List<int>() { 0, 1, 2, 3, 4, 8, 9, 12, 13 };
        }

        internal static class SwingType
        {
            public static List<int> Up = new List<int>() { 0, 4, 5 };
            public static List<int> Down = new List<int>() { 1, 6, 7 };
            public static List<int> Left = new List<int>() { 2, 4, 6 };
            public static List<int> Right = new List<int>() { 3, 5, 7 };
            public static List<int> Vertical = new List<int>() { 0, 1, 4, 5, 6, 7 };
            public static List<int> Horizontal = new List<int>() { 2, 3, 4, 5, 6, 7 };
            public static List<int> Diagonal = new List<int>() { 4, 5, 6, 7 };
        }
    }
}
