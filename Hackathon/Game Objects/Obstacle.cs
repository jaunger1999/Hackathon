using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hackathon {
    class Obstacle : GameObject {
        private const int GRAB_RADIUS = 10, GRAB_SQUARED = GRAB_RADIUS * GRAB_RADIUS;
        private const float DEFAULT_ALPHA = 0.7f, GRABBED_ALPHA = 0.9f;

        public List<CollisionObject> objs, oldObjs;

        public Vector2 OldVelocity { get; private set; }

        public float Radians { get; private set; }
        public float MinRadians => (Rotation + MathHelper.TwoPi) % MathHelper.TwoPi;
        public float MaxRadians {
            get {
                return MinRadians + Radians;
            }
        }

        public Vector2 Velocity => Position - OldPosition;

        private Texture2D grabPoint;
        private Vector2 grabOrigin;
        private Color GrabColor => new Color(Color.Pink, alpha);
        private float alpha;
        private bool grabbed, wasHeld;

        public Obstacle(int radius, float radians, float thickness) : base(ProceduralTextures.CreateArc(radius, radians, thickness), Color.Black, radius) {
            objs = new List<CollisionObject>();
            oldObjs = new List<CollisionObject>();

            Radians = radians;

            grabPoint = ProceduralTextures.CreateCircle(10);
            grabOrigin = new Vector2(grabPoint.Width / 2, grabPoint.Height / 2);
            alpha = DEFAULT_ALPHA;

            SetPosition(new Vector2(120, 520));
        }

        public bool Active() {
            wasHeld = wasHeld && GameManager.Collision(this);

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

        public void AddCollisionObj(CollisionObject obj) {
            objs.Add(obj);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void OldUpdate() {
            OldVelocity = Velocity;
            base.OldUpdate();
        }

        /// <summary>
        /// I'll probably have to scrap this for later. More difficult than I thought.
        /// </summary>
        private void UpdateCollidingObjects() {
            foreach (CollisionObject c in objs) {
                c.SetPosition(c.Position + Velocity);

                if (Velocity.LengthSquared() == 0) {
                    Vector2 toward = Vector2.Normalize(Position - c.Position);

                    c.SetPosition(c.Position + toward);
                }

                if (Velocity.LengthSquared() < OldVelocity.LengthSquared()) {
                    //c.AddVelocity(OldVelocity);
                }
            }

            /*foreach (CollisionObject c in oldObjs) {
                if (!objs.Contains(c)) {
                    //c.AddVelocity(OldVelocity);
                }
            }

            oldObjs = objs;
            objs = new List<CollisionObject>();*/
            objs.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(grabPoint, Position, null, GrabColor, 0, grabOrigin, 1, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

    }
}
