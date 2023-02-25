namespace Directions {
    public enum Dir { right, downright, down, downleft, left, upleft, up, upright } //all possible directions

    /// <summary>
    /// Contains methods relevant to the Dir enum.
    /// </summary>
    static class Direction {
        #region Constants
        private const int TOTAL_DIRS = 8;
        #endregion

        #region Relative Directions
        /// <summary>
        /// returns the dir opposite of d
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Dir Reverse(this Dir d) {
            return (Dir)(((int)d + 4) % TOTAL_DIRS);
        }

        /// <summary>
        /// returns the dir to the right of d
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Dir Right(this Dir d) {
            return (Dir)(((int)d + 2) % TOTAL_DIRS);
        }

        /// <summary>
        /// returns the dir to the left of d
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Dir Left(this Dir d) {
            return (Dir)(((int)d + 6) % TOTAL_DIRS);
        }
        #endregion
    }
}