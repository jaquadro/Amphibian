using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Amphibian.Collision;

namespace Amphibian.Behaviors
{
    public class PlatformMovement : Behavior
    {
        private GameObject _object;

        private float _xAccel;
        private float _yAccel;
        private float _xVelocity;
        private float _yVelocity;
        private float _xMinVel;
        private float _yMinVel;
        private float _xMaxVel;
        private float _yMaxVel;

        private AXLineMask _floorDet;
        private AXLineMask _subFloorDet;

        public PlatformMovement (GameObject obj)
            : base()
        {
            _object = obj;

            ICollidable c = _object as ICollidable;
            _floorDet = new AXLineMask(new Vector2(obj.X - c.CollisionMask.Bounds.Left, c.CollisionMask.Bounds.Bottom - obj.Y), c.CollisionMask.Bounds.Width);
            _floorDet.Position = new Vector2(obj.X, obj.Y);

            _subFloorDet = _floorDet.Clone() as AXLineMask;
            _subFloorDet.Position = new Vector2(_subFloorDet.Position.X, _subFloorDet.Position.Y + 1);
        }

        #region Properties

        public float AccelX
        {
            get { return _xAccel; }
            set { _xAccel = value; }
        }

        public float AccelY
        {
            get { return _yAccel; }
            set { _yAccel = value; }
        }

        public float MaxVelocityX
        {
            get { return _xMaxVel; }
            set { _xMaxVel = value; }
        }

        public float MaxVelocityY
        {
            get { return _yMaxVel; }
            set { _yMaxVel = value; }
        }

        public float MinVelocityX
        {
            get { return _xMinVel; }
            set { _xMinVel = value; }
        }

        public float MinVelocityY
        {
            get { return _yMinVel; }
            set { _yMinVel = value; }
        }

        public float VelocityX
        {
            get { return _xVelocity; }
            set { _xVelocity = value; }
        }

        public float VelocityY
        {
            get { return _yVelocity; }
            set { _yVelocity = value; }
        }

        #endregion

        public override void Execute ()
        {
            float time = (float)_object.Parent.Engine.GameTime.ElapsedGameTime.TotalSeconds;

            _xVelocity = MathHelper.Clamp(_xVelocity + _xAccel * time, _xMinVel, _xMaxVel);
            _yVelocity = MathHelper.Clamp(_yVelocity + _yAccel * time, _yMinVel, _yMaxVel);

            _object.X += _xVelocity * time;
            //_object.Y -= _yVelocity * time;

            

            /*if (_object.Parent.OverlapsBackdrop(c.CollisionMask)) {
                _xAccel = 0;
                _yAccel = 0;
                _xVelocity = 0;
                _yVelocity = 0;
            }*/

            Fall(_yVelocity * time);

            ICollidable c = _object as ICollidable;
            c.CollisionMask.Position = new Vector2(_object.X, c.CollisionMask.Position.Y);

            _floorDet.Position = new Vector2(_object.X, _object.Y);
            _subFloorDet.Position = new Vector2(_object.X, _object.Y + 1);
        }

        private void Fall (float dist)
        {
            if (_object.Parent.OverlapsBackdrop(_subFloorDet)) {
                if (!_object.Parent.OverlapsBackdrop(_floorDet)) {
                    _yVelocity = 0;
                    return;
                }

                _yVelocity = 0;

                while (_object.Parent.OverlapsBackdrop(_floorDet)) {
                    _floorDet.Position = new Vector2(_floorDet.Position.X, _floorDet.Position.Y - 1);
                }

                _object.Y = _floorDet.Position.Y;
                _subFloorDet.Position = new Vector2(_subFloorDet.Position.X, _floorDet.Position.Y + 1);

                ICollidable c = _object as ICollidable;
                Mask mask = c.CollisionMask;
                mask.Position = new Vector2(mask.Position.X, _object.Y);
            }
            else {
                AABBMask rmask = new AABBMask(new Vector2(_floorDet.Bounds.Left, _floorDet.Bounds.Bottom), _floorDet.Bounds.Width, -dist);
                if (_object.Parent.OverlapsBackdrop(rmask)) {
                    _yVelocity = 0;

                    while (_object.Parent.OverlapsBackdrop(rmask)) {
                        rmask = new AABBMask(new Vector2(rmask.Bounds.Left, rmask.Bounds.Top), rmask.Bounds.Width, rmask.Bounds.Height - 1);
                    }
                }

                _object.Y += rmask.Bounds.Height;
                _floorDet.Position = new Vector2(_floorDet.Position.X, _object.Y);
                _subFloorDet.Position = new Vector2(_floorDet.Position.X, _object.Y + 1);

                ICollidable c = _object as ICollidable;
                Mask mask = c.CollisionMask;
                mask.Position = new Vector2(mask.Position.X, _object.Y);
            }
        }

        private void StepMovement ()
        {

        }
    }
}
