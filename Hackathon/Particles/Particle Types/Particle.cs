using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Particles {
    /// <summary>
    /// Element in an emitter.
    /// </summary>
    abstract class Particle {
        #region Variables
        protected Sprite Sprite { get; private set; }
        protected Vector2 Position { get; private set; }
        #endregion

        #region Constructor & Initialization
        public Particle(Sprite s, Vector2 position) {
            Sprite = s;
            Position = position;
        }

        public Particle(Texture2D texture, Vector2 position) {
            Sprite = new Sprite(texture);
            Position = position;
        }

        public Particle(Texture2D texture, Vector2 position, int[] dims) {
            Sprite = new Sprite(texture, dims);
            Position = position;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update the particle.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Move the particle over.
        /// </summary>
        /// <param name="v"></param>
        protected void Offset(GameTime gameTime, Vector2 v) {
            Position += v * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        #endregion

        #region Expiration
        public abstract bool Expired();
        #endregion

        #region Draw
        /// <summary>
        /// Draw the particle.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch, Color? c = null, float alpha = 1, float depth = 0) {
            //we want the particles to be on top of whatever's being drawn.
            const float DEPTH_OFFSET = 0.001f;

            Sprite.Draw(spriteBatch, Position, c, alpha, depth + DEPTH_OFFSET);
        }
        #endregion
    }
}