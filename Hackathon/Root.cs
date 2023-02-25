using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Particles;

namespace Hackathon {
    public class Root : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Root() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            ProceduralTextures.SetGraphicsDevice(_graphics.GraphicsDevice);
            GameManager.AddObstacle(new Obstacle(100, MathHelper.Pi, 5.7f));
            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Resolution.Init(ref _graphics);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Input.Update();
            GameManager.Update(gameTime);
            Input.OldUpdate();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            Matrix resMatrix;

            Resolution.BeginDraw();
            resMatrix = Resolution.GetTransformationMatrix();

            GraphicsDevice.Clear(Color.Gray);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp,
                transformMatrix: resMatrix * Camera.ViewMatrix);

            ParticleManager.Draw(_spriteBatch);
            GameManager.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
