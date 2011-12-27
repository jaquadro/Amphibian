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
    public class PlatformPhysicsSystem : ProcessingSystem
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

            MoveHorizontal(diffPosX, diffPosY, positionCom, collisionCom, physicsCom);
            MoveVertical(diffPosX, diffPosY, positionCom, collisionCom, physicsCom);
        }

        private FPInt StepXPhysics (float time, PlatformPhysics physics)
        {
            FPInt diffVelX = 0;

            switch (physics.AccelStateX) {
                case PlatformAccelState.Accelerate:
                    diffVelX = (FPInt)((float)physics.AccelX * time);
                    break;
                case PlatformAccelState.Decelerate:
                    diffVelX = (FPInt)((float)physics.DecelX * time);
                    if (physics.VelocityX > 0) {
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
                case PlatformAccelState.Accelerate:
                    diffVelY = (FPInt)((float)physics.AccelY * time);
                    break;
                case PlatformAccelState.Decelerate:
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

        private AYLine OffsetDetector (AYLine line, FPInt offX, FPInt offY)
        {
            return new AYLine(new PointFP(line.X + offX, line.Top + offY), line.Height);
        }

        private AXLine LowerDetector (RectangleFP bounds)
        {
            return new AXLine(new PointFP(bounds.Left + 1, bounds.Bottom), bounds.Width - 2);
        }

        private AXLine UpperDetector (RectangleFP bounds)
        {
            return new AXLine(new PointFP(bounds.Left + 1, bounds.Top), bounds.Width - 2);
        }

        private static FPInt _maxSlope = 2;

        private AYLine LeftDetector (RectangleFP bounds)
        {
            return new AYLine(new PointFP(bounds.Left, bounds.Top + _maxSlope), bounds.Height - (4 * _maxSlope));
        }

        private AYLine RightDetector (RectangleFP bounds)
        {
            return new AYLine(new PointFP(bounds.Right, bounds.Top + _maxSlope), bounds.Height - (4 * _maxSlope));
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

        private void MoveHorizontal (FPInt distX, FPInt distY, Position position, Collidable collidable, PlatformPhysics physics)
        {
            FPInt posX = position.X;
            FPInt posY = position.Y;
            collidable.Mask.Position.X = posX;
            collidable.Mask.Position.Y = position.Y;

            FPInt offX = 0;

            RectangleFP bounds = collidable.Mask.Bounds;

            if (distX > 0) {
                offX = distX.Ceil - distX;
                int offY = 0;

                posX -= offX;
                distX = (FPInt)distX.Ceil;

                AYLine right = OffsetDetector(RightDetector(bounds), -offX, 0);

                for (; distX > 0; distX -= 1) {
                    posX += 1;
                    right = OffsetDetector(right, 1, 0);

                    if (_backCollisionSystem.Test(right)) {
                        physics.VelocityX = 0;

                        posX = (FPInt)posX.Floor;
                        right = new AYLine(new PointFP((FPInt)right.X.Floor, right.Top), right.Height);
                        if (_backCollisionSystem.Test(right)) {
                            posX -= 1;
                            right = OffsetDetector(right, -1, 0);
                        }
                    }

                    bool test1 = _backCollisionSystem.TestEdge(right.X - bounds.Width, bounds.Bottom + offY);
                    bool test2 = _backCollisionSystem.TestEdge(right.X - bounds.Width, bounds.Bottom + offY + 1);

                    if (!test1 && test2) {
                        posY += 1;
                        offY += 1;
                    }
                }
            }
            else if (distX < 0) {
                offX = distX - distX.Floor;
                int offY = 0;

                posX += offX;
                distX = (FPInt)distX.Floor;

                AYLine left = OffsetDetector(LeftDetector(bounds), offX, 0);

                for (; distX < 0; distX += 1) {
                    posX -= 1;
                    left = OffsetDetector(left, -1, 0);

                    if (_backCollisionSystem.Test(left)) {
                        physics.VelocityX = 0;

                        posX = (FPInt)posX.Ceil;
                        left = new AYLine(new PointFP((FPInt)left.X.Ceil, left.Top), left.Height);
                        if (_backCollisionSystem.Test(left)) {
                            posX += 1;
                            left = OffsetDetector(left, 1, 0);
                        }
                    }

                    bool test1 = _backCollisionSystem.TestEdge(left.X + bounds.Width, bounds.Bottom + offY);
                    bool test2 = _backCollisionSystem.TestEdge(left.X + bounds.Width, bounds.Bottom + offY + 1);

                    if (!test1 && test2) {
                        posY += 1;
                        offY += 1;
                    }
                }
            }

            position.X = posX;
            position.Y = posY;
            collidable.Mask.Position.X = posX;
            collidable.Mask.Position.Y = posY;

            /*_object.X += dist;

            if (dist > 0) {
                if (_object.Parent.TestBackdrop(_detRight)) {
                    _xVelocity = 0;

                    _object.X = (FPInt)_object.X.Floor;
                    while (_object.Parent.TestBackdrop(_detRight)) {
                        _object.X -= 1;
                    }
                }
            }
            else if (dist < 0) {
                if (_object.Parent.TestBackdrop(_detLeft)) {
                    _xVelocity = 0;

                    _object.X = (FPInt)_object.X.Floor;
                    while (_object.Parent.TestBackdrop(_detLeft)) {
                        _object.X += 1;
                    }
                }
            }*/
        }

        private void SystemManager_SystemAdded (BaseSystem system)
        {
            if (system is BackgroundCollisionSystem) {
                _backCollisionSystem = system as BackgroundCollisionSystem;
            }
        }
    }
}
