using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class Ball : CollisionObject {

        public Ball(int radius) : base(ProceduralTextures.CreateCircle(radius), Color.White, radius) {

            SetPosition(new Vector2(120, 120));
        }

        public override void Update(GameTime gameTime) {

            base.Update(gameTime);
        }

        protected override Movement NextMovement(GameTime gameTime) {
            return new BallMovement(this, Position, Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, Radius);
        }
    }
}
