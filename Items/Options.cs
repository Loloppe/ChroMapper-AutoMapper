namespace Automapper.Items
{
    static class Options
    {
        public static class Light
        {
            private static float colorOffset = 0.0f;
            private static float colorSwap = 4.0f;
            private static float colorBoostSwap = 8.0f;

            public static float ColorOffset { set => colorOffset = value > -100.0f ? value : 0.0f; get => colorOffset; }
            public static float ColorBoostSwap { set => colorBoostSwap = value > 0.0f ? value : 8.0f; get => colorBoostSwap; }
            public static float ColorSwap { set => colorSwap = value > 0.0f ? value : 4.0f; get => colorSwap; }
            public static bool AllowBoostColor { set; get; } = true;
            public static bool NerfStrobes { set; get; } = false;
            public static bool IgnoreBomb { set; get; } = true;
        }

        public static class Mapper
        {
            private static float indistinguishableRange = 0.003f;
            private static float onsetSensitivity = 1.3f;
            private static float doubleThreshold = 0.2f;
            private static float minRange = 0f;
            private static float maxRange = 100000f;
            private static double maxSpeed = (1d / 8d);
            private static double maxDoubleSpeed = (1d / 3d);

            public static bool UpDownOnly { set; get; } = false;
            public static bool BottomRowOnly { set; get; } = false;
            public static bool GenerateAsTiming { set; get; } = true;
            public static bool Limiter { set; get; } = true;
            public static float IndistinguishableRange { set => indistinguishableRange = value > 0.0f ? value : 0.003f; get => indistinguishableRange; }
            public static float OnsetSensitivity { set => onsetSensitivity = value > 0.0f ? value : 1.3f; get => onsetSensitivity; }
            public static float DoubleThreshold { set => doubleThreshold = value >= 0.0f ? value : 0.2f; get => doubleThreshold; }
            public static float MinRange { set => minRange = value >= 0.0f ? value : 0f; get => minRange; }
            public static float MaxRange { set => maxRange = value >= 0.0f ? value : 100000f; get => maxRange; }
            public static double MaxSpeed { set => maxSpeed = value > 0.0f ? value : (1d / 8d); get => maxSpeed; }
            public static double MaxDoubleSpeed { set => maxDoubleSpeed = value > 0.0f ? value : (1d / 3d); get => maxDoubleSpeed; }
        }
    }
}
