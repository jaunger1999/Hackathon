using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class GameObject {
        private Color color;
        private Vector2 position;

        private Texture2D texture;

        private float radius;

        public GameObject(Texture2D texture, Color color, int radius) {
            this.texture = texture;
            this.radius = radius;

            position = new Vector2(120, 120);
        }

        public virtual void Update(GameTime gameTime) {

        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
