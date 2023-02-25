using Microsoft.Xna.Framework;

namespace Extensions {
    /// <summary>
    /// Contains extension methods for various classes.
    /// </summary>
    static class Conversions {
        /// <summary>
        /// Use to prevent blurry images.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Truncated(this Vector2 v) {
            return new Vector2((int)v.X, (int)v.Y);
        }

        /// <summary>
        /// Convert a point to a Vector2.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Point p) {
            return new Vector2(p.X, p.Y);
        }
    }
}