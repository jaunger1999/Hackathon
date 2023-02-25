using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class BallMovement : Movement {
        public BallMovement(Vector2 position, Vector2 velocity) : base(position, velocity) {

        }

        protected override bool Collision(Vector2 position) {
            List<Obstacle> o = GameManager.Obstacles;
            bool collision = false, inside = false;

            for (int i = 0; i < o.Count; i++) {
                inside = (o[i].Position - position).LengthSquared() < o[i].RadiusSquared;
            }

            collision = inside;

            return collision;
        }
    }
}
