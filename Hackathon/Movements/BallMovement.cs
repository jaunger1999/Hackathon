using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class BallMovement : Movement {
        public BallMovement(Vector2 start, Vector2 end) : base(start, end) {

        }

        protected override bool Collision() {
            throw new NotImplementedException();
        }
    }
}
