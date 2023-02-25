using Hackathon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles {
    /// <summary>
    /// These particles disappear after a timer.
    /// </summary>
    class TimedParticle : FloatingParticle {
        #region Constants

        #endregion

        #region Variables
        private Timer expirationTimer;
        #endregion

        #region Constructor & Initialization
        public TimedParticle(Texture2D texture, Vector2 pos) : 
            base(texture, pos) {
        }

        public TimedParticle(Texture2D texture, Vector2 oldPos, Vector2 pos, Vector2 offset, float angle, float speed, float coDrag, float seconds) : 
            base(texture, pos) {
            expirationTimer = new Timer(seconds, start: true);
        }
        #endregion

        #region Update
        /// <summary>
        /// Get closer to expiration.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            expirationTimer.Update(gameTime);
            base.Update(gameTime);
        }
        #endregion

        #region Expiry
        /// <summary>
        /// The particle is expired when the timer runs out.
        /// </summary>
        /// <returns></returns>
        public override bool Expired() {
            return expirationTimer.Complete;
        }
        #endregion
    }
}