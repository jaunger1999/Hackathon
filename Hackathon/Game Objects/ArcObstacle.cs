using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hackathon {
    class ArcObstacle : Obstacle {        
        public List<CollisionObject> objs;


        public float Radians { get; private set; }
        public float MinRadians => (Rotation + MathHelper.TwoPi) % MathHelper.TwoPi;
        public float MaxRadians {
            get {
                return MinRadians + Radians;
            }
        }

        public float RotationChange {
            get {
                return Rotation - OldRotation;
            }
        }
        
        public ArcObstacle(int radius, float radians, float thickness) : base(ProceduralTextures.CreateArc(radius, radians, thickness), Color.Black, radius) {
            objs = new List<CollisionObject>();

            Radians = radians;                       

            SetPosition(new Vector2(120, 520));
        }

        
        public void AddCollisionObj(CollisionObject obj) {
            objs.Add(obj);
        }

        public override void Update(GameTime gameTime) {
            if (Active()) {
                UpdateCollidingObjects(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// I'll probably have to scrap this for later. More difficult than I thought.
        /// </summary>
        private void UpdateCollidingObjects(GameTime gameTime) {
            float rotationSpeed = 0.01f * Radius * RotationChange / (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (CollisionObject c in objs) {
                Vector2 away = c.Position - Position;
                Vector2 perp = new Vector2(-away.Y, away.X);
                c.AddVelocity(rotationSpeed * Vector2.Normalize(perp));

                if (GameManager.Collision(c, this)) {
                    c.PushOut(Position + away);
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

        

    }
}
