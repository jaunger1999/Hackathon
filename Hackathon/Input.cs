using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Hackathon {
    public enum CheatInput { Up, Down, Left, Right, A, B }

    /// <summary>
    /// Processes all keyboard and gamepad inputs
    /// </summary>
    static class Input {
        #region Constants
        private static readonly float deadzone = .05f, //deadzones for the analog sticks
            crouchDeadzone = 0.25f,
            scrollWheelMin = .001f, 
            scrollWheelMax = 0.01f, 
            scrollWheelDivisor = 120000; //max and min scroll wheel values.
        #endregion

        #region Variables
        public static Point OldMousePosition { get; private set; }
        public static Point MousePosition { get; private set; }
        public static float RotChange { get; private set; }

        private static KeyboardState keyB, oldKeyB;
        private static GamePadState actualGameP, oldActualGameP, gameP, oldGameP;
        private static MouseState mouse, oldMouse;
        private static Vector2 actualResolution, mouseXOffset, mouseYOffset, resolution, screenResolution, virtualResolution;

        private static CheatInput cheatInput;
        private static Buttons grabButton = Buttons.RightShoulder, itemSlotOne = Buttons.DPadUp;

        private static bool fullscreen, usingController;
        private static float aspectRatio, screenAspectRatio, scrollWheel, rightTrigger, vibrationIntensity;
        private static int dirLength;
        #endregion

        #region Initialization
        static Input() {
            usingController = false;
            scrollWheel = scrollWheelMax;

            keyB = new KeyboardState();
            oldKeyB = keyB;

            gameP = new GamePadState();
            oldGameP = gameP;

            vibrationIntensity = 0;
        }

        public static void SetActualResolution(int width, int height) {
            actualResolution = new Vector2(width, height);
        }

        /// <summary>
        /// Sets resolution value.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetResolution(int width, int height) {
            resolution = new Vector2(width, height);
        }

        public static void SetScreenResolution(int width, int height) {
            screenResolution = new Vector2(width, height);

            screenAspectRatio = (float)width / height;
        }

        /// <summary>
        /// Sets virtual resolution value.
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="virtualResolution"></param>
        public static void SetVirtualResolution(int width, int height) {
            virtualResolution = new Vector2(width, height);
        }

        public static void SetAspectRatio(float aspectRatio) {
            Input.aspectRatio = aspectRatio;
        }

        public static void SetFullscreen(bool fullscreen) {
            Input.fullscreen = fullscreen;
        }

        public static void SetDirLength(int d) {
            dirLength = d;
        }
        #endregion

        #region Keyboard
        public static bool Save() {
            return (Pressing(Keys.LeftControl) || Pressing(Keys.RightControl)) && Pressed(Keys.S);
        }

        /// <summary>
        /// Toggles fullscreen.
        /// </summary>
        /// <returns></returns>
        public static bool Fullscreen() {
            return Pressed(Keys.F11);
        }

        /// <summary>
        /// Checks if a key was pressed once.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool ActuallyPressed(Keys key) {
            return ActuallyPressing(key) && oldKeyB.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if the user is pressing a key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool ActuallyPressing(Keys key) {
            bool keyPressing = keyB.IsKeyDown(key);

            return keyPressing;
        }

        /// <summary>
        /// Checks if a key was pressed once.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool Pressed(Keys key) {
            return Pressing(key) && oldKeyB.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if the user is pressing a key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool Pressing(Keys key) {
            bool keyPressing = keyB.IsKeyDown(key);

            //if we're pressing a key we are not using the controller.
            if (keyPressing) {
                NotUsingController();
            }

            return keyPressing;
        }

        /// <summary>
        /// Return true if key is up.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool NotHeld(Keys key) {
            return keyB.IsKeyUp(key);
        }
        #endregion

        #region Mouse
        private static bool LeftMousePressing() {
            bool pressing = mouse.LeftButton == ButtonState.Pressed;

            if (pressing) {
                NotUsingController();
            }

            return pressing;
        }

        public static bool LeftMousePressed() {
            return oldMouse.LeftButton == ButtonState.Released && LeftMousePressing();
        }

        private static bool MouseOnScreen() {
            bool onScreen = false;

            if (fullscreen) {
                onScreen = mouse.Position.X >= mouseXOffset.X && mouse.Position.Y >= 0 &&
                    mouse.Position.X < screenResolution.X - mouseXOffset.X && mouse.Position.Y < screenResolution.Y;
            }
            else {
                onScreen = mouse.Position.X >= 0 && mouse.Position.Y >= 0 &&
                    mouse.Position.X < resolution.X && mouse.Position.Y < resolution.Y;
            }

            return onScreen;
        }

        /// <summary>
        /// Returns how much the scroll wheel has changed this frame.
        /// </summary>
        /// <returns></returns>
        private static float ScrollWheelChange() {
            float difference = mouse.ScrollWheelValue - oldMouse.ScrollWheelValue;

            if (difference != 0) {
                NotUsingController();
            }

            return difference;
        }

        /// <summary>
        /// Updates the scroll wheel position.
        /// </summary>
        private static void UpdateScrollWheel() {
            scrollWheel += ScrollWheelChange() / scrollWheelDivisor;

            //keep it in bounds.
            scrollWheel = Math.Min(scrollWheel, scrollWheelMax);
            scrollWheel = Math.Max(scrollWheel, scrollWheelMin);
        }
        #endregion

        #region Bools
        public static bool Connected(PlayerIndex p) {
            return GamePad.GetState(p).IsConnected;
        }
        #endregion

        #region Controller Buttons
        /// <summary>
        /// Checks if a button was pressed once.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool ActuallyPressed(Buttons b) {
            return ActuallyPressing(b) && oldActualGameP.IsButtonUp(b);
        }

        /// <summary>
        /// Checks if a button is held down on the controller.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool ActuallyPressing(Buttons b) {
            bool pressing = actualGameP.IsButtonDown(b);

            return pressing;
        }

        /// <summary>
        /// Checks if a button was pressed once.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Pressed(Buttons b) {
            return Pressing(b) && oldGameP.IsButtonUp(b);
        }

        /// <summary>
        /// Checks if a button is held down on the controller.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Pressing(Buttons b) {
            bool pressing = gameP.IsButtonDown(b);

            if (pressing) {
                usingController = pressing;
            }

            return pressing;
        }

        private static bool Released(Buttons b) {
            return NotHeld(b) && oldGameP.IsButtonDown(b);
        }

        private static bool NotHeld(Buttons b) {
            return gameP.IsButtonUp(b);
        }
        #endregion

        #region Analog Input
        public static Vector2 Direction() {
            return new Vector2(LeftAnalogX(), LeftAnalogY());
        }

        /// <summary>
        /// Gives us constant access to trigger values.
        /// </summary>
        private static void UpdateTriggers() {
            UpdateRightTrigger();
        }

        /// <summary>
        /// Gives us constant access to the right trigger.
        /// </summary>
        private static void UpdateRightTrigger() {
            float trigger = gameP.Triggers.Right;

            if (trigger > 0) {
                usingController = true;
            }

            rightTrigger = trigger;
        }

        public static Vector2 LeftAnalog() {
            return new Vector2(LeftAnalogX(), LeftAnalogY());
        }

        public static Vector2 OldLeftAnalog() {
            return new Vector2(OldLeftAnalogX(), OldLeftAnalogY());
        }

        /// <summary>
        /// Returns the left analog stick's x position.
        /// </summary>
        /// <returns></returns>
        public static float LeftAnalogX() {
            float x = gameP.ThumbSticks.Left.X;

            if (Math.Abs(x) > deadzone) {
                usingController = true;
            }

            return x;
        }

        public static float OldLeftAnalogX() {
            return oldGameP.ThumbSticks.Left.X;
        }

        /// <summary>
        /// Returns the left analog stick's y position.
        /// </summary>
        /// <returns></returns>
        public static float LeftAnalogY() {
            //negative y axis to put put keep analog and screen coordinate systems consistent.
            float y = -gameP.ThumbSticks.Left.Y;

            if (Math.Abs(y) > deadzone) {
                usingController = true;
            }

            return y;
        }

        public static float OldLeftAnalogY() {
            return -oldGameP.ThumbSticks.Left.Y;
        }

        public static Vector2 RightAnalog() {
            return new Vector2(RightAnalogX(), RightAnalogY());
        }

        /// <summary>
        /// Returns the right analog stick's x position.
        /// </summary>
        /// <returns></returns>
        public static float RightAnalogX() {
            float x = gameP.ThumbSticks.Right.X;

            if (Math.Abs(x) > deadzone) {
                usingController = true;
            }

            return x;
        }

        /// <summary>
        /// Returns the right analog stick's y position.
        /// </summary>
        /// <returns></returns>
        public static float RightAnalogY() {
            //negative y axis to put put keep analog and screen coordinate systems consistent.
            float y = -gameP.ThumbSticks.Right.Y;

            if (Math.Abs(y) > deadzone) {
                usingController = true;
            }

            return y;
        }

        private static float LeftAnalogAngle() {
            float angle = (float)Math.Atan2(LeftAnalogY(), LeftAnalogX());

            if (angle < 0) {
                angle += MathHelper.TwoPi;
            }

            return angle;
        }

        private static float RightAnalogAngle() {
            float angle = (float)Math.Atan2(RightAnalogY(), RightAnalogX());

            if (angle < 0) {
                angle += MathHelper.TwoPi;
            }

            return angle;
        }

        private static float LeftAnalogLength() {
            return LeftAnalog().Length();
        }

        private static float OldLeftAnalogLength() {
            return OldLeftAnalog().Length();
        }

        private static float RightAnalogLength() {
            return RightAnalog().Length();
        }
        #endregion

        #region Volume
        /// <summary>
        /// When the plus key is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool VolumeUp() {
            return Pressed(Keys.Add);
        }

        /// <summary>
        /// When the plus key is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool VolumeDown() {
            return Pressed(Keys.Subtract);
        }

        /// <summary>
        /// When the plus key is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool Mute() {
            return Pressed(Keys.Back);
        }
        #endregion

        #region Debug Inputs
        /// <summary>
        /// Display info related to blinky.
        /// </summary>
        /// <returns></returns>
        public static bool BlinkyInfoPressed() {
            return Pressed(Keys.F1);
        }

        /// <summary>
        /// Display info related to pinky.
        /// </summary>
        /// <returns></returns>
        public static bool PinkyInfoPressed() {
            return Pressed(Keys.F2);
        }

        /// <summary>
        /// Display info related to inky.
        /// </summary>
        /// <returns></returns>
        public static bool InkyInfoPressed() {
            return Pressed(Keys.F3);
        }

        /// <summary>
        /// Display info related to clyde.
        /// </summary>
        /// <returns></returns>
        public static bool ClydeInfoPressed() {
            return Pressed(Keys.F4);
        }

        /// <summary>
        /// Display info related to ghostmanager
        /// </summary>
        /// <returns></returns>
        public static bool GhostInfoPressed() {
            return Pressed(Keys.F5);
        }

        /// <summary>
        /// Display info related to the player.
        /// </summary>
        /// <returns></returns>
        public static bool PlayerInfoPressed() {
            return Pressed(Keys.F6);
        }

        /// <summary>
        /// Display info related to the current level.
        /// </summary>
        /// <returns></returns>
        public static bool LevelInfoPressed() {
            return Pressed(Keys.F7);
        }

        /// <summary>
        /// Display the debug grid.
        /// </summary>
        /// <returns></returns>
        public static bool GridPressed() {
            return Pressed(Keys.F12);
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the keyB at the beginning of the update loop.
        /// </summary>
        public static void Update(GamePadState gP) {
            UpdateStates();

            UpdateMousePosition();
            UpdateScrollWheel();
            UpdateTriggers();
        }

        /// <summary>
        /// Updates the keyB at the beginning of the update loop.
        /// </summary>
        public static void Update() {
            

            UpdateStates();

            UpdateMousePosition();
            UpdateScrollWheel();

            RotChange = (mouse.X - oldMouse.X) * scrollWheel;

            UpdateTriggers();
        }

        /// <summary>
        /// Used for single presses.
        /// Meant to be called at the end of the main update loop.
        /// </summary>
        public static void OldUpdate() {
            oldActualGameP = actualGameP;

            oldKeyB = keyB;
            oldGameP = gameP;
            oldMouse = mouse;
        }

        /// <summary>
        /// Guarantees demos start with the same oldGameP state.
        /// </summary>
        public static void Reset() {
            oldGameP = GamePadState.Default;
        }

        /// <summary>
        /// If were using the controller and now we're not, reset scrollWheel to 1.
        /// </summary>
        private static void NotUsingController() {
            if (usingController) {
                scrollWheel = 1;
                usingController = false;
            }
        }

        private static void UpdateMousePosition() {
            Vector2 tempX, tempY;
            OldMousePosition = MousePosition;

            if (MouseOnScreen()) {
                if (fullscreen) {
                    mouseXOffset = new Vector2(actualResolution.X - resolution.X, 0) / 2 * screenResolution / actualResolution;
                    mouseYOffset = new Vector2(0, actualResolution.Y - resolution.Y) / 2 * screenResolution / actualResolution;

                    tempX = (mouse.Position.ToVector2() - mouseXOffset);

                    if (screenAspectRatio > aspectRatio) {
                        tempX = (mouse.Position.ToVector2() - mouseXOffset);

                        MousePosition = new Point((int)(tempX.X * screenAspectRatio / aspectRatio * virtualResolution.X / screenResolution.X),
                            (int)(tempX.Y * virtualResolution.Y / screenResolution.Y));
                    }
                }
                else {
                    MousePosition = (mouse.Position.ToVector2() * virtualResolution / resolution).ToPoint();
                }
            }

            MousePosition = new Point(Math.Max(0, Math.Min(MousePosition.X, (int)virtualResolution.X)), 
                Math.Max(0, Math.Min(MousePosition.Y, (int)virtualResolution.Y)));
        }

        private static void UpdateStates() {
            keyB = Keyboard.GetState();
            actualGameP = gameP = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
            mouse = Mouse.GetState();
        }
        #endregion

        #region Activated
        public static void Vibrate(float intensity) {
            vibrationIntensity = intensity;
        }
        #endregion

        #region Draw
        public static void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont) {
            spriteBatch.DrawString(spriteFont, "I", MousePosition.ToVector2(), Color.White);
        }
        #endregion
    }
}