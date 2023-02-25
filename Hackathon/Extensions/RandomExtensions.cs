using System;

namespace Extensions {
    /// <summary>
    /// Contains extension methods for various classes.
    /// </summary>
    static class RandomExtensions {
        /// <summary>
        /// Returns a double in the range [min, max)
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double NextDouble(this Random random, double min, double max) {
            return min + random.NextDouble() * (max - min);
        }

        /// <summary>
        /// Returns a float in the range [min, max)
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float NextFloat(this Random random, double min, double max) {
            return (float)random.NextDouble(min, max);
        }

        /// <summary>
        /// Returns a float in the range [0, max)
        /// </summary>
        /// <param name="random"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float NextFloat(this Random random, double max) {
            return (float)(random.NextDouble() * max);
        }

        /// <summary>
        /// Returns a random true or false value.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static bool NextBoolean(this Random random) {
            //Next() returns an int in the range [0..Int32.MaxValue]
            return random.Next() > (Int32.MaxValue / 2);
        }
    }
}