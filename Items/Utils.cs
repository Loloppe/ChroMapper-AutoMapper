using Beatmap.Base;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using static Automapper.Items.Enumerator;

namespace Automapper.Items
{
    static class Utils
    {
        internal static class EnvironmentEvent
        {
            public static List<int> LIGHT_EVENT_TYPE = new List<int>() { 0, 1, 2, 3, 4, 6, 7, 10, 11 };
            public static List<int> AUX_EVENT_TYPE = new List<int>() { 5, 8, 9, 12, 13, 16, 17 };
            public static List<int> RING_EVENT_TYPE = new List<int>() { 8, 9 };
            public static List<int> LASER_ROTATION_EVENT_TYPE = new List<int>() { 12, 13 };
            public static List<int> LANE_ROTATION_EVENT_TYPE = new List<int>() { 14, 15 };
            public static List<int> ENVIRONMENT_EVENT_TYPE = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 16, 17 };
            public static List<int> ALL_EVENT_TYPE = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 100 };

            public static bool IsEnvironmentEvent(BaseEvent ev)
            {
                return ENVIRONMENT_EVENT_TYPE.Contains(ev.Type);
            }
        }

        /// <summary>
        /// Method to randomly generate a number between the minimum and maximum (excluded)
        /// If Maximum is Minimum, return Minimum
        /// </summary>
        /// <param name="Low">Minimum</param>
        /// <param name="High">Maximum - 1</param>
        /// <returns></returns>
        internal static int RandNumber(int Low, int High)
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            int rnd = rndNum.Next(Low, High);

            return rnd;
        }

        /// <summary>
        /// Method to swap between Fade and On for EventLightValue
        /// </summary>
        /// <param name="value">Current EventLightValue</param>
        /// <returns>Swapped EventLightValue</returns>
        internal static int Swap(int value)
        {
            switch (value)
            {
                case EventLightValue.BLUE_FADE: return EventLightValue.BLUE_ON;
                case EventLightValue.RED_FADE: return EventLightValue.RED_ON;
                case EventLightValue.BLUE_ON: return EventLightValue.BLUE_FADE;
                case EventLightValue.RED_ON: return EventLightValue.RED_FADE;
                default: return 0;
            }
        }

        /// <summary>
        /// Method to inverse the current EventLightValue between Red and Blue
        /// </summary>
        /// <param name="value">Current EventLightValue</param>
        /// <returns>Inversed EventLightValue</returns>
        internal static int Inverse(int value)
        {
            if (value > EventLightValue.BLUE_FADE)
                return value - 4; //Turn to blue
            else
                return value + 4; //Turn to red
        }

        /// <summary>
        /// Method to randomise the element of a List
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="list">List</param>
        internal static void Shuffle<T>(this IList<T> list)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do rng.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
