using Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles {
    /// <summary>
    /// This emitter spawns timed particles.
    /// </summary>
    class TimedParticleEmitter : Emitter {
        #region Variables
        private double secondsMin, secondsMax;
        private float coDrag, speedMin, speedMax;
        #endregion

        #region Constructor & Initialization
        public TimedParticleEmitter(Texture2D[] texture, Vector2 pos, 
            double secondsMin, double secondsMax, double spawnFrequency, float spawnRadius, int spawnMin, int spawnMax,
            int[][] dims = null, float coDrag = 0, float depth = 0, float speedMin = 1, float speedMax = 1) {
            this.secondsMin = secondsMin;
            this.secondsMax = secondsMax;

            this.coDrag = coDrag;
            this.speedMin = speedMin;
            this.speedMax = speedMax;
        }

        public TimedParticleEmitter(Texture2D[] texture, Vector2 oldPos, Vector2 pos, double secondsMin, 
            double secondsMax, double spawnFrequency, float lifespanLength, float spawnRadius, int spawnMin, int spawnMax,
            int[][] dims = null, float coDrag = 0, float depth = 0, float speedMin = 1, float speedMax = 1) {
            this.secondsMin = secondsMin;
            this.secondsMax = secondsMax;

            this.coDrag = coDrag;
            this.speedMin = speedMin;
            this.speedMax = speedMax;
        }
        #endregion

        #region Update
        /// <summary>
        /// Add a floating particle.
        /// First, calculate how many particles to spawn in the specified range.
        /// Second, calculate a random distance and angle from the origin of the emitter.
        /// Third, choose a random texture for this particle.
        /// Repeat the second and third steps until we've spawned the number of particles
        /// calculated in the first step.
        /// </summary>
        protected override void AddParticles() {
            const float TWO_PI = 2 * MathHelper.Pi;

            float angle, distance, speed;
            int index, seconds, totalToSpawn = Rand.Next(SpawnMin, SpawnMax + 1);

            for (int i = 0; i < totalToSpawn; i++) {
                angle = Rand.NextFloat(TWO_PI);
                distance = Rand.NextFloat(SpawnRadius);
                seconds = Rand.Next(0, 1);
                speed = Rand.NextFloat(speedMin, speedMax);

                index = Rand.Next(Texture.Length);
                AddParticle(Texture[index], OldPosition, Position, Offset(angle, distance), seconds, angle, coDrag, speed);
            }
        }

        /// <summary>
        /// Add a particle to this emitter.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="oldPos"></param>
        /// <param name="pos"></param>
        /// <param name="offset"></param>
        /// <param name="angle"></param>
        /// <param name="speed"></param>
        /// <param name="coDrag"></param>
        /// <param name="frames"></param>
        /// <param name="inertiaMult"></param>
        protected virtual void AddParticle(Texture2D texture, Vector2 oldPos, Vector2 pos, Vector2 offset, double seconds, float angle, float coDrag, float speed) {

        }
        #endregion
    }
}