using Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Hackathon {
    /// <summary>
    /// Lets us move the scene around.
    /// </summary>
    static class Camera {
        #region Constants
        private const float APPROACH_FACTOR = 0.1f, TRAUMA_PER_SECOND = 0.5f, MIN_TRAUMA = 0, MAX_TRAUMA = 1, MAX_ANGLE = (float)Math.PI / 32;
        #endregion

        #region Variables
        public static Matrix ViewMatrix { get; private set; }

        public static Vector2 Position => virtualPosition;
        public static float Left => virtualPosition.X;
        public static float Right => virtualPosition.X + width;
        public static float Top => virtualPosition.Y;
        public static float Bottom => virtualPosition.Y + height;

        public static int LeftTile => (int)(Left / tileSize);
        public static int RightTile => (int)(Right / tileSize);

        private static Vector2 ShakePosition {
            get {
                return position + shakeOffset;
            }
        }
        
        private static SimplexNoiseGenerator angleGen, xOffsetGen, yOffsetGen;
        private static Vector2 oldPosition, position, toPosition, shakeOffset, toVirtualPosition, virtualPosition;
        private static Timer shakeTimer;
        private static float angle, oldAngle, xOffset, yOffset, trauma, maxShake;
        private static int height, width, numberOfShakes, shakeIntensity, tileSize;
        #endregion

        #region Constructor & Initialization
        static Camera() {
            Random r = new Random();

            ResetCamera();
            shakeTimer = new Timer(ResetShake);

            angleGen = new SimplexNoiseGenerator(r.Next());
            xOffsetGen = new SimplexNoiseGenerator(r.Next());
            yOffsetGen = new SimplexNoiseGenerator(r.Next());
        }

        /// <summary>
        /// Set the dimensions for the camera's use.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public static void SetGameDims(int width, int height, int tileSize) {
            Camera.width = width;
            Camera.height = height;
            Camera.tileSize = tileSize;

            maxShake = tileSize;

            ResetCamera();
        }

        /// <summary>
        /// Bring the camera back to its original position.
        /// </summary>
        private static void ResetCamera() {
            oldPosition = position = shakeOffset = Vector2.Zero;
            ViewMatrix = Matrix.Identity;
        }
        #endregion

        #region Effects
        /// <summary>
        /// Causes a screen shake of frames duration.
        /// </summary>
        public static void Shake(float seconds, int intensity, int shakes) {
            shakeTimer = new Timer(seconds, ResetShake, true);

            numberOfShakes = shakes;
            shakeIntensity = intensity;
        }

        /// <summary>
        /// We don't want the screen to be permanently shook,
        /// so reset the offset.
        /// </summary>
        private static void ResetShake() {
            shakeOffset = Vector2.Zero;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update all real time effects.
        /// </summary>
        public static void Update(GameTime gameTime) {
            virtualPosition += (toVirtualPosition - virtualPosition) * APPROACH_FACTOR;
            position = -virtualPosition * Resolution.WindowResolution / Resolution.VirtualResolution;

            UpdateScreenShake(gameTime);
            UpdateTransformationMatrix();
        }

        public static void UpdatePosition(Vector2 pos) {
            toVirtualPosition = pos;
            toPosition = -toVirtualPosition * Resolution.WindowResolution / Resolution.VirtualResolution;
        }

        public static void SetTrauma(float t) {
            trauma = Math.Max(trauma, Math.Min(MAX_TRAUMA, t));
        }

        private static void UpdateScreenShake(GameTime gameTime) {
            const float SPEED_FACTOR = 20;

            float shake = trauma * trauma,
                time = (float)gameTime.TotalGameTime.TotalSeconds;

            angle = MAX_ANGLE * shake * angleGen.Generate(SPEED_FACTOR * time);
            xOffset = maxShake * shake * xOffsetGen.Generate(SPEED_FACTOR * time);
            yOffset = maxShake * shake * yOffsetGen.Generate(SPEED_FACTOR * time);

            shakeOffset = new Vector2(xOffset, yOffset);

            trauma = Math.Max(MIN_TRAUMA, trauma - TRAUMA_PER_SECOND * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (Input.Connected(PlayerIndex.One)) {
                GamePad.SetVibration(PlayerIndex.One, shake, shake);
            }
        }

        private static void UpdateShake(GameTime gameTime) {
            const float INTERVAL = 2 * (float)Math.PI;
            float timeRatio = shakeTimer.SecondsPassed / shakeTimer.InitTime;

            if (!shakeTimer.Complete) {
                shakeOffset = new Vector2(shakeIntensity * (float)Math.Sin(INTERVAL * numberOfShakes * timeRatio), 0);
                shakeTimer.Update(gameTime);
            }
        }

        /// <summary>
        /// Update the translation matrix if the camera position has changed.
        /// </summary>
        private static void UpdateTransformationMatrix() {
            if (oldPosition != ShakePosition || angle != oldAngle) {
                ViewMatrix = Matrix.CreateTranslation(new Vector3(ShakePosition, 0)) * 
                Matrix.CreateTranslation(new Vector3(-Resolution.WindowResolution / 2, 0)) *
                Matrix.CreateRotationZ(angle) *
                Matrix.CreateTranslation(new Vector3(Resolution.WindowResolution / 2, 0));

                //translationMat = Matrix.CreateTranslation(new Vector3(ShakePosition, 0));
                oldPosition = ShakePosition;
                oldAngle = angle;
            }
        }
        #endregion
    }
}