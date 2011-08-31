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

    public enum PlatformAccelState
    {
        Fixed,
        Accelerate,
        Decelerate
    }

    public class PlayerPlatformMovement<TActionSet> : InterpBehavior
        where TActionSet : struct
    {
        private class State
        {
            public FPInt ObjectX;
            public FPInt ObjectY;
        }

        private const int _maxSlope = 2;

        private string _controllerName;
        private ButtonController<TActionSet> _controller;
        private Dictionary<PlatformAction, TActionSet> _inputMap;

        private GameObject _object;

        private FPInt _xAccel;
        private FPInt _yAccel;
        private FPInt _xDecel;
        private FPInt _yDecel;
        private FPInt _xVelocity;
        private FPInt _yVelocity;
        private FPInt _xMinVel;
        private FPInt _yMinVel;
        private FPInt _xMaxVel;
        private FPInt _yMaxVel;
        private PlatformAccelState _xAccelState;
        private PlatformAccelState _yAccelState;

        // Detectors
        private AXLineMask _detLow;
        private AXLineMask _detHigh;
        private FPInt _detLowLeft;
        private FPInt _detLowRight;
        private AYLineMask _detLeft;
        private AYLineMask _detRight;

        private State _current;
        private State _prev;

        #region Constructors

        private PlayerPlatformMovement (GameObject obj)
            : base()
        {
            _object = obj;

            ICollidable c = _object as ICollidable;

            _detLow = new AXLineMask(new PointFP(c.CollisionMask.Bounds.Left - _object.X + 1, c.CollisionMask.Bounds.Bottom - _object.Y), c.CollisionMask.Bounds.Width - 2);
            _detHigh = new AXLineMask(new PointFP(c.CollisionMask.Bounds.Left - _object.X + 1, c.CollisionMask.Bounds.Top - _object.Y), c.CollisionMask.Bounds.Width - 2);

            _detLowLeft = c.CollisionMask.Bounds.Left - _object.X;
            _detLowRight = c.CollisionMask.Bounds.Right - _object.X;

            _detLeft = new AYLineMask(new PointFP(_detLowLeft, c.CollisionMask.Bounds.Top - _object.Y + _maxSlope), c.CollisionMask.Bounds.Height - (4 * _maxSlope));
            _detRight = new AYLineMask(new PointFP(_detLowRight, c.CollisionMask.Bounds.Top - _object.Y + _maxSlope), c.CollisionMask.Bounds.Height - (4 * _maxSlope));

            _detLow.Position = _object.Position;
            _detHigh.Position = _object.Position;
            _detLeft.Position = _object.Position;
            _detRight.Position = _object.Position;

            _current = new State();
            _prev = new State();
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

        public FPInt DecelX
        {
            get { return _xDecel; }
            set { _xDecel = value; }
        }

        public FPInt DecelY
        {
            get { return _yDecel; }
            set { _yDecel = value; }
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

        public PlatformAccelState AccelStateX
        {
            get { return _xAccelState; }
            set { _xAccelState = value; }
        }

        public PlatformAccelState AccelStateY
        {
            get { return _yAccelState; }
            set { _yAccelState = value; }
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

            float time = (float)_object.Parent.Engine.GameTime.ElapsedGameTime.TotalSeconds * 60f;

            FPInt diffPosX, diffPosY;
            StepPhysics(time, out diffPosX, out diffPosY);

            MoveLR(diffPosX);
            StepMovement(diffPosX, diffPosY);

            _prev = _current;
            _current.ObjectX = _object.X;
            _current.ObjectY = _object.Y;

            _object.RenderAt = null;
        }

        public override void Interpolate (double alpha)
        {
            FPInt midx = _current.ObjectX;
            if (midx != _prev.ObjectX) {
                midx = (FPInt)((double)midx * alpha + (double)_prev.ObjectX * (1.0 - alpha));
            }

            FPInt midy = _current.ObjectY;
            if (midy != _prev.ObjectY) {
                midy = (FPInt)((double)midy * alpha + (double)_prev.ObjectY * (1.0 - alpha));
            }

            _object.RenderAt = new SharedPointFP(midx, midy);
        }

        private void HandleInput ()
        {
            if (_controller.ButtonHeld(_inputMap[PlatformAction.Left])) {
                _xAccel = -FPMath.Abs(_xAccel);
                _xAccelState = PlatformAccelState.Accelerate;
            }
            else if (_controller.ButtonHeld(_inputMap[PlatformAction.Right])) {
                _xAccel = FPMath.Abs(_xAccel);
                _xAccelState = PlatformAccelState.Accelerate;
            }
            else {
                _xAccelState = PlatformAccelState.Decelerate;
            }

            if (_controller.ButtonPressed(_inputMap[PlatformAction.Jump])) {
                _yVelocity = -12;
            }
        }

        private void StepPhysics (float time, out FPInt diffPosX, out FPInt diffPosY)
        {
            FPInt diffVelX = 0;
            FPInt diffVelY = 0;

            switch(_xAccelState) {
                case PlatformAccelState.Accelerate:
                    diffVelX = (FPInt)((float)_xAccel * time);
                    break;
                case PlatformAccelState.Decelerate:
                    diffVelX = (FPInt)((float)_xDecel * time);
                    if (_xVelocity > 0) {
                        diffVelX = -FPMath.Min(_xVelocity, diffVelX);
                    }
                    else {
                        diffVelX = -FPMath.Max(_xVelocity, -diffVelX);
                    }
                    break;
            }

            switch (_yAccelState) {
                case PlatformAccelState.Accelerate:
                    diffVelY = (FPInt)((float)_yAccel * time);
                    break;
                case PlatformAccelState.Decelerate:
                    diffVelY = (FPInt)((float)_yDecel * time);
                    if (_yVelocity > 0) {
                        diffVelY = -FPMath.Min(_yVelocity, diffVelY);
                    }
                    else {
                        diffVelY = -FPMath.Max(_yVelocity, -diffVelY);
                    }
                    break;
            }

            FPInt nextVelX = FPMath.Clamp(_xVelocity + diffVelX, _xMinVel, _xMaxVel);
            FPInt nextVelY = FPMath.Clamp(_yVelocity + diffVelY, _yMinVel, _yMaxVel);

            diffPosX = (FPInt)((float)((_xVelocity + nextVelX) >> 1) * time);
            diffPosY = (FPInt)((float)((_yVelocity + nextVelY) >> 1) * time);

            _xVelocity = nextVelX;
            _yVelocity = nextVelY;
        }

        private void StepMovement (FPInt distX, FPInt distY)
        {
            _object.Y += distY;

            if (distY > 0) {
                if (_object.Parent.TestBackdropEdge(_detLow)) {
                    _yVelocity = 0;

                    _object.Y = (FPInt)_object.Y.Floor;
                    while (_object.Parent.TestBackdropEdge(_detLow)) {
                        _object.Y -= 1;
                    }
                    _object.Y += 1;

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
                if (_object.Parent.TestBackdropEdge(_detHigh)) {
                    _yVelocity = 0;

                    _object.Y = (FPInt)_object.Y.Floor;
                    while (_object.Parent.TestBackdropEdge(_detHigh)) {
                        _object.Y += 1;
                    }
                }
            }
        }

        private void MoveLR (FPInt dist)
        {
            if (dist > 0) {
                _object.X -= dist.Ceil - dist;
                dist = (FPInt)dist.Ceil;

                for (; dist > 0; dist -= 1) {
                    _object.X += 1;

                    if (_object.Parent.TestBackdrop(_detRight)) {
                        _xVelocity = 0;
                        _object.X = (FPInt)_object.X.Floor;

                        if (_object.Parent.TestBackdrop(_detRight)) {
                            _object.X -= 1;
                        }
                    }

                    bool test1 = _object.Parent.TestBackdropEdge(_object.X + _detLowLeft, _detLow.Position.Y);
                    bool test2 = _object.Parent.TestBackdropEdge(_object.X + _detLowLeft, _detLow.Position.Y + 1);

                    if (!test1 && test2) {
                        _object.Y += 1;
                    }
                }
            }
            else if (dist < 0) {
                _object.X += dist - dist.Floor;
                dist = (FPInt)dist.Floor;

                for (; dist < 0; dist += 1) {
                    _object.X -= 1;

                    if (_object.Parent.TestBackdrop(_detLeft)) {
                        _xVelocity = 0;
                        _object.X = (FPInt)_object.X.Ceil;

                        if (_object.Parent.TestBackdrop(_detLeft)) {
                            _object.X += 1;
                        }
                    }

                    bool test1 = _object.Parent.TestBackdropEdge(_object.X + _detLowRight, _detLow.Position.Y);
                    bool test2 = _object.Parent.TestBackdropEdge(_object.X + _detLowRight, _detLow.Position.Y + 1);

                    if (!test1 && test2) {
                        _object.Y += 1;
                    }
                }
            }

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
    }
}
