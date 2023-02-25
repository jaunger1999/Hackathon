using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Hackathon {
    static class GameManager {
        public static List<Obstacle> Obstacles { get; private set; }

        private static Ball ball = new Ball(20);

        public static Obstacle HeldObstacle { get; private set; }

        public static void AddObstacle(Obstacle obstacle) {
            if (Obstacles == null) {
                Obstacles = new List<Obstacle>();
            }

            Obstacles.Add(obstacle);
        }

        public static bool Collision(GameObject obj) {
            return false;
        }

        public static void Update(GameTime gameTime) {
            ball.Update(gameTime);

            CheckClickedObstacle();
            UpdateObstacles(gameTime);
            OldUpdateObstacles();
        }

        private static void UpdateObstacles(GameTime gameTime) {
            for (int i = 0; i < Obstacles.Count; i++) {
                Obstacles[i].AddToRotation(Input.RotChange);
                Obstacles[i].Update(gameTime);
            }
        }

        private static void CheckClickedObstacle() {
            if (Input.LeftMousePressed()) {
                if (HeldObstacle == null) {
                    for (int i = 0; i < Obstacles.Count && HeldObstacle == null; i++) {
                        if (Obstacles[i].PointOnGrab(Input.MousePosition.ToVector2())) {
                            HeldObstacle = Obstacles[i];
                            HeldObstacle.GrabToggle();
                        }
                    }
                }
                else {
                    HeldObstacle.GrabToggle();
                    HeldObstacle = null;
                }
            }

            HeldObstacle?.SetPosition(Input.MousePosition.ToVector2());
        }

        private static void OldUpdateObstacles() {
            for (int i = 0; i < Obstacles.Count; i++) {
                Obstacles[i].OldUpdate();
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {
            ball.Draw(spriteBatch);

            foreach (Obstacle o in Obstacles) {
                o.Draw(spriteBatch);
            }
        }
    }
}
