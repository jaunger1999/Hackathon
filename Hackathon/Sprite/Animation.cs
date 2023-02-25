namespace Sprites {
    /// <summary>
    /// Struct which contains information about how many frames an animation is, how long it should last
    /// and additional information derived from those two variables.
    /// </summary>
    struct Animation {
        public int Frames { get; private set; }
        public float Seconds { get; private set; }
        public float TimeBetweenFrames { get; private set; }

        public Animation(int frames, float seconds) {
            Frames = frames;
            Seconds = seconds;
            TimeBetweenFrames = seconds / frames;
        }
    }
}