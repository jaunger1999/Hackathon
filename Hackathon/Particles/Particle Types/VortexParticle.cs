using Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles {
    /// <summary>
    /// These particles disappear after being pulled into a vortex.
    /// </summary>
    class VortexParticle : Particle {
        #region Variables
        private bool InVortex => fromCenter.Length() < centerRadius;

        private Vector2 fromCenter, velocity, vortexCenter;
        private float coDrag, centerRadius, opacity, g;
        #endregion

        #region Constructor & Initialization
        public VortexParticle(Texture2D texture, Vector2 oldPos, Vector2 pos, Vector2 offset, float angle, float centerRadius,
            float opacity = 1, float speed = 1, float coDrag = 0, float g = 1) : base(texture, pos + offset) {
            Vector2 direction = new Vector2(-offset.Y, offset.X);

            this.centerRadius = centerRadius;
            this.opacity = opacity;
            this.coDrag = coDrag;
            this.g = g;

            fromCenter = Vector2.Zero;
            vortexCenter = pos;

            //pos - oldPos for inertia.
            //the rest launches the particle away to the side of the vortex.
            velocity = (pos - oldPos) + speed * Vector2.Normalize(direction);
        }
        #endregion

        #region Update
        /// <summary>
        /// Move the particle and let it fade away.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            fromCenter = Position - vortexCenter;

            Offset(gameTime, velocity);
            Accelerate(Physics.Drag(coDrag, velocity));
            Accelerate(g * Vector2.Normalize(fromCenter));
        }

        /// <summary>
        /// Gives the particle a new center to refer to.
        /// </summary>
        /// <param name="center"></param>
        public void UpdateVortexCenter(Vector2 center) {
            vortexCenter = center;
        }

        /// <summary>
        /// Accelerate this particle.
        /// </summary>
        /// <param name="a"></param>
        private void Accelerate(Vector2 a) {
            velocity += a;
        }
        #endregion

        #region Expired
        /// <summary>
        /// The particle can be taken out when it's in the center of the vortex.
        /// </summary>
        /// <returns></returns>
        public override bool Expired() {
            return InVortex;
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
