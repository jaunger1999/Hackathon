using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Particles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class Ball : CollisionObject {
        public bool Alive = true;

        int Tries = 5;

        public Ball(int radius) : base(ProceduralTextures.CreateCircle(radius), Color.White, radius) {
            Alive = false;
        }

        public override void Update(GameTime gameTime) {
            //check for outer point of arc collisions and have a special interaction.
            //maybe change order of movement.
            //maybe check if the ball was colliding with the circle prior, then made a collision with the arc.
            //Maybe the arc should move between steps as well, then move the colliding objects each step of the way.
            if (Position.Y > Resolution.VirtualResolution.Y + Radius) {
                Tries--;

                if (Tries <= 0) {
                    Alive = false;
                    ParticleManager.AddEmitter(new FadingParticleEmitter(ProceduralTextures.CreateCircle(10, Color.Red), Position, 10, 5, 10));
                }
                else {
                    //SetVelocity(new Vector2(Velocity.X, -Velocity.Y));
                }
            }


            base.Update(gameTime);
        }

        protected override Movement NextMovement(GameTime gameTime) {
            return new BallMovement(this, Position, Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, Radius);
        }
    }
}
