using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        public PlatformMovement (GameObject obj)
            : base()
        {
            _object = obj;
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
            _object.Y -= _yVelocity * time;
        }
    }
}
