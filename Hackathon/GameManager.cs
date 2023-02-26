using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Hackathon {
    static class GameManager {
        public static List<ArcObstacle> ArcObstacles { get; private set; }
        public static List<CircleObstacle> CircleObstacles { get; private set; }

        public static Ball ball = new Ball(20);
        private static int Score;
        public static Obstacle HeldObstacle { get; private set; }

        public static void IncScore() {
            Score++;
        }

        public static void AddArcObstacle(ArcObstacle obstacle, Vector2 position) {
            if (ArcObstacles == null) {
                ArcObstacles = new List<ArcObstacle>();
            }

            ArcObstacles.Add(obstacle);
            obstacle.SetPosition(position);
        }

        public static void AddCircleObstacle(CircleObstacle obstacle, Vector2 position) {
            if (CircleObstacles == null) {
                CircleObstacles = new List<CircleObstacle>();
            }

            CircleObstacles.Add(obstacle);
            obstacle.SetPosition(position);
        }

        public static bool Collision(CollisionObject obj, Obstacle o) {
            return o is ArcObstacle ? Collision(obj, (ArcObstacle)o) : Collision(obj, (CircleObstacle)o);
        }

        public static bool Collision(CollisionObject obj, CircleObstacle o) {
            return (obj.Position - o.Position).Length() < obj.Radius + o.Radius;
        }

        public static bool Collision(CollisionObject obj, ArcObstacle o) {
            bool collision = false;
            float length = (o.Position - obj.Position).Length(), thetaOne = float.MaxValue, thetaTwo = float.MinValue;
            IntersectionPoints(obj.Position, o.Position, obj.Radius, o.Radius, out Vector2 one, out Vector2 two);

            if (!float.IsNaN(one.X)) {
                Vector2 t = one - o.Position, t2 = two - o.Position;

                thetaOne = (float)(Math.Atan2(t.Y, t.X) + MathHelper.TwoPi) % MathHelper.TwoPi;
                thetaTwo = (float)(Math.Atan2(t2.Y, t2.X) + MathHelper.TwoPi) % MathHelper.TwoPi;
            }

            //complete circle collision.
            //collision = length < (o[i].Radius + radius) && //outside collision
            //length > Math.Abs(o[i].Radius - radius); //inside collision

            //arc collision

            for (int j = -1; j <= 1 && !collision; j++) {
                collision = (thetaOne > o.MinRadians + j * MathHelper.TwoPi && thetaOne < o.MaxRadians + j * MathHelper.TwoPi) ||
                    (thetaTwo > o.MinRadians + j * MathHelper.TwoPi && thetaTwo < o.MaxRadians + j * MathHelper.TwoPi);
            }

            if (collision) {
                //o.AddCollisionObj(obj);
            }

            return collision;
        }

        public static bool Collision(CollisionObject obj) {
            List<ArcObstacle> o = ArcObstacles;
            bool collision = false;

            for (int i = 0; i < o.Count && !collision; i++) {
                if (o[i].Active()) {
                    float length = (o[i].Position - obj.Position).Length(), thetaOne = float.MaxValue, thetaTwo = float.MinValue;
                    IntersectionPoints(obj.Position, o[i].Position, obj.Radius, o[i].Radius, out Vector2 one, out Vector2 two);

                    if (!float.IsNaN(one.X)) {
                        Vector2 t = one - o[i].Position, t2 = two - o[i].Position;

                        thetaOne = (float)(Math.Atan2(t.Y, t.X) + MathHelper.TwoPi) % MathHelper.TwoPi;
                        thetaTwo = (float)(Math.Atan2(t2.Y, t2.X) + MathHelper.TwoPi) % MathHelper.TwoPi;
                    }

                    //complete circle collision.
                    //collision = length < (o[i].Radius + radius) && //outside collision
                    //length > Math.Abs(o[i].Radius - radius); //inside collision

                    //arc collision

                    for (int j = -1; j <= 1 && !collision; j++) {
                        collision = (thetaOne > o[i].MinRadians + j * MathHelper.TwoPi && thetaOne < o[i].MaxRadians + j * MathHelper.TwoPi) ||
                            (thetaTwo > o[i].MinRadians + j * MathHelper.TwoPi && thetaTwo < o[i].MaxRadians + j * MathHelper.TwoPi);
                    }

                    if (collision) {
                        //o[i].AddCollisionObj(obj);
                    }
                }
            }

            return collision;
        }

        /// <summary>
        /// Use out parameters for two solutions. 
        /// </summary>
        /// <param name="positionOne"></param>
        /// <param name="positionTwo"></param>
        /// <param name="r2"></param>
        /// <param name="one"></param>
        /// <param name="two"></param>
        private static void IntersectionPoints(Vector2 positionOne, Vector2 positionTwo, float r1, float r2, out Vector2 one, out Vector2 two) {
            //Source: https://math.stackexchange.com/questions/256100/how-can-i-find-the-points-at-which-two-circles-intersect
            float x1 = positionOne.X, y1 = positionOne.Y, x2 = positionTwo.X, y2 = positionTwo.Y, R = (positionOne - positionTwo).Length();
            Vector2 baseVector = 0.5f * new Vector2(x1 + x2, y1 + y2) + (r1 * r1 - r2 * r2) / (2 * R * R) * new Vector2(x2 - x1, y2 - y1),
                addition = 0.5f * (float)Math.Sqrt(2 * (r1 * r1 + r2 * r2) / (R * R) - (float)Math.Pow((r1 * r1 - r2 * r2), 2) / (R * R * R * R) - 1) * new Vector2(y2 - y1, x1 - x2);
            one = baseVector + addition;
            two = baseVector - addition;
        }

        public static void Update(GameTime gameTime) {
            ball.Update(gameTime);

            CheckClickedObstacle();
            UpdateObstacles(gameTime);
            OldUpdateObstacles();
        }

        private static void UpdateObstacles(GameTime gameTime) {
            for (int i = 0; i < ArcObstacles.Count; i++) {
                ArcObstacles[i].AddToRotation(Input.RotChange);
                ArcObstacles[i].Update(gameTime);
            }
        }

        private static void CheckClickedObstacle() {
            if (Input.LeftMousePressed()) {
                if (HeldObstacle == null) {
                    for (int i = 0; i < ArcObstacles.Count && HeldObstacle == null; i++) {
                        if (ArcObstacles[i].PointOnGrab(Input.MousePosition.ToVector2())) {
                            HeldObstacle = ArcObstacles[i];
                            HeldObstacle.GrabToggle();
                        }
                    }

                    for (int i = 0; i < CircleObstacles.Count && HeldObstacle == null; i++) {
                        if (CircleObstacles[i].PointOnGrab(Input.MousePosition.ToVector2())) {
                            HeldObstacle = CircleObstacles[i];
                            HeldObstacle.GrabToggle();
                        }
                    }
                }
                else if (!Collision(HeldObstacle)) {
                    HeldObstacle.GrabToggle();
                    HeldObstacle = null;
                }
            }

            HeldObstacle?.SetPosition(Input.MousePosition.ToVector2());
        }

        private static bool Collision(Obstacle o) {
            bool collision = false;
            Obstacle ot;

            for (int i = 0; i < ArcObstacles.Count && !collision; i++) {
                ot = ArcObstacles[i];
                collision = o != ot && (o.Position - ot.Position).Length() < (o.Radius + ot.Radius);
            }

            for (int i = 0; i < CircleObstacles.Count && !collision; i++) {
                ot = CircleObstacles[i];
                collision = o != ot && (o.Position - ot.Position).Length() < (o.Radius + ot.Radius);
            }

            return collision;
        }

        private static void OldUpdateObstacles() {
            for (int i = 0; i < ArcObstacles.Count; i++) {
                ArcObstacles[i].OldUpdate();
            }

            foreach (CircleObstacle c in CircleObstacles) {
                c.OldUpdate();
            }
        }

        public static void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont) {
            spriteBatch.DrawString(spriteFont, "Score: " + Score, Vector2.Zero, Color.White);

            ball.Draw(spriteBatch);

            foreach (ArcObstacle o in ArcObstacles) {
                o.Draw(spriteBatch);
            }

            foreach (CircleObstacle c in CircleObstacles) {
                c.Draw(spriteBatch);
            }
        }
    }
}