using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class CircleObstacle : Obstacle {
        public CircleObstacle(int radius, Color color) : base(ProceduralTextures.CreateCircle(radius), color, radius) {

        }
    }
}
