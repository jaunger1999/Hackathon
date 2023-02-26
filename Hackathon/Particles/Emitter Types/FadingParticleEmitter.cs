using Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles {
    /// <summary>
    /// Shoots out a bunch of floating/fading particles.
    /// </summary>
    class FadingParticleEmitter : Emitter {
        #region Variables
        private float coDrag = 0.1f, fadeMin = 5, fadeMax = 10, opacityMin = 0.5f, opacityMax = 1, speedMin = 30, speedMax = 50;
        #endregion

        #region Constructor & Initialization
        public FadingParticleEmitter(Texture2D tex, Vector2 pos, double spawnFreq, int min, int max) : base(tex, pos, spawnFreq, min,max) {

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
            
            float angle, distance, fadeRate, opacity, speed;
            int index, totalToSpawn = Rand.Next(SpawnMin, SpawnMax + 1);
            
            for (int i = 0; i < totalToSpawn; i++) {
                angle = Rand.NextFloat(TWO_PI);
                distance = Rand.NextFloat(SpawnRadius);
                fadeRate = Rand.NextFloat(fadeMin, fadeMax);
                opacity = Rand.NextFloat(opacityMin, opacityMax);
                speed = Rand.NextFloat(speedMin, speedMax);

                index = Rand.Next(Texture.Length);
                AddParticle(new FadingParticle(Texture[index], OldPosition, Position, new Vector2(100, 0), angle));
            }
        }
        #endregion
    }
}