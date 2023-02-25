using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprites {
    /// <summary>
    /// Anything non-text that's drawn on screen
    /// </summary>
    class Sprite {
        private const float ROTATION = 0, SCALE = 1;
        private const int ROW = 0, COLUMN = 1; //first value in Dimensions is ROW, next is COLUMN

        public Vector2 ScaledOrigin => scale * Origin;
        public int[] Dimensions { get; private set; } //rows and columns for each sprite
        public int RowI => ROW;
        public int ColumnI => COLUMN;
        public int Row => row;
        public int Column => column;
        public int Width => Source.Width;
        public int Height => Source.Height;

        /// <summary>
        /// The center of what is being drawn.
        /// </summary>
        private Vector2 Origin {
            get {
                //bottom center origin for growth while sitting on the ground.
                return sprite == null ? Vector2.Zero : new Vector2(Source.Width / 2, Source.Height);
            }
        }

        /// <summary>
        /// The part of the sprite that is currently being drawn.
        /// </summary>
        private Rectangle Source {
            get {
                return sprite == null ? Rectangle.Empty : new Rectangle(column * sprite.Width / Dimensions[COLUMN],
                    row * sprite.Height / Dimensions[ROW],
                    sprite.Width / Dimensions[COLUMN],
                    sprite.Height / Dimensions[ROW]);
            }
        }

        private Animation[] animations;
        private Animation CurrAnimation => animations[row];

        private Texture2D sprite; //the image being drawn
        private Color color; //the tint of the sprite.

        public bool JustFinished { get; private set; }
        private int row, column; //animation variables
        private double timeSinceLastFrame;
        private float scale;

        /// <summary>
        /// This constructor assumes the longest animation in your spritesheet is the first one.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="animations"></param>
        public Sprite(Texture2D sprite, Animation[] animations) {
            int framesWide = 1;

            this.sprite = sprite;
            this.animations = animations;

            for (int i = 0; i < animations.Length; i++) {
                if (animations[i].Frames > framesWide) {
                    framesWide = animations[i].Frames;
                }
            }

            color = Color.White;
            row = 0;
            column = 0;
            timeSinceLastFrame = 0;
            Dimensions = new int[] { animations.Length, framesWide };
            scale = SCALE;
        }

        public Sprite(Texture2D sprite, Animation animation) {
            this.sprite = sprite;
            animations = new Animation[]{ animation };

            color = Color.White;
            row = 0;
            column = 0;
            timeSinceLastFrame = 0;
            Dimensions = new int[] { animations.Length, animations[0].Frames };
            scale = SCALE;
        }

        public Sprite(Texture2D sprite, int[] dimensions) {
            this.sprite = sprite;
            color = Color.White;
            row = 0;
            column = 0;
            timeSinceLastFrame = 0;

            Dimensions = dimensions;
            scale = SCALE;
        }

        public Sprite(Texture2D sprite) {
            this.sprite = sprite;
            color = Color.White;
            row = 0;
            column = 0;
            timeSinceLastFrame = 0;
            Dimensions = new int[] { 1, 1 };
            scale = SCALE;
        }

        /// <summary>
        /// Keep rows and columns in bounds.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update() {
            //keeps these values in bounds.
            row = row % Dimensions[ROW];
            column = column % Dimensions[COLUMN];
        }

        /// <summary>
        /// Give this sprite a new color.
        /// </summary>
        public void UpdateColor(Color c) {
            color = c;
        }

        /// <summary>
        /// Give this sprite a new scale.
        /// </summary>
        /// <param name="scale"></param>
        public void UpdateScale(float scale) {
            this.scale = scale;
        }

        /// <summary>
        /// Changes the animation.
        /// </summary>
        /// <param name="animationI"></param>
        public void SetAnimation(int animationI) {
            int oldRow = row;

            row = animationI % Dimensions[ROW];

            if (oldRow != row) {
                ResetAnimation();
            }
        }

        /// <summary>
        /// Changes the animation if we have a higher priority animation.
        /// </summary>
        /// <param name="animationI"></param>
        public void SetAnimationPriority(int animationI) {
            if (animationI < row) {
                SetAnimation(animationI);
            }
        }

        /// <summary>
        /// Brings you back to the start of an animation
        /// </summary>
        public void ResetAnimation() {
            JustFinished = false;
            column = 0;
        }

        /// <summary>
        /// Moves to the next frame in the animation every x seconds.
        /// Returns true when reaching the end of an animation.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="ticksPerSprite"></param>
        public bool Animate(GameTime gameTime) {
            double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;
            int oldColumn = column,
                framesToAnimate, nextFrame;

            JustFinished = false;

            //calculate how long it has been since the last frame advance
            //and compare it to how long it's supposed to take.
            timeSinceLastFrame += elapsedSeconds;
            framesToAnimate = (int)Math.Floor(timeSinceLastFrame / CurrAnimation.TimeBetweenFrames);

            if (framesToAnimate > 0) {
                nextFrame = column + framesToAnimate;

                column = nextFrame % CurrAnimation.Frames;
                JustFinished = (oldColumn <= CurrAnimation.Frames - 1) && (nextFrame >= CurrAnimation.Frames - 1);

                timeSinceLastFrame -= framesToAnimate * CurrAnimation.TimeBetweenFrames;
            }

            return JustFinished;
        }

        /// <summary>
        /// Moves to the next frame in the animation every x seconds.
        /// Returns true when reaching the end of an animation.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="ticksPerSprite"></param>
        public bool Animate(GameTime gameTime, float speedPercent) {
            double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds * speedPercent;
            int oldColumn = column,
                framesToAnimate, nextFrame;

            JustFinished = false;

            //calculate how long it has been since the last frame advance
            //and compare it to how long it's supposed to take.
            timeSinceLastFrame += elapsedSeconds;
            framesToAnimate = (int)Math.Floor(timeSinceLastFrame / CurrAnimation.TimeBetweenFrames);

            if (framesToAnimate > 0) {
                nextFrame = column + framesToAnimate;

                column = nextFrame % CurrAnimation.Frames;
                JustFinished = (oldColumn <= CurrAnimation.Frames - 1) && (nextFrame >= CurrAnimation.Frames - 1);

                timeSinceLastFrame -= framesToAnimate * CurrAnimation.TimeBetweenFrames;
            }

            return JustFinished;
        }

        /// <summary>
        /// Move to the next frame and return true if the animation has finished.
        /// </summary>
        /// <returns></returns>
        public bool NextFrame() {
            JustFinished = false;

            column = (column + 1) % CurrAnimation.Frames;
            JustFinished = column == CurrAnimation.Frames - 1;

            return JustFinished;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, Color? c = null, float alpha = 1, float rotation = ROTATION, SpriteEffects spriteEffect = SpriteEffects.None, float depth = 0) {
            Color color = c == null ? Color.White : (Color)c;

            spriteBatch.Draw(sprite, pos, Source, color * alpha, rotation, Origin, scale, spriteEffect, depth);
        }
    }
}