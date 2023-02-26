using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Particles;

namespace Hackathon {
    public class Root : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        public Root() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            ProceduralTextures.SetGraphicsDevice(_graphics.GraphicsDevice);
            GameManager.AddArcObstacle(new ArcObstacle(100, MathHelper.Pi, 5.7f), new Vector2(200, 600));
            GameManager.AddCircleObstacle(new CircleObstacle(30, Color.Blue), new Vector2(500, 500));

            GameManager.AddArcObstacle(new ArcObstacle(100, MathHelper.Pi, 5.7f), new Vector2(700, 600));
            GameManager.AddCircleObstacle(new CircleObstacle(30, Color.Blue), new Vector2(1000, 500));
            GameManager.ball.SetPosition(new Vector2(710, 50));

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Resolution.Init(ref _graphics);
            _spriteFont = Content.Load<SpriteFont>("File");
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            Emitter.UpdateStaticCounter(gameTime);
            ParticleManager.Update(gameTime);
            Input.Update();
            GameManager.Update(gameTime);
            Input.OldUpdate();
            ParticleManager.Cleanup();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            Matrix resMatrix;

            Resolution.BeginDraw();
            resMatrix = Resolution.GetTransformationMatrix();

            GraphicsDevice.Clear(Color.Gray);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.LinearClamp,
                transformMatrix: resMatrix * Camera.ViewMatrix);

            ParticleManager.Draw(_spriteBatch);
            GameManager.Draw(_spriteBatch, _spriteFont);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
