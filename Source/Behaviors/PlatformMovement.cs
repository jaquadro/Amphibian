using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Amphibian.Collision;
using Amphibian.Geometry;

namespace Amphibian.Behaviors
{
    public class PlatformMovement : Behavior
    {
        private GameObject _object;

        private FPInt _xAccel;
        private FPInt _yAccel;
        private FPInt _xVelocity;
        private FPInt _yVelocity;
        private FPInt _xMinVel;
        private FPInt _yMinVel;
        private FPInt _xMaxVel;
        private FPInt _yMaxVel;

        private AXLineMask _floorDet;
        private AXLineMask _subFloorDet;

        public PlatformMovement (GameObject obj)
            : base()
        {
            _object = obj;

            ICollidable c = _object as ICollidable;
            _floorDet = new AXLineMask(new PointFP(obj.X - c.CollisionMask.Bounds.Left, c.CollisionMask.Bounds.Bottom - obj.Y), c.CollisionMask.Bounds.Width);
            _floorDet.Position = new PointFP(obj.X, obj.Y);

            _subFloorDet = _floorDet.Clone() as AXLineMask;
            _subFloorDet.Position = new PointFP(_subFloorDet.Position.X, _subFloorDet.Position.Y + 1);
        }

        #region Properties

        public FPInt AccelX
        {
            get { return _xAccel; }
            set { _xAccel = value; }
        }

        public FPInt AccelY
        {
            get { return _yAccel; }
            set { _yAccel = value; }
        }

        public FPInt MaxVelocityX
        {
            get { return _xMaxVel; }
            set { _xMaxVel = value; }
        }

        public FPInt MaxVelocityY
        {
            get { return _yMaxVel; }
            set { _yMaxVel = value; }
        }

        public FPInt MinVelocityX
        {
            get { return _xMinVel; }
            set { _xMinVel = value; }
        }

        public FPInt MinVelocityY
        {
            get { return _yMinVel; }
            set { _yMinVel = value; }
        }

        public FPInt VelocityX
        {
            get { return _xVelocity; }
            set { _xVelocity = value; }
        }

        public FPInt VelocityY
        {
            get { return _yVelocity; }
            set { _yVelocity = value; }
        }

        #endregion

        public override void Execute ()
        {
            float time = (float)_object.Parent.Engine.GameTime.ElapsedGameTime.TotalSeconds;

            FPInt txAccel = (FPInt)((float)_xAccel * time);
            FPInt tyAccel = (FPInt)((float)_yAccel * time);

            _xVelocity = FPMath.Clamp(_xVelocity + txAccel, _xMinVel, _xMaxVel);
            _yVelocity = FPMath.Clamp(_yVelocity + tyAccel, _yMinVel, _yMaxVel);

            FPInt txVelocity = (FPInt)((float)_xVelocity * time);
            FPInt tyVelocity = (FPInt)((float)_yVelocity * time);

            _object.X += txVelocity;
            //_object.Y -= _yVelocity * time;

            

            /*if (_object.Parent.OverlapsBackdrop(c.CollisionMask)) {
                _xAccel = 0;
                _yAccel = 0;
                _xVelocity = 0;
                _yVelocity = 0;
            }*/

            Fall(tyVelocity);

            ICollidable c = _object as ICollidable;
            c.CollisionMask.Position = new PointFP(_object.X, c.CollisionMask.Position.Y);

            _floorDet.Position = new PointFP(_object.X, _object.Y);
            _subFloorDet.Position = new PointFP(_object.X, _object.Y + 1);
        }

        private void Fall (FPInt dist)
        {
            if (_object.Parent.OverlapsBackdrop(_subFloorDet)) {
                if (!_object.Parent.OverlapsBackdrop(_floorDet)) {
                    _yVelocity = 0;
                    return;
                }

                _yVelocity = 0;

                while (_object.Parent.OverlapsBackdrop(_floorDet)) {
                    _floorDet.Position = new PointFP(_floorDet.Position.X, _floorDet.Position.Y - 1);
                }

                _object.Y = _floorDet.Position.Y;
                _subFloorDet.Position = new PointFP(_subFloorDet.Position.X, _floorDet.Position.Y + 1);

                ICollidable c = _object as ICollidable;
                Mask mask = c.CollisionMask;
                mask.Position = new PointFP(mask.Position.X, _object.Y);
            }
            else {
                AABBMask rmask = new AABBMask(new PointFP(_floorDet.Bounds.Left, _floorDet.Bounds.Bottom), _floorDet.Bounds.Width, -dist);
                if (_object.Parent.OverlapsBackdrop(rmask)) {
                    _yVelocity = 0;

                    while (_object.Parent.OverlapsBackdrop(rmask)) {
                        rmask = new AABBMask(new PointFP(rmask.Bounds.Left, rmask.Bounds.Top), rmask.Bounds.Width, rmask.Bounds.Height - 1);
                    }
                }

                _object.Y += rmask.Bounds.Height;
                _floorDet.Position = new PointFP(_floorDet.Position.X, _object.Y);
                _subFloorDet.Position = new PointFP(_floorDet.Position.X, _object.Y + 1);

                ICollidable c = _object as ICollidable;
                Mask mask = c.CollisionMask;
                mask.Position = new PointFP(mask.Position.X, _object.Y);
            }
        }

        private void StepMovement ()
        {

        }
    }
}
