namespace Extensions {
    /// <summary>
    /// Contains extension methods for various classes.
    /// </summary>
    static class Intersections {
        /// <summary>
        /// Return true iff the distance is less than the sum of r1 and r2.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static bool CircleIntersection(float r1, float r2, float distance) {
            float sum = r1 + r2;

            return distance <= sum;
        }
    }
}