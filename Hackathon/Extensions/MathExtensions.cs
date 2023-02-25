using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions {
    /// <summary>
    /// A set of math class extensions.
    /// </summary>
    static class MathExtensions {
        /// <summary>
        /// Returns the smallest in an array of values.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Min(params float[] x) {
            float min = float.MaxValue;

            for (int i = 0; i < x.Length; i++) {
                if (min > x[i]) {
                    min = x[i];
                }
            }

            return min;
        }

        /// <summary>
        /// Returns a positive atan 2 result.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double PositiveAtan2(double y, double x) {
            double angle = Math.Atan2(y, x);

            if (angle < 0) {
                angle += 2 * Math.PI;
            }
            else if (angle >= 2 * Math.PI) {
                angle %= 2 * Math.PI;
            }

            return angle;
        }
    }
}