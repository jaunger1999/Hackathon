using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class Obstacle : GameObject {
        private const int GRAB_RADIUS = 10, GRAB_SQUARED = GRAB_RADIUS * GRAB_RADIUS;
        private const float DEFAULT_ALPHA = 0.7f, GRABBED_ALPHA = 0.9f;

        private Texture2D grabPoint;
        private Vector2 grabOrigin;
        private Color GrabColor => new Color(Color.Pink, alpha);
        private float alpha;
        private bool grabbed, wasHeld;

        public Obstacle(Texture2D texture, Color color, int radius) : base(texture, color, radius) {
            grabPoint = ProceduralTextures.CreateCircle(10);
            grabOrigin = new Vector2(grabPoint.Width / 2, grabPoint.Height / 2);
            alpha = DEFAULT_ALPHA;
        }

        public bool Active() {
            bool what = false;

            if (this != GameManager.HeldObstacle && wasHeld) {
                what = GameManager.Collision(GameManager.ball, this);
            }

            wasHeld = wasHeld && GameManager.Collision(GameManager.ball, this);

            return this != GameManager.HeldObstacle && !wasHeld;
        }

        public void GrabToggle() {
            grabbed = !grabbed;

            if (grabbed) {
                wasHeld = true;
            }

            alpha = grabbed ? GRABBED_ALPHA : DEFAULT_ALPHA;
        }

        public bool PointOnGrab(Vector2 point) {
            return (point - Position).LengthSquared() < GRAB_SQUARED;
        }
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(grabPoint, Position, null, GrabColor, 0, grabOrigin, 1, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }
    }
}
