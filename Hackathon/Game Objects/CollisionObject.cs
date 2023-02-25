using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    /// <summary>
    /// These objects interact with obstacles and each other.
    /// </summary>
    abstract class CollisionObject : GameObject {
        private static Vector2 gravity = new Vector2(0,5);

        protected Vector2 Velocity { get; private set; }

        public static void SetGravity(Vector2 v) {
            gravity = v;
        }

        public CollisionObject(Texture2D texture, Color color, int radius) : base(texture, color, radius) {

        }

        public void AddVelocity(Vector2 v) {
            Velocity += v;
        }

        public override void Update(GameTime gameTime) {
            Velocity += gravity;

            UpdatePosition(gameTime);
            base.Update(gameTime);
        }

        private void UpdatePosition(GameTime gameTime) {
            Movement m = NextMovement(gameTime);
            SetPosition(m.FurthestAvailablePosition());

            if (m.HitY) {
                Velocity *= Vector2.UnitX;
            }
            if (m.HitX) {
                Velocity *= Vector2.UnitY;
            }
        }

        protected abstract Movement NextMovement(GameTime gameTime);
    }
}
