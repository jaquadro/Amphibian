using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Components;
using Amphibian.Geometry;
using Amphibian.Collision;

namespace Amphibian.Systems
{
    public class PlatformPhysicsSystem : BaseSystem
    {
        private BackgroundCollisionSystem _backCollisionSystem;

        public PlatformPhysicsSystem ()
            : base(typeof(PlatformPhysics))
        {
        }

        protected internal override void Initialize ()
        {
            _backCollisionSystem = SystemManager.GetSystem(typeof(BackgroundCollisionSystem)) as BackgroundCollisionSystem;

            SystemManager.SystemAdded += SystemManager_SystemAdded;
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities) {
                Process(entity);
            }
        }

        private void Process (Entity entity)
        {
            PlatformPhysics physicsCom = null;
            Collidable collisionCom = null;
            Position positionCom = null;

            foreach (IComponent com in EntityManager.GetComponents(entity)) {
                if (com is PlatformPhysics)
                    physicsCom = com as PlatformPhysics;
                if (com is Collidable)
                    collisionCom = com as Collidable;
                if (com is Position)
                    positionCom = com as Position;
            }

            if (physicsCom == null || collisionCom == null || positionCom == null)
                return;

            float time = (float)SystemManager.World.GameTime.ElapsedGameTime.TotalSeconds * 60f;

            FPInt diffPosX = StepXPhysics(time, physicsCom);
            FPInt diffPosY = StepYPhysics(time, physicsCom);

            MoveVertical(diffPosX, diffPosY, positionCom, collisionCom, physicsCom);
        }

        private FPInt StepXPhysics (float time, PlatformPhysics physics)
        {
            FPInt diffVelX = 0;

            switch (physics.AccelStateX) {
                case PlatformAccelStateC.Accelerate:
                    diffVelX = (FPInt)((float)physics.AccelX * time);
                    break;
                case PlatformAccelStateC.Decelerate:
                    diffVelX = (FPInt)((float)physics.DecelX * time);
                    if (physics.VelocityY > 0) {
                        diffVelX = -FPMath.Min(physics.VelocityX, diffVelX);
                    }
                    else {
                        diffVelX = -FPMath.Max(physics.VelocityX, -diffVelX);
                    }
                    break;
            }

            FPInt nextVelX = FPMath.Clamp(physics.VelocityX + diffVelX, physics.MinVelocityX, physics.MaxVelocityX);
            FPInt diffPosX = (FPInt)((float)((physics.VelocityX + nextVelX) >> 1) * time);

            physics.VelocityX = nextVelX;
            return diffPosX;
        }

        private FPInt StepYPhysics (float time, PlatformPhysics physics)
        {
            FPInt diffVelY = 0;

            switch (physics.AccelStateY) {
                case PlatformAccelStateC.Accelerate:
                    diffVelY = (FPInt)((float)physics.AccelY * time);
                    break;
                case PlatformAccelStateC.Decelerate:
                    diffVelY = (FPInt)((float)physics.DecelY * time);
                    if (physics.VelocityY > 0) {
                        diffVelY = -FPMath.Min(physics.VelocityY, diffVelY);
                    }
                    else {
                        diffVelY = -FPMath.Max(physics.VelocityY, -diffVelY);
                    }
                    break;
            }

            FPInt nextVelY = FPMath.Clamp(physics.VelocityY + diffVelY, physics.MinVelocityY, physics.MaxVelocityY);
            FPInt diffPosY = (FPInt)((float)((physics.VelocityY + nextVelY) >> 1) * time);

            physics.VelocityY = nextVelY;
            return diffPosY;
        }

        private AXLine OffsetDetector (AXLine line, FPInt offX, FPInt offY)
        {
            return new AXLine(new PointFP(line.Left + offX, line.Y + offY), line.Width);
        }

        private AXLine LowerDetector (RectangleFP bounds)
        {
            return new AXLine(new PointFP(bounds.Left + 1, bounds.Bottom), bounds.Width - 2);
        }

        private AXLine UpperDetector (RectangleFP bounds)
        {
            return new AXLine(new PointFP(bounds.Left + 1, bounds.Top), bounds.Width - 2);
        }

        private void MoveVertical (FPInt distX, FPInt distY, Position position, Collidable collidable, PlatformPhysics physics)
        {
            FPInt posY = position.Y + distY;
            collidable.Mask.Position.X = position.X;
            collidable.Mask.Position.Y = posY;

            if (distY > 0) {
                AXLine low = LowerDetector(collidable.Mask.Bounds);

                if (_backCollisionSystem.TestEdge(low)) {
                    physics.VelocityY = 0;

                    posY = (FPInt)posY.Floor;
                    while (_backCollisionSystem.TestEdge(low)) {
                        posY -= 1;
                        low = OffsetDetector(low, 0, -1);
                    }
                    posY += 1;

                    /*bool testLL = _object.Parent.TestBackdropEdge(_object.X + _detLowLeft, _detLow.Position.Y);
                    bool testLR = _object.Parent.TestBackdropEdge(_object.X + _detLowRight, _detLow.Position.Y);

                    if (testLL && testLR) {
                        // Done for now
                    }
                    else if (testLL) {
                        if (_object.Parent.TestBackdropEdge(_object.X + _detLowLeft + 5, _detLow.Position.Y + 5)) {
                            _yVelocity = 10;
                        }
                    }*/
                }
            }
            else if (distY < 0) {
                AXLine high = UpperDetector(collidable.Mask.Bounds);

                if (_backCollisionSystem.TestEdge(high)) {
                    physics.VelocityY = 0;

                    posY = (FPInt)posY.Floor;
                    while (_backCollisionSystem.TestEdge(high)) {
                        posY += 1;
                        high = OffsetDetector(high, 0, 1);
                    }
                }
            }

            position.Y = posY;
            collidable.Mask.Position.Y = posY;
        }

        private void SystemManager_SystemAdded (BaseSystem system)
        {
            if (system is BackgroundCollisionSystem) {
                _backCollisionSystem = system as BackgroundCollisionSystem;
            }
        }
    }
}
