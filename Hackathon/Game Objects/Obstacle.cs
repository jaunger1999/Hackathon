using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class Obstacle : GameObject {
        private const int GRAB_RADIUS = 10, GRAB_SQUARED = GRAB_RADIUS * GRAB_RADIUS;

        private Texture2D grabPoint;
        private Vector2 grabOrigin;
        private static readonly Color grabColor = new Color(Color.Pink, 0.5f);

        public Obstacle(int radius, float radians, float thickness) : base(ProceduralTextures.CreateArc(radius, radians, thickness), Color.Black, radius) {
            grabPoint = ProceduralTextures.CreateCircle(10);
            grabOrigin = new Vector2(grabPoint.Width / 2, grabPoint.Height / 2);


            SetPosition(new Vector2(120, 320));
        }

        public bool PointOnGrab(Vector2 point) {
            return (point - Position).LengthSquared() < GRAB_SQUARED;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(grabPoint, Position, null, Color.Pink, 0, grabOrigin, 1, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

    }
}
