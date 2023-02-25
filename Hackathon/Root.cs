using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Particles;

namespace Hackathon {
    public class Root : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Obstacle obstacle;
        private Ball ball;
        Texture2D t;

        public Root() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            ProceduralTextures.SetGraphicsDevice(_graphics.GraphicsDevice);
            ball = new Ball(20);
            obstacle = new Obstacle(100, 5 * MathHelper.Pi / 4, 5.7f);
            t = ProceduralTextures.CreateArc(100, 5 * MathHelper.Pi / 3, 5.7f);
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
            ball.Draw(_spriteBatch);
            obstacle.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
