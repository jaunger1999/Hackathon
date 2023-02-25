using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Particles {
    /// <summary>
    /// Keeps track of all particle emitters.
    /// </summary>
    static class ParticleManager {
        #region Constants
        private const int MAX_EMITTERS = 1000;
        #endregion

        #region Variables
        private static List<Emitter> emitters;
        private static List<Particle> particles;
        #endregion

        #region Constructor & Initialization
        static ParticleManager() {
            emitters = new List<Emitter>();
            particles = new List<Particle>();
        }
        #endregion

        #region Add & Remove Emitters
        /// <summary>
        /// Add an emitter to the manager's list.
        /// </summary>
        /// <param name="e"></param>
        public static void AddEmitter(Emitter e) {
            if (emitters.Count < MAX_EMITTERS) {
                emitters.Add(e);
            }
        }

        /// <summary>
        /// Add a single particle.
        /// </summary>
        /// <param name="p"></param>
        public static void AddParticle(Particle p) {
            particles.Add(p);
        }

        /// <summary>
        /// Remove every emitter.
        /// </summary>
        public static void Clear() {
            emitters.RemoveRange(0, emitters.Count);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update all emitters.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime) {
            for (int i = 0; i < emitters.Count; i++) {
                emitters[i].Update(gameTime);
            }

            for (int i = 0; i < particles.Count; i++) {
                particles[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Remove all deleteable emitters.
        /// </summary>
        public static void Cleanup() {
            int i = 0;

            while (i < emitters.Count) {
                if (emitters[i].Deleteable) {
                    emitters.RemoveAt(i);
                }
                else {
                    i++;
                }
            }

            i = 0;

            while (i < particles.Count) {
                if (particles[i].Expired()) {
                    particles.RemoveAt(i);
                }
                else {
                    i++;
                }
            }
        }
        #endregion

        #region Draw
        /// <summary>
        /// Draw all the pretty particles.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch) {
            foreach (Emitter e in emitters) {
                e.Draw(spriteBatch);
            }

            foreach (Particle p in particles) {
                p.Draw(spriteBatch);
            }
        }
        #endregion
    }
}