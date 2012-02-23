using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Components;
using Amphibian.Geometry;
using Amphibian.Collision;
using Amphibian.Templates;
using Amphibian.Utility;

namespace Amphibian.Systems
{
    public class PhysicsSystem : ProcessingSystem
    {
        public PhysicsSystem ()
            : base(typeof(Physics))
        {
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities) {
                Process(entity);
            }
        }

        private void Process (Entity entity)
        {
            Physics physicsCom = null;
            Position positionCom = null;

            foreach (IComponent com in EntityManager.GetComponents(entity)) {
                if (com is Physics)
                    physicsCom = com as Physics;
                if (com is Position)
                    positionCom = com as Position;
            }

            if (physicsCom == null || positionCom == null)
                return;

            float time = (float)SystemManager.World.GameTime.ElapsedGameTime.TotalSeconds * 60f;

            FPInt diffPosX = StepXPhysics(time, physicsCom);
            FPInt diffPosY = StepYPhysics(time, physicsCom);

            positionCom.X += diffPosX;
            positionCom.Y += diffPosY;
        }

        private FPInt StepXPhysics (float time, Physics physics)
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

        private FPInt StepYPhysics (float time, Physics physics)
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
    }

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

            FPInt preX = positionCom.X;
            FPInt preY = positionCom.Y;
            FPInt diffPosX = StepXPhysics(time, physicsCom);
            FPInt diffPosY = StepYPhysics(time, physicsCom);

            MoveHorizontal(entity, diffPosX, diffPosY, positionCom, collisionCom, physicsCom);
            MoveVertical(entity, diffPosX, diffPosY, positionCom, collisionCom, physicsCom);

            UpdatePushableState(entity, physicsCom, diffPosX, diffPosY, positionCom.X - preX, positionCom.Y - preY);
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

        private UnorderedList<Entity> _testBuffer = new UnorderedList<Entity>();

        private bool TestSolidObject (Collidable collidable, AXLine line)
        {
            _testBuffer.Clear();

            foreach (Entity entity in EntityManager.GetEntities(typeof(PlatformPhysics))) {
                PlatformPhysics comPhysics = EntityManager.GetComponent(entity, typeof(PlatformPhysics)) as PlatformPhysics;
                if (comPhysics == null || comPhysics.State != PlatformState.Solid)
                    continue;

                Collidable comCollision = EntityManager.GetComponent(entity, typeof(Collidable)) as Collidable;
                if (comCollision == null || comCollision == collidable)
                    continue;

                if (comCollision.Mask.TestOverlapEdge(line)) {
                    _testBuffer.Add(entity);
                    return true;
                }
            }
            return false;
        }

        private bool TestCollisionEdge (Collidable collidable, AXLine line)
        {
            return _backCollisionSystem.TestEdge(line) || TestSolidObject(collidable, line);
        }

        private bool TestSolidObject (Collidable collidable, AYLine line)
        {
            _testBuffer.Clear();

            foreach (Entity entity in EntityManager.GetEntities(typeof(PlatformPhysics))) {
                PlatformPhysics comPhysics = EntityManager.GetComponent(entity, typeof(PlatformPhysics)) as PlatformPhysics;
                if (comPhysics == null || comPhysics.State != PlatformState.Solid)
                    continue;

                Collidable comCollision = EntityManager.GetComponent(entity, typeof(Collidable)) as Collidable;
                if (comCollision == null || comCollision == collidable)
                    continue;

                if (comCollision.Mask.TestOverlapEdge(line)) {
                    _testBuffer.Add(entity);
                    return true;
                }
            }
            return false;
        }

        private bool TestCollisionEdge (Collidable collidable, AYLine line)
        {
            return _backCollisionSystem.TestEdge(line) || TestSolidObject(collidable, line);
        }

        private void MoveVertical (Entity entity, FPInt distX, FPInt distY, Position position, Collidable collidable, PlatformPhysics physics)
        {
            FPInt posY = position.Y + distY;
            collidable.Mask.Position.X = position.X;
            collidable.Mask.Position.Y = posY;

            if (distY > 0) {
                AXLine low = LowerDetector(collidable.Mask.Bounds);

                if (TestCollisionEdge(collidable, low)) {
                    physics.VelocityY = 0;

                    posY = (FPInt)posY.Floor;
                    while (TestCollisionEdge(collidable, low)) {
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

                if (TestCollisionEdge(collidable, high)) {
                    physics.VelocityY = 0;

                    posY = (FPInt)posY.Floor;
                    while (TestCollisionEdge(collidable, high)) {
                        posY += 1;
                        high = OffsetDetector(high, 0, 1);
                    }
                }
            }

            position.Y = posY;
            collidable.Mask.Position.Y = posY;
        }

        private void MoveHorizontal (Entity entity, FPInt distX, FPInt distY, Position position, Collidable collidable, PlatformPhysics physics)
        {
            FPInt posX = position.X;
            FPInt posY = position.Y;
            collidable.Mask.Position.X = posX;
            collidable.Mask.Position.Y = position.Y;

            FPInt offX = 0;
            FPInt dirX = distX;
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

                    if (TestCollisionEdge(collidable, right)) {
                        physics.VelocityX = 0;

                        // Try Attach pusher to pushable

                        //DebugCue.Create(SystemManager.World.Frame.Engine, SystemManager, new PointFP(right.X, right.Top + (right.Height / 2)));
                        TryAttachPushable(entity, physics, PlatformPushDirection.Right);

                        posX = (FPInt)posX.Floor;
                        right = new AYLine(new PointFP((FPInt)right.X.Floor, right.Top), right.Height);
                        if (TestCollisionEdge(collidable, right)) {
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

                    if (TestCollisionEdge(collidable, left)) {
                        physics.VelocityX = 0;

                        TryAttachPushable(entity, physics, PlatformPushDirection.Left);

                        posX = (FPInt)posX.Ceil;
                        left = new AYLine(new PointFP((FPInt)left.X.Ceil, left.Top), left.Height);
                        if (TestCollisionEdge(collidable, left)) {
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

            //if (physics.Pusher != null && physics.Pusher.TargetEntity != Entity.None) {
            //    PlatformPhysics targetPP = EntityManager.GetComponent(physics.Pusher.TargetEntity, typeof(PlatformPhysics)) as PlatformPhysics;
            //    if (targetPP != null && targetPP.Pushable != null) {
            //        if (dirX > 0) {
            //            if (physics.Pusher.TargetDirection == PlatformPushDirection.Right)
            //                targetPP.VelocityX = (FPInt)targetPP.Pushable.PushSpeed;
            //            else {
            //                physics.Pusher.TargetEntity = Entity.None;
            //                targetPP.Pushable.SourceEntity = Entity.None;
            //            }
            //        }
            //        else if (dirX < 0) {
            //            if (physics.Pusher.TargetDirection == PlatformPushDirection.Left)
            //                targetPP.VelocityX = -(FPInt)targetPP.Pushable.PushSpeed;
            //            else {
            //                physics.Pusher.TargetEntity = Entity.None;
            //                targetPP.Pushable.SourceEntity = Entity.None;
            //            }
            //        }
            //    }
            //}

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

        // Pushable Subsystem

        private void TryAttachPushable (Entity entity, PlatformPhysics pp, PlatformPushDirection direction)
        {
            if (pp.Pusher != null && pp.Pusher.TargetEntity == Entity.None && _testBuffer.Count > 0) {
                Entity otherEntity = _testBuffer[0];
                PlatformPhysics otherPP = EntityManager.GetComponent(otherEntity, typeof(PlatformPhysics)) as PlatformPhysics;

                if (otherPP != null && otherPP.Pushable != null && otherPP.Pushable.SourceEntity == Entity.None) {
                    pp.Pusher.DelayExpended = 0;
                    pp.Pusher.TargetEntity = otherEntity;
                    pp.Pusher.TargetDirection = direction;
                    otherPP.Pushable.SourceEntity = entity;
                }
            }
        }

        private void UpdatePushableState (Entity entity, PlatformPhysics physics, FPInt reqDiffX, FPInt reqDiffY, FPInt actDiffX, FPInt actDiffY)
        {
            if (physics.Pusher != null && physics.Pusher.TargetEntity != Entity.None) {
                PlatformPhysics targetPP = EntityManager.GetComponent(physics.Pusher.TargetEntity, typeof(PlatformPhysics)) as PlatformPhysics;
                if (targetPP != null && targetPP.Pushable != null) {
                    if (reqDiffX > 0) {
                        if (physics.Pusher.TargetDirection == PlatformPushDirection.Right)
                            UpdateTargetPushableSpeed(physics, targetPP);
                        else {
                            physics.Pusher.TargetEntity = Entity.None;
                            targetPP.Pushable.SourceEntity = Entity.None;
                        }
                    }
                    else if (reqDiffX < 0) {
                        if (physics.Pusher.TargetDirection == PlatformPushDirection.Left)
                            UpdateTargetPushableSpeed(physics, targetPP);
                        else {
                            physics.Pusher.TargetEntity = Entity.None;
                            targetPP.Pushable.SourceEntity = Entity.None;
                        }
                    }

                    if (actDiffY < -3 || actDiffY > 3) {
                        physics.Pusher.TargetEntity = Entity.None;
                        targetPP.Pushable.SourceEntity = Entity.None;
                    }
                }
            }
        }

        private void UpdateTargetPushableSpeed (PlatformPhysics source, PlatformPhysics target)
        {
            if (source.Pusher.DelayExpended < target.Pushable.PushDelay) {
                source.Pusher.DelayExpended += (float)SystemManager.World.GameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            if (source.Pusher.TargetDirection == PlatformPushDirection.Right)
                target.VelocityX = (FPInt)target.Pushable.PushSpeed;
            else
                target.VelocityX = -(FPInt)target.Pushable.PushSpeed;
        }

        private void SystemManager_SystemAdded (BaseSystem system)
        {
            if (system is BackgroundCollisionSystem) {
                _backCollisionSystem = system as BackgroundCollisionSystem;
            }
        }
    }
}
