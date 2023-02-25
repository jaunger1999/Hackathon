using Microsoft.Xna.Framework;

namespace Extensions {
    static class ToStrings {
        public static string ToString(this Point p) {
            return p.X + ", " + p.Y;
        }
    }
}