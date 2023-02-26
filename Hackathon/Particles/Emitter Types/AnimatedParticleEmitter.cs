using Extensions;
using Microsoft.Xna.Framework;

namespace Particles {
    /*class AnimatedParticleEmitter : Emitter {
        #region Constants

        #endregion

        #region Variables
        private double animationRateMin, animationRateMax;
        #endregion

        #region Constructor & Initialization
        
        #endregion

        #region Update
        /// <summary>
        /// Add a sparkle.
        /// </summary>
        protected override void AddParticles() {
            const float TWO_PI = 2 * MathHelper.Pi;

            int animationRate;
            float angle, distance;
            int index, totalToSpawn = Rand.Next(SpawnMin, SpawnMax + 1);

            for (int i = 0; i < totalToSpawn; i++) {
                animationRate = Rand.Next(0, 1);
                angle = Rand.NextFloat(TWO_PI);
                distance = Rand.NextFloat(SpawnRadius);

                index = Rand.Next(Texture.Length);

                AddParticle(new AnimatedParticle(Texture[index], Position + Offset(angle, distance), animationRate, Dims[index]));
            }
        }
        #endregion
    }*/
}