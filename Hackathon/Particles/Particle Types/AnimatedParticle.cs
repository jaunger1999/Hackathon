using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using System;

namespace Particles {
    /// <summary>
    /// These particles disappear after animating.
    /// </summary>
    class AnimatedParticle : Particle {
        #region Constants
        #endregion

        #region Variables
        private bool animationFinished;
        private int animationRate;
        #endregion

        #region Constructor & Initialization
        public AnimatedParticle(Sprite s, Vector2 position) : base(s, position) {

        }

        public AnimatedParticle(Texture2D texture, Vector2 pos, int animationRate) : base(texture, pos) {
            this.animationRate = animationRate;
            animationFinished = false;
        }

        public AnimatedParticle(Texture2D texture, Vector2 pos, int animationRate, int[] dims = null) : base(texture, pos, dims) {
            this.animationRate = animationRate;
            animationFinished = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Animate this particle until we reach the end of the animation.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            if (!animationFinished) {
                animationFinished = Sprite.Animate(gameTime);
            }
        }
        #endregion

        #region Expiration
        /// <summary>
        /// The particle expires after the animation finishes.
        /// </summary>
        /// <returns></returns>
        public override bool Expired() {
            return animationFinished;
        }
        #endregion
    }
}