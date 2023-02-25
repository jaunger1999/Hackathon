using Microsoft.Xna.Framework;
using System;

namespace Hackathon {
    /// <summary>
    /// Counts down and performs an action if one is set after reaching zero.
    /// </summary>
    class Timer {
        private const float SECONDS_PER_MINUTE = 60;

        public float InitTime { get; private set; } //the first time input
        public float SecondsRemaining { get; private set; }
        public float SecondsPassed => lastSetTime - SecondsRemaining;
        public bool Complete => SecondsRemaining <= 0;
        public bool JustCompleted => Complete && (oldTime > 0 || JustSet);
        public bool JustSet { get; private set; } //just in case the timer is set to zero.

        private int Minutes => (int)(SecondsRemaining / SECONDS_PER_MINUTE);
        private int Seconds => (int)(SecondsRemaining % SECONDS_PER_MINUTE);
        private int DisplayedMilliseconds => (int)(100 * Milliseconds);
        private float Milliseconds => (SecondsRemaining % SECONDS_PER_MINUTE) % 1;

        private Action a;
        private float lastSetTime, oldTime; //lets us see what time was last frame.

        public Timer(Action a = null) {
            InitTime = SecondsRemaining = 0;
            oldTime = 0;
            JustSet = false;
            SetAction(a);
        }

        public Timer(float secondsRemaining, Action a = null, bool start = false) {
            JustSet = start;
            InitTime = secondsRemaining;
            SecondsRemaining = lastSetTime = start ? InitTime : 0;
            oldTime = SecondsRemaining;
            SetAction(a);
        }

        /// <summary>
        /// Sets an event that takes place when this timer is over.
        /// </summary>
        private void SetAction(Action a) {
            this.a = a;
        }

        /// <summary>
        /// Count down and execute this timer's action if completed.
        /// </summary>
        public void Update(GameTime gameTime) {
            //update what time was remaining last frame.
            oldTime = SecondsRemaining;

            CountDown(gameTime);
            ActionCheck();
        }

        /// <summary>
        /// Count up.
        /// </summary>
        public bool Rewind(GameTime gameTime, float speed = 1) {
            //update what time was remaining last frame.
            oldTime = SecondsRemaining;

            if (SecondsRemaining < lastSetTime) {
                SecondsRemaining = Math.Min(lastSetTime, SecondsRemaining + (float)gameTime.ElapsedGameTime.TotalSeconds * speed);
            }

            return SecondsRemaining >= lastSetTime;
        }

        /// <summary>
        /// Count down the timer.
        /// </summary>
        private void CountDown(GameTime gameTime) {
            if (SecondsRemaining > 0) {
                SecondsRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// If the timer just completed we invoke this timer's action.
        /// </summary>
        private void ActionCheck() {
            if (JustCompleted) {
                a?.Invoke();
                JustSet = false;
            }
        }

        /// <summary>
        /// Sets this timer
        /// </summary>
        /// <param name="x"></param>
        public void SetTime(float x) {
            SecondsRemaining = lastSetTime = x;
            JustSet = true;
        }

        /// <summary>
        /// Sets the inital time.
        /// </summary>
        public void SetTime() {
            SecondsRemaining = lastSetTime = InitTime;
            JustSet = true;
        }

        /// <summary>
        /// Cancel out of this timer.
        /// </summary>
        public void Quit() {
            SecondsRemaining = 0;
        }

        /// <summary>
        /// A string of the time remaining in an XX:XX:XX format.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Minutes + ":" + Seconds.ToString("00") + ":" + DisplayedMilliseconds.ToString("00");
        }
    }
}