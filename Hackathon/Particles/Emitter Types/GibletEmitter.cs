using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles {
    class GibletEmitter : TimedParticleEmitter {
        #region Constants
        private const int LIFESPAN = 0;
        #endregion

        #region Variables
        #endregion

        #region Constructor & Initialization
        public GibletEmitter(Texture2D[] texture, Vector2 oldPos, Vector2 pos, float secondsMin, float secondsMax, 
            float spawnRadius, int spawnFrequency, int spawnMin, int spawnMax, float coDrag = 0, float depth = 0,
            float speedMin = 1, float speedMax = 1) : 
            base(texture, oldPos, pos, LIFESPAN, spawnRadius, spawnFrequency, secondsMin, 
                secondsMax, spawnMin, spawnMax, null, coDrag, depth, speedMin, speedMax) {

        }

        protected override void AddParticle(Texture2D texture, Vector2 oldPos, Vector2 pos, Vector2 offset, double seconds, float angle, float coDrag, float speed) {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}