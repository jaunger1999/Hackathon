using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Hackathon {
    class Obstacle : GameObject {
        private const int GRAB_RADIUS = 10, GRAB_SQUARED = GRAB_RADIUS * GRAB_RADIUS;
        private const float DEFAULT_ALPHA = 0.7f, GRABBED_ALPHA = 0.9f;

        public float Radians { get; private set; }
        public float MinRadians => (Rotation + MathHelper.TwoPi) % MathHelper.TwoPi;
        public float MaxRadians {
            get {
                return MinRadians + Radians;
            }
        }

        private Texture2D grabPoint;
        private Vector2 grabOrigin;
        private Color GrabColor => new Color(Color.Pink, alpha);
        private float alpha;
        private bool grabbed;

        public Obstacle(int radius, float radians, float thickness) : base(ProceduralTextures.CreateArc(radius, radians, thickness), Color.Black, radius) {
            Radians = radians;

            grabPoint = ProceduralTextures.CreateCircle(10);
            grabOrigin = new Vector2(grabPoint.Width / 2, grabPoint.Height / 2);
            alpha = DEFAULT_ALPHA;

            SetPosition(new Vector2(120, 520));
        }

        public void GrabToggle() {
            grabbed = !grabbed;

            alpha = grabbed ? GRABBED_ALPHA : DEFAULT_ALPHA;
        }

        public bool PointOnGrab(Vector2 point) {
            return (point - Position).LengthSquared() < GRAB_SQUARED;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(grabPoint, Position, null, GrabColor, 0, grabOrigin, 1, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

    }
}
