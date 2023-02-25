using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon {
    class GameObject {
        private Color color;
        private Vector2 position;
        private Vector2 velocity;

        private Texture2D texture;

        public GameObject() {

        }

        public virtual void Update(GameTime gameTime) {

        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
