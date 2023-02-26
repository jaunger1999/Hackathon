using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    abstract class GameObject {
        public Vector2 OldPosition { get; private set; }
        public Vector2 Position { get; private set; }

        private Vector2 origin;
        private Texture2D texture;

        public float Rotation { get; private set; }
        public float OldRotation { get; private set; }
        public float Depth { get; private set; }
        public float Radius { get; private set; }
        public float RadiusSquared { get; private set; }

        private Color color;

        public GameObject(Texture2D texture, Color color, int radius) {
            this.texture = texture;
            this.Radius = radius;
            this.color = color;

            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            RadiusSquared = Radius * Radius;
            Depth = 0;
        }

        public virtual void Update(GameTime gameTime) {

        }

        public virtual void OldUpdate() {
            OldPosition = Position;
            OldRotation = Rotation;
        }

        public void AddToRotation(float delta) {
            Rotation += delta;
        }

        public void SetPosition(Vector2 position) {
            Position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, Position, null, color, Rotation, origin, 1, SpriteEffects.None, Depth);
        }
    }
}
