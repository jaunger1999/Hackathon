using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles {
    /// <summary>
    /// These particles are deleted when their opacity reaches zero.
    /// </summary>
    class FadingParticle : FloatingParticle {
        #region Variables
        private float fadeRate, opacity;
        #endregion

        #region Constructor & Initialization
        public FadingParticle(Texture2D texture, Vector2 oldPos, Vector2 pos, Vector2 velocity, float angle,
            float fadeRate = 0.005f, float opacity = 1, float speed = 1, float coDrag = 0) : 
            base(texture, pos) {
            this.fadeRate = fadeRate;
            this.opacity = opacity;
        }
        #endregion

        #region Update
        /// <summary>
        /// Move the particle and let it fade away.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            opacity -= fadeRate;
            base.Update(gameTime);
        }
        #endregion

        #region Expiration
        /// <summary>
        /// When the particle is invisible we don't need it anymore.
        /// </summary>
        /// <returns></returns>
        public override bool Expired() {
            return opacity <= 0;
        }
        #endregion

        #region Draw
        /// <summary>
        /// Let's us set the opacity of this particle.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="c"></param>
        /// <param name="alpha"></param>
        /// <param name="depth"></param>
        public override void Draw(SpriteBatch spriteBatch, Color? c = null, float alpha = 1, float depth = 0) {
            base.Draw(spriteBatch, c, opacity * alpha, depth);
        }
        #endregion
    }
}