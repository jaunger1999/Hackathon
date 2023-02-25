using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Hackathon {
    static class GameManager {
        public static List<Obstacle> Obstacles { get; private set; }

        private static Ball ball = new Ball(20);

        private static Obstacle heldObstacle;

        public static void AddObstacle(Obstacle obstacle) {
            if (Obstacles == null) {
                Obstacles = new List<Obstacle>();
            }

            Obstacles.Add(obstacle);
        }

        public static void Update(GameTime gameTime) {
            ball.Update(gameTime);

            UpdateObstacles(gameTime);
            CheckClickedObstacle();
        }

        private static void UpdateObstacles(GameTime gameTime) {
            for (int i = 0; i < Obstacles.Count; i++) {
                Obstacles[i].AddToRotation(Input.RotChange);
                Obstacles[i].Update(gameTime);
            }
        }

        private static void CheckClickedObstacle() {
            if (Input.LeftMousePressed()) {
                if (heldObstacle == null) {
                    for (int i = 0; i < Obstacles.Count && heldObstacle == null; i++) {
                        if (Obstacles[i].PointOnGrab(Input.MousePosition.ToVector2())) {
                            heldObstacle = Obstacles[i];
                            heldObstacle.GrabToggle();
                        }
                    }
                }
                else {
                    heldObstacle.GrabToggle();
                    heldObstacle = null;
                }
            }

            heldObstacle?.SetPosition(Input.MousePosition.ToVector2());
        }

        public static void Draw(SpriteBatch spriteBatch) {
            ball.Draw(spriteBatch);

            foreach (Obstacle o in Obstacles) {
                o.Draw(spriteBatch);
            }
        }
    }
}
