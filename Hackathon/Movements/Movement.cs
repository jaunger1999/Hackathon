using Microsoft.Xna.Framework;
using System;

namespace Hackathon {
    /// <summary>
    /// Checks for and returns the furthest valid movement in a given level by 
    /// breaking it down step by step.
    /// </summary>
    abstract class Movement {

        public bool Hit { get; private set; }
        public bool HitX { get; private set; }
        public bool HitY { get; private set; }

        protected Vector2 Velocity { get; private set; }
        public Vector2 RemainingMovement { get; private set; }

        private Vector2 oneStep, nextStep, nextIncrement, startPosition, endPosition;
        
        protected Vector2 furthestPosition { get; private set; }

        private int steps;
        private bool calculated, diagonal, canMoveAlongObjects;

        public Movement(Vector2 start, Vector2 end, float some, bool canMoveAlongObjects = true) {
            this.canMoveAlongObjects = canMoveAlongObjects;

            startPosition = start;
            endPosition = end;
            Velocity = RemainingMovement = end - start;

            furthestPosition = start;
            CalculateSteps();
            diagonal = Velocity.X != 0 && Velocity.Y != 0;
            Hit = HitX = HitY = calculated = false;
        }

        public Movement(Vector2 position, Vector2 velocity, bool canMoveAlongObjects = true) {
            this.canMoveAlongObjects = canMoveAlongObjects;
            Velocity = RemainingMovement = velocity;

            startPosition = position;
            endPosition = position + velocity;

            furthestPosition = startPosition;
            CalculateSteps();
            diagonal = velocity.X != 0 && velocity.Y != 0;
            Hit = HitX = HitY = calculated = false;
        }

        /// <summary>
        /// Calculate how many steps to break this movement into.
        /// </summary>
        private void CalculateSteps() {
            steps = (int)(endPosition - startPosition).Length() + 1;
            oneStep = (endPosition - startPosition) / steps;
        }

        /// <summary>
        /// Returns the furthest available position
        /// relative to the desired movement.
        /// </summary>
        /// <returns></returns>
        public Vector2 FurthestAvailablePosition() {
            if (!calculated) {
                CalculateFurthestAvailablePosition();
                calculated = true;
            }

            return furthestPosition;
        }

        /// <summary>
        /// Return true if we collide with something.
        /// </summary>
        /// <param name="newHS"></param>
        /// <returns></returns>
        protected abstract bool Collision(Vector2 position);

        /// <summary>
        /// Check which type of secondary movement to calculate between
        /// slope or separate axes.
        /// </summary>
        /// <returns></returns>
        private Vector2 ValidSecondaryMovement() {
            Vector2 v;

            v = ValidNonDiagonalMovement();

            return v;
        }

        /// <summary>
        /// Return a Vector2 that move along one axis.
        /// This is used if the step taken along both axes resulted
        /// in a collision.
        /// </summary>
        /// <returns></returns>
        private Vector2 ValidNonDiagonalMovement() {
            Vector2 xInc = new Vector2(oneStep.X, 0),
                yInc = new Vector2(0, oneStep.Y),
                nextXStep = furthestPosition + xInc,
                nextYStep = furthestPosition + yInc,
                localNextStep = furthestPosition;

            bool xCollision = CollisionAtNextStep(ref nextXStep),
                yCollision = CollisionAtNextStep(ref nextYStep);

            HitX = Velocity.X != 0 && (HitX || xCollision);
            HitY = Velocity.Y != 0 && (HitY || yCollision);

            return localNextStep;
        }

        /// <summary>
        /// Returns true if the hitbox at nextStep hits a wall
        /// or GameObject.
        /// </summary>
        /// <param name="nextStep"></param>
        /// <returns></returns>
        private bool CollisionAtNextStep(ref Vector2 nextStep) {
            //trialHS = hS.CloneAt(nextStep);
            bool collision = Collision(nextStep);

            if (!collision) {
                //trialHS = hS.CloneAt(nextStep);
                //collision = Collision(trialHS);
            }

            Hit = Hit || collision;

            if (!diagonal) {
                HitX = Velocity.X != 0 && (HitX || collision);
                HitY = Velocity.Y != 0 && (HitY || collision);
            }

            return collision;
        }

        /// <summary>
        /// Go as far as possible without bumping into a wall.
        /// Check one step at a time, slowing inching closer.
        /// </summary>
        private void CalculateFurthestAvailablePosition() {
            bool hitSomething = false, performSecondaryCheck = canMoveAlongObjects && diagonal;

            //if (hitSomething and !performSeparateAxisCheck) we didn't move so no more checks needed.
            for (int i = 0; i < steps && (!hitSomething || performSecondaryCheck); i++) {
                //PrevHS = newHS;

                if (!hitSomething) {
                    nextStep = furthestPosition + oneStep;
                    nextIncrement = oneStep;
                    hitSomething = CollisionAtNextStep(ref nextStep);

                    if (!hitSomething) {
                        //newHS = trialHS;
                        RemainingMovement -= nextIncrement;
                        furthestPosition = nextStep;
                    }
                }

                if (hitSomething && performSecondaryCheck) {
                    nextStep = ValidSecondaryMovement();

                    //If we didn't move there's no need to keep checking.
                    performSecondaryCheck = furthestPosition != nextStep;

                    if (performSecondaryCheck) {
                        //newHS = trialHS;
                        RemainingMovement -= nextIncrement;
                        furthestPosition = nextStep;
                    }
                }
            }
        }
    }
}