using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Hackathon;

namespace Particles {
    /// <summary>
    /// Emits particles.
    /// </summary>
    abstract class Emitter {
        #region Constants
        private const int MAX_PARTICLES = 200;
        #endregion

        #region Variables
        protected static Random Rand { get; private set; }
        private static double seconds;

        public bool Deleteable => madeDeleteable || (Expired && particles.Count == 0);
        private bool Expired => cancelled || (!immortal && lifespan != null && lifespan.Complete && !lifespan.JustSet);
        private bool CanAddMore => particles.Count + SpawnMax <= MAX_PARTICLES;

        protected Texture2D[] Texture { get; private set; }
        protected Vector2 OldPosition { get; private set; }
        protected Vector2 Position { get; private set; }
        protected int[][] Dims { get; private set; }
        protected float SpawnRadius { get; private set; }
        protected int SpawnMin { get; private set; }
        protected int SpawnMax { get; private set; }

        private List<Particle> particles;
        private Timer lifespan;
        private double spawnFrequency;
        private float depth;
        private int minParticlesPerSecond, maxParticlesPerSecond;
        private bool cancelled, immortal, madeDeleteable, randomSpawnFreq;
        #endregion

        #region Constructor & Initialization
        static Emitter() {
            Rand = new Random();
        }

        public Emitter(Texture2D tex, Vector2 pos, double spawnFreq, int min, int max) {
            Position = pos;
            SpawnMin = min;
            SpawnMax = max;
            Texture = new Texture2D[] { tex };
            spawnFrequency = spawnFreq;
            particles = new List<Particle>();
            AddParticles();
        }

        /// <summary>
        /// Set the random pointer.
        /// </summary>
        /// <param name="r"></param>
        public static void SetRandom(int seed) {
            Rand = new Random(seed);
        }
        #endregion

        #region Update
        /// <summary>
        /// Used to spawn particles every x frames.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void UpdateStaticCounter(GameTime gameTime) {
            seconds = gameTime.TotalGameTime.Seconds;
        }

        /// <summary>
        /// Update the particles and position of the emitter.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds; 

            if (!Expired) {
                if (CanAddMore && seconds % spawnFrequency < elapsedSeconds) {
                    //AddParticles();
                    cancelled = true;
                }

                lifespan?.Update(gameTime);
            }

            for (int i = 0; i < particles.Count; i++) {
                UpdateParticle(particles[i]);
                particles[i].Update(gameTime);

                if (particles[i].Expired()) {
                    particles.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Let the emitter move around.
        /// </summary>
        /// <param name="position"></param>
        public void UpdatePosition(Vector2 position) {
            OldPosition = Position;
            Position = position;
        }

        /// <summary>
        /// Stop this emitter from existing.
        /// </summary>
        public void Cancel() {
            cancelled = true;
        }

        public void MakeDeleteable() {
            madeDeleteable = true;
        }

        /// <summary>
        /// Put a new particle in the game world.
        /// Each inheriting class is going to add different types.
        /// </summary>
        protected abstract void AddParticles();

        /// <summary>
        /// Add a particle.
        /// </summary>
        /// <param name="p"></param>
        protected void AddParticle(Particle p) {
            particles.Add(p);
        }

        /// <summary>
        /// Gives subclasses an opportunity to update particles
        /// on their own.
        /// </summary>
        /// <param name="p"></param>
        protected virtual void UpdateParticle(Particle p) {

        }
        #endregion

        #region Offset Calculation
        /// <summary>
        /// Calculates a point so and so far away from an origin at an angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        protected Vector2 Offset(float angle, float distance) {
            return distance * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
        #endregion

        #region Draw
        /// <summary>
        /// Draw all the particles.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) {
            foreach (Particle p in particles) {
                p.Draw(spriteBatch, Color.White, depth: depth);
            }
        }
        #endregion
    }
}