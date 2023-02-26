using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hackathon {
    /// <summary>
    /// These objects interact with obstacles and each other.
    /// </summary>
    abstract class CollisionObject : GameObject {
        private const float MAX_VELOCITY = 1000, MAX_VELOCITY_SQUARED = MAX_VELOCITY * MAX_VELOCITY;

        private static Vector2 gravity = new Vector2(0,8);

        protected Vector2 Velocity { get; private set; }

        public static void SetGravity(Vector2 v) {
            gravity = v;
        }

        public CollisionObject(Texture2D texture, Color color, int radius) : base(texture, color, radius) {

        }

        public void PushOut(Vector2 dir) {
            dir = Vector2.Normalize(dir);

            while (GameManager.Collision(this)) {
                SetPosition(Position + dir);
            }
        }

        public void AddVelocity(Vector2 v) {
            Velocity += v;

            if (Velocity.LengthSquared() > MAX_VELOCITY_SQUARED) {
                Velocity = Vector2.Normalize(Velocity) * MAX_VELOCITY;
            }
        }

        public void MirrorVelocity(Vector2 other, float bounce = 1) {
            Vector2 n = Vector2.Normalize(Position - other);
            Vector2 oldVelocity = Velocity;
            float dot = Vector2.Dot(Velocity, n);

            Velocity = bounce * (Velocity - 2 * dot * n);
        }

        public override void Update(GameTime gameTime) {
            Velocity += gravity;

            UpdatePosition(gameTime);

            if (Position.X < Radius || Position.X > Resolution.VirtualResolution.X - Radius) {
                Velocity = new Vector2(-Velocity.X, Velocity.Y);
            }
            if (Position.Y < Radius) {
                Velocity = new Vector2(Velocity.X, -Velocity.Y);
            }

            

            base.Update(gameTime);
        }

        private void UpdatePosition(GameTime gameTime) {
            Movement m = NextMovement(gameTime);
            SetPosition(m.FurthestAvailablePosition());
        }

        protected abstract Movement NextMovement(GameTime gameTime);
    }
}
