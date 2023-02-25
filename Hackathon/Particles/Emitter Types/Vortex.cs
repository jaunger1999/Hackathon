using Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles {
    class Vortex : Emitter {
        #region Variables
        private float centerRadius, coDrag, g, opacityMin, opacityMax, speedMin, speedMax;
        #endregion

        #region Constructor & Initialization
        
        #endregion

        #region Update
        /// <summary>
        /// Give this particle access to the center of the vortex
        /// for gravity purposes.
        /// </summary>
        /// <param name="p"></param>
        protected override void UpdateParticle(Particle p) {
            if (p is VortexParticle) {
                ((VortexParticle)p).UpdateVortexCenter(Position);
            }

            base.UpdateParticle(p);
        }
        #endregion

        #region Add Particles
        /// <summary>
        /// Launch the particles in the direction of a vector perpendicular 
        /// to the vector from the vortex center and the particle spawn point.
        /// </summary>
        protected override void AddParticles() {
            const float TWO_PI = 2 * MathHelper.Pi;

            float angle, distance, opacity, speed;
            int index, totalToSpawn = Rand.Next(SpawnMin, SpawnMax + 1);

            for (int i = 0; i < totalToSpawn; i++) {
                angle = Rand.NextFloat(TWO_PI);
                distance = Rand.NextFloat(centerRadius, SpawnRadius);
                opacity = Rand.NextFloat(opacityMin, opacityMax);
                speed = Rand.NextFloat(speedMin, speedMax);

                index = Rand.Next(Texture.Length);
                AddParticle(new VortexParticle(Texture[index], OldPosition, Position, Offset(angle, distance), angle, centerRadius, opacity, speed, coDrag, g));
            }
        }
        #endregion

        #region Draw
        #endregion
    }
}