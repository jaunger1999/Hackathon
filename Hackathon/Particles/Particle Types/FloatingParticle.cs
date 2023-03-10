using Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using System;

namespace Particles {
    /// <summary>
    /// This particle floats until it expires.
    /// </summary>
    abstract class FloatingParticle : Particle {
        #region Variables
        private Vector2 velocity;
        private float coDrag;
        #endregion

        #region Constructor & Initialization
        public FloatingParticle(Texture2D texture, Vector2 position, int speed = 100) : base(new Sprite(texture), position) {
            Random r = new Random();
            this.velocity = new Vector2(r.Next(-speed, speed), r.Next(-speed, speed));
        }
        #endregion

        #region Update
        /// <summary>
        /// Move the particle and let it fade away.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            Offset(gameTime, velocity);
            //Accelerate(gameTime, velocity);
        }

        /// <summary>
        /// Accelerate this particle.
        /// </summary>
        /// <param name="a"></param>
        private void Accelerate(GameTime gameTime, Vector2 a) {
            velocity += a;
        }
        #endregion
    }
}