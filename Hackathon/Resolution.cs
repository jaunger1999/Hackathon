//////////////////////////////////////////////////////////////////////////
////License:  The MIT License (MIT)
////Copyright (c) 2010 David Amador (http://www.david-amador.com)
////
////Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
////
////The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
////
////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace Hackathon {
    static class Resolution {
        //private const int RES_BASE_Y = 240, TILE_SIZE = 16;
        private const int RES_BASE_Y = 1080, TILE_SIZE = 16;

        public static Vector2 VirtualResolution { get; private set; }
        public static Vector2 WindowResolution => newViewportDims.ToVector2();

        public static int DefaultWidth { get; private set; }
        public static int DefaultHeight { get; private set; }
        public static int DefaultTileWidth { get; private set; }
        public static int DefaultTileHeight { get; private set; }

        private static GraphicsDeviceManager device;
        private static Matrix scaleMatrix;
        private static Point newViewportDims, screenResolution;

        private static int width, height, vWidth, vHeight;
        private static float aspectRatio, screenAspectRatio, virtualAspectRatio;
        
        private static bool dirtyMatrix, fullscreen, vSync;

        public static void ToggleFullscreen() {
            fullscreen = !fullscreen;
            CalculateNewViewport();

            if (!fullscreen) { //prevents weird scaling.
                CalculateNewViewport();
            }
        }

        static Resolution() {
            newViewportDims = screenResolution = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            aspectRatio = screenAspectRatio = (float)screenResolution.X / screenResolution.Y;
        }

        public static void Init(ref GraphicsDeviceManager device) {
            Resolution.device = device;
            width = device.PreferredBackBufferWidth;
            height = device.PreferredBackBufferHeight;
            
            dirtyMatrix = true;
            
            CalculateNewViewport();

            DefaultWidth = vWidth;
            DefaultHeight = vHeight;
            DefaultTileWidth = vWidth / TILE_SIZE;
            DefaultTileHeight = vHeight / TILE_SIZE;

            ToggleFullscreen(); //lazy
        }

        public static Matrix GetTransformationMatrix() {
            if (dirtyMatrix) {
                RecreateScaleMatrix();
            }

            return scaleMatrix;
        }

        private static void RecreateScaleMatrix() {
            dirtyMatrix = false;
            scaleMatrix = Matrix.CreateScale(
                           (float)device.GraphicsDevice.Viewport.Width / vWidth,
                           (float)device.GraphicsDevice.Viewport.Width / vWidth,
                           1f);
        }

        public static void SetResolution(int width, int height, bool fullscreen) {
            Resolution.width = width;
            Resolution.height = height;

            Resolution.fullscreen = fullscreen;
            
            aspectRatio = (float)width / height;
            
            //Input.SetActualResolution(width, height);
            ApplyResolutionSettings();
        }

        public static void SetVirtualResolution(int width, int height) {
            vWidth = width;
            vHeight = height;

            VirtualResolution = new Vector2(vWidth, vHeight);
            virtualAspectRatio = (float)vWidth / vHeight;

            dirtyMatrix = true;

            SetGameDims();
        }

        private static void ApplyResolutionSettings() {
            bool wasFullscreen = device.IsFullScreen;
            #if XBOX360
               _FullScreen = true;
            #endif

            if ((width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)) {
                device.PreferredBackBufferWidth = width;
                device.PreferredBackBufferHeight = height;

                //go to windowed mode and then fullscreen
                //to prevent weird resolution scaling.
                device.IsFullScreen = false;
                device.ApplyChanges();

                if (fullscreen) {
                    device.IsFullScreen = true;
                    device.ApplyChanges();
                }
            }

            dirtyMatrix = true;

            if (wasFullscreen && !fullscreen) { //prevents weirds scaling.
                CalculateNewViewport();
            }
        }

        /// <summary>
        /// Gives a bunch of classes a reference to this game's tile size and width/height.
        /// </summary>
        private static void SetGameDims() {

        }

        /// <summary>
        /// Sets the device to use the draw pump.
        /// Sets correct aspect ratio.
        /// </summary>
        public static void BeginDraw() {
            // Start by reseting viewport to (0,0,1,1)
            FullViewport();
            // Clear to Black
            device.GraphicsDevice.Clear(Color.Black);
            // Calculate Proper Viewport according to Aspect Ratio
            ResetViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
            device.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        private static void SetNewViewportDims(int width, int height) {
            newViewportDims = new Point(width, height);
            //Input.SetAspectRatio((float)newViewportDims.X / newViewportDims.Y);
            //Input.SetResolution(newViewportDims.X, newViewportDims.Y);
        }

        private static void CalculateNewViewport() {
            int newVWidth = (int)(RES_BASE_Y * (float)newViewportDims.X / newViewportDims.Y);

            if (newVWidth != vWidth) {
                SetVirtualResolution(newVWidth, RES_BASE_Y);
            }
            
            if (fullscreen) { //calculate dimensions that don't stretch the viewport.
                CalculateFullscreenViewportDims();
                //Input.SetFullscreen(true);
            }
            else {
                SetResolution(newViewportDims.X, newViewportDims.Y, false);
                //Input.SetFullscreen(false);
            }
        }

        private static void CalculateFullscreenViewportDims() {
            if (virtualAspectRatio >= screenAspectRatio) {
                SetResolution(newViewportDims.X, (int)Math.Round(newViewportDims.X * (float)screenResolution.Y / screenResolution.X), true);
            }
            else {
                SetResolution((int)Math.Round(newViewportDims.Y * (float)screenResolution.X / screenResolution.Y), newViewportDims.Y, true);
            }
        }

        private static void FullViewport() {
            Viewport vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = width;
            vp.Height = height;
            device.GraphicsDevice.Viewport = vp;
        }

        private static void ResetViewport() {
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            int width = device.PreferredBackBufferWidth;
            int height = (int)(width / virtualAspectRatio + .5f);
            bool changed = false;
            
            if (height > device.PreferredBackBufferHeight) {
                height = device.PreferredBackBufferHeight;
                // PillarBox
                width = (int)(height * virtualAspectRatio + .5f);
                changed = true;
            }

            // set up the new viewport centered in the backbuffer
            Viewport viewport = new Viewport {
                X = (device.PreferredBackBufferWidth / 2) - (width / 2),
                Y = (device.PreferredBackBufferHeight / 2) - (height / 2),
                Width = width,
                Height = height,
                MinDepth = 0,
                MaxDepth = 1
            };

            if (changed) {
                dirtyMatrix = true;
            }

            device.GraphicsDevice.Viewport = viewport;
        }

        private static void SetVSync(bool vSync) {
            Resolution.vSync = vSync;

            device.SynchronizeWithVerticalRetrace = vSync;
        }
    }
}