using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Particles {
    /// <summary>
    /// These particles depict a meat chunk with blood dripping off.
    /// </summary>
    class Giblet : TimedParticle {
        #region Constants
        private const float CO_DRAG = 0.05f, INERTIA_MULT = 0.25f, SPAWN_RADIUS = 1, SPEED_MIN = 0.25f, SPEED_MAX = 0.5f;

        private const int SPAWN_FREQUENCY = 1, FRAMES_MIN = 15, FRAMES_MAX = 60, SPAWN_MIN = 4, SPAWN_MAX = 7, TOTAL_PARTICLES = 2;

        private const string BLOOD_PARTICLE = "blooddrop";
        #endregion

        #region Variables
        private static Texture2D[] bloodParticles;

        private TimedParticleEmitter bloodEmitter;
        #endregion

        #region Constructor & Initialization
        public Giblet(Texture2D texture, Vector2 oldPos, Vector2 pos, Vector2 offset, float angle, float speed, float coDrag, int frames) : 
            base(texture, pos) {
            bloodEmitter = new TimedParticleEmitter(bloodParticles, pos, SPAWN_RADIUS, SPAWN_FREQUENCY, 
                SPAWN_MIN, SPAWN_MAX, FRAMES_MIN, FRAMES_MAX, coDrag: CO_DRAG, speedMin: SPEED_MIN, speedMax: SPEED_MAX);

            ParticleManager.AddEmitter(bloodEmitter);
        }

        public static void Load(ContentManager content, string goreDir) {
            bloodParticles = new Texture2D[TOTAL_PARTICLES];

            for (int i = 0; i < TOTAL_PARTICLES; i++) {
                bloodParticles[i] = content.Load<Texture2D>(goreDir + BLOOD_PARTICLE + i);
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update this giblet's position.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            bloodEmitter.UpdatePosition(Position);
            base.Update(gameTime);

            //If we've expired on this frame we also want to stop the blood
            //emitter from producing any more particles.
            if (Expired()) {
                bloodEmitter.Cancel();
            }
        }
        #endregion
    }
}
