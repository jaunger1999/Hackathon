using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class Obstacle : GameObject {
        private float rotation;

        public Obstacle(int radius, float radians, float thickness) : base(ProceduralTextures.CreateArc(radius, radians, thickness), Color.Black, radius) {

        }
    }
}
