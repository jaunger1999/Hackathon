using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Particles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class Ball : CollisionObject {
        public bool Alive = true;

        public Ball(int radius) : base(ProceduralTextures.CreateCircle(radius), Color.White, radius) {
            Alive = false;
        }

        public override void Update(GameTime gameTime) {
            if (Position.Y > Resolution.VirtualResolution.Y + Radius) {
                Alive = false;
                ParticleManager.AddEmitter(new FadingParticleEmitter(ProceduralTextures.CreateCircle(10, Color.Red), Position, 10, 5, 10));
            }


            base.Update(gameTime);
        }

        protected override Movement NextMovement(GameTime gameTime) {
            return new BallMovement(this, Position, Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, Radius);
        }
    }
}
