using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class Ball : GameObject {
        private Vector2 velocity;

        public Ball(int radius) : base(ProceduralTextures.CreateCircle(radius), Color.White, radius) {

        }
    }
}
