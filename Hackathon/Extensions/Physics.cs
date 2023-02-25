using Microsoft.Xna.Framework;
using System;

namespace Extensions {
    /// <summary>
    /// Contains extension methods for various classes.
    /// </summary>
    static class Physics {
        /// <summary>
        /// Calculates air resistance.
        /// </summary>
        /// <param name="coDrag"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public static Vector2 Drag(float coDrag, Vector2 velocity) {
            Vector2 drag;
            float x, y;

            try {
                x = Math.Sign(-velocity.X) * coDrag * (velocity.X * velocity.X);
                y = Math.Sign(-velocity.Y) * coDrag * (velocity.Y * velocity.Y);

                drag = new Vector2(x, y);
            }
            catch (ArithmeticException e) {
                Console.WriteLine(e.Message);
                drag = Vector2.Zero;
            }

            return drag;
        }
    }
}