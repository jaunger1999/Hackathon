using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Hackathon {
    /// <summary>
    /// A set of methods useful for generating graphics on the fly.
    /// </summary>
    static class ProceduralTextures {
        private static GraphicsDevice device;

        public static void SetGraphicsDevice(GraphicsDevice device) {
            ProceduralTextures.device = device;
        }

        public static Texture2D CreateTexture(int width, int height, Func<int, Color> paint) {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Length; pixel++) {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }

        public static Texture2D CreateCircle(int radius, Color color) {
            int diameter = radius * 2;
            int rSquared = radius * radius;
            Texture2D texture = new Texture2D(device, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];


            for (int x = 0; x < diameter; x++) {
                for (int y = 0; y < diameter; y++) {
                    int index = x + diameter * y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.LengthSquared() <= rSquared) { //length squared saves a square root operation.
                        colorData[index] = color;
                    }
                    else {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        public static Texture2D CreateCircle(int radius) {
            int diameter = radius * 2;
            int rSquared = radius * radius;
            Texture2D texture = new Texture2D(device, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];


            for (int x = 0; x < diameter; x++) {
                for (int y = 0; y < diameter; y++) {
                    int index = x + diameter * y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.LengthSquared() <= rSquared) { //length squared saves a square root operation.
                        colorData[index] = Color.White;
                    }
                    else {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        public static Texture2D CreateArc(int radius, float radians, float thickness) {
            int diameter = radius * 2, rSquared = radius * radius;
            float innerRadius = radius - thickness, innerRadiusSquared = innerRadius * innerRadius;
            Texture2D texture = new Texture2D(device, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];


            for (int x = 0; x < diameter; x++) {
                for (int y = 0; y < diameter; y++) {
                    int index = x + diameter * y;
                    float adj = x - radius, opp = y - radius, angle = ((float)Math.Atan2(opp, adj) + MathHelper.TwoPi) % MathHelper.TwoPi;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    float posLengthSquared = pos.LengthSquared();
                    Console.WriteLine(Math.Atan2(opp, adj));

                    if (posLengthSquared <= rSquared && posLengthSquared > innerRadiusSquared && Math.Abs(angle) < radians) { //length squared saves a square root operation.
                        colorData[index] = Color.White;
                    }
                    else {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
