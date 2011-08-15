using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Amphibian.Collision;
using Amphibian.Geometry;
using Amphibian.Input;

namespace Amphibian.Behaviors
{
    public enum PlatformAction
    {
        Left,
        Right,
        Up,
        Down,
        Jump,
        Action
    }

    public class PlayerPlatformMovement<TActionSet> : Behavior
        where TActionSet : struct
    {
        private const int _maxSlope = 2;

        private string _controllerName;
        private ButtonController<TActionSet> _controller;
        private Dictionary<PlatformAction, TActionSet> _inputMap;

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

        // Detectors
        private FPInt _detLowLeft;
        private FPInt _detLowRight;
        private AYLineMask _detLeft;
        private AYLineMask _detRight;

        #region Constructors

        private PlayerPlatformMovement (GameObject obj)
            : base()
        {
            _object = obj;

            ICollidable c = _object as ICollidable;
            _floorDet = new AXLineMask(new PointFP(c.CollisionMask.Bounds.Left - _object.X, c.CollisionMask.Bounds.Bottom - _object.Y), c.CollisionMask.Bounds.Width);
            _floorDet.Position = _object.Position;

            _subFloorDet = new AXLineMask(new PointFP(c.CollisionMask.Bounds.Left - _object.X, c.CollisionMask.Bounds.Bottom - _object.Y + 1), c.CollisionMask.Bounds.Width);
            _subFloorDet.Position = _object.Position;

            _detLowLeft = c.CollisionMask.Bounds.Left - _object.X + 2;
            _detLowRight = c.CollisionMask.Bounds.Right - _object.X - 2;

            _detLeft = new AYLineMask(new PointFP(_detLowLeft - 2, c.CollisionMask.Bounds.Top - _object.Y + _maxSlope), c.CollisionMask.Bounds.Height - (2 * _maxSlope));
            _detRight = new AYLineMask(new PointFP(_detLowRight + 2, c.CollisionMask.Bounds.Top - _object.Y + _maxSlope), c.CollisionMask.Bounds.Height - (2 * _maxSlope));

            _detLeft.Position = _object.Position;
            _detRight.Position = _object.Position;
        }

        public PlayerPlatformMovement (GameObject obj, string buttonControllerName, Dictionary<PlatformAction, TActionSet> controlMap)
            : this(obj)
        {
            Engine engine = obj.Parent.Engine;

            _controllerName = buttonControllerName;
            _controller = engine.GetController(buttonControllerName) as ButtonController<TActionSet>;
            _inputMap = controlMap;
        }

        public PlayerPlatformMovement (GameObject obj, ButtonController<TActionSet> buttonController, Dictionary<PlatformAction, TActionSet> controlMap)
            : this(obj)
        {
            _controller = buttonController;
            _inputMap = controlMap;
        }

        #endregion

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

        public FPInt DetectorLowerLeft
        {
            get { return _detLowLeft; }
            set { _detLowLeft = value; }
        }

        public FPInt DetectorLowerRight
        {
            get { return _detLowRight; }
            set { _detLowRight = value; }
        }

        #endregion

        #region Event Handlers

        private void HandleControllerAdded (Object sender, ControllerEventArgs e)
        {
            if (_controllerName == e.Name) {
                _controller = e.Controller as ButtonController<TActionSet>;
            }
        }

        private void HandleControllerRemoved (Object sender, ControllerEventArgs e)
        {
            if (_controllerName == e.Name) {
                _controller = null;
            }
        }

        #endregion

        public override void Execute ()
        {
            HandleInput();

            float time = (float)_object.Parent.Engine.GameTime.ElapsedGameTime.TotalSeconds;

            FPInt txAccel = (FPInt)((float)_xAccel * time);
            FPInt tyAccel = (FPInt)((float)_yAccel * time);

            _xVelocity = FPMath.Clamp(_xVelocity + txAccel, _xMinVel, _xMaxVel);
            _yVelocity = FPMath.Clamp(_yVelocity + tyAccel, _yMinVel, _yMaxVel);

            FPInt txVelocity = (FPInt)((float)_xVelocity * time);
            FPInt tyVelocity = (FPInt)((float)_yVelocity * time);

            MoveLR(txVelocity);

            //_object.X += txVelocity;
            //_object.Y -= _yVelocity * time;



            /*if (_object.Parent.OverlapsBackdrop(c.CollisionMask)) {
                _xAccel = 0;
                _yAccel = 0;
                _xVelocity = 0;
                _yVelocity = 0;
            }*/

            Fall(tyVelocity);
        }

        private void HandleInput ()
        {
            if (_controller.ButtonHeld(_inputMap[PlatformAction.Left])) {
                _xAccel = -500;
            }
            else if (_controller.ButtonHeld(_inputMap[PlatformAction.Right])) {
                _xAccel = 500;
            }
            else {
                _xAccel = 0;
            }
        }

        private void StepMovement (FPInt distX, FPInt distY)
        {
            ICollidable c = _object as ICollidable;
            RectangleFP bb = c.CollisionMask.Bounds;

            // X Movement
            _object.X += distX;

            // Y Movement
            bool testLL = _object.Parent.OverlapsBackdrop(_object.X + _detLowLeft, bb.Bottom);
            bool testLR = _object.Parent.OverlapsBackdrop(_object.X + _detLowRight, bb.Bottom);
            bool testLC = _object.Parent.OverlapsBackdrop(_object.X, bb.Bottom + 1);

            // Y - No points colliding; Fall
            if (!(testLC | testLL | testLR)) {

            }
            // Y - Center subpoint colliding; On Level Terrain
            else if (testLC && !(testLL | testLR)) {

            }
            // Y - Both points colliding; Dig Out
            else if (testLL & testLR) {

            }
            // Y - One point colliding; Downward Slope Movement (Modified Fall)
            else {

            }
        }

        private void MoveLR (FPInt dist)
        {
            _object.X += dist;

            if (dist > 0) {
                if (_object.Parent.OverlapsBackdrop(_detRight)) {
                    _xVelocity = 0;

                    _object.X = (FPInt)_object.X.Floor;
                    while (_object.Parent.OverlapsBackdrop(_detRight)) {
                        _object.X -= 1;
                    }
                }
            }
            else if (dist < 0) {
                if (_object.Parent.OverlapsBackdrop(_detLeft)) {
                    _xVelocity = 0;

                    _object.X = (FPInt)_object.X.Floor;
                    while (_object.Parent.OverlapsBackdrop(_detLeft)) {
                        _object.X += 1;
                    }
                }
            }            
        }

        /*private void MoveFall (FPInt distY)
        {
            AYLineMask 
        }*/

        private void Fall (FPInt dist)
        {
            if (_object.Parent.OverlapsBackdrop(_subFloorDet)) {
                if (!_object.Parent.OverlapsBackdrop(_floorDet)) {
                    _yVelocity = 0;
                    return;
                }

                _yVelocity = 0;

                while (_object.Parent.OverlapsBackdrop(_floorDet)) {
                    _object.Offset(0, -1);
                    //_floorDet.Position = new PointFP(_floorDet.Position.X, _floorDet.Position.Y - 1);
                }

                //_object.Y = _floorDet.Position.Y;
                //_subFloorDet.Position = new PointFP(_subFloorDet.Position.X, _floorDet.Position.Y + 1);

                //ICollidable c = _object as ICollidable;
                //Mask mask = c.CollisionMask;
                //mask.Position = new PointFP(mask.Position.X, _object.Y);
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
                //_floorDet.Position = new PointFP(_floorDet.Position.X, _object.Y);
                //_subFloorDet.Position = new PointFP(_floorDet.Position.X, _object.Y + 1);

                //ICollidable c = _object as ICollidable;
                //Mask mask = c.CollisionMask;
                //mask.Position = new PointFP(mask.Position.X, _object.Y);
            }
        }


    }
}
