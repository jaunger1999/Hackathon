using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hackathon {
    class BallMovement : Movement {
        private float radius;

        public BallMovement(Vector2 position, Vector2 velocity, float radius) : base(position, velocity) {
            this.radius = radius;
        }

        protected override bool Collision(Vector2 position) {
            List<Obstacle> o = GameManager.Obstacles;
            bool collision = false;

            for (int i = 0; i < o.Count && !collision; i++) {
                float length = (o[i].Position - position).Length(), thetaOne = float.MaxValue, thetaTwo = float.MinValue;
                IntersectionPoints(position, o[i].Position, o[i].Radius, out Vector2 one, out Vector2 two);

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
        private void IntersectionPoints(Vector2 positionOne, Vector2 positionTwo, float r2, out Vector2 one, out Vector2 two) {
            //Source: https://math.stackexchange.com/questions/256100/how-can-i-find-the-points-at-which-two-circles-intersect
            float x1 = positionOne.X, y1 = positionOne.Y, x2 = positionTwo.X, y2 = positionTwo.Y, R = (positionOne - positionTwo).Length(), r1 = radius;
            Vector2 baseVector = 0.5f * new Vector2(x1 + x2, y1 + y2) + (r1 * r1 - r2 * r2)/(2 * R * R) * new Vector2(x2 - x1, y2 - y1),
                addition = 0.5f * (float)Math.Sqrt( 2 * (r1 * r1 + r2 * r2)/(R * R) - (float)Math.Pow((r1 * r1 - r2 * r2), 2)/(R*R*R*R) - 1 ) * new Vector2(y2 - y1, x1 - x2);
            one = baseVector + addition;
            two = baseVector - addition;
        }
    }
}