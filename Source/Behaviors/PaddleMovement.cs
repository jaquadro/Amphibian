using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amphibian.Input;

namespace Amphibian.Behaviors
{
    public enum PaddleDirection
    {
        Horizontal,
        Vertical
    }

    public enum PaddleAction
    {
        Left,
        Right,
        Up,
        Down
    }

    public class PaddleMovement<TActionSet> : Behavior 
        where TActionSet : struct
    {
        private string _controllerName;
        private ButtonController<TActionSet> _controller;
        private Dictionary<PaddleAction, TActionSet> _inputMap;

        private Vector2 _center;
        private PaddleDirection _direction;
        private float _range;
        private float _speed;

        private GameObject _object;
        private float _pos;

        public PaddleMovement (GameObject obj, string buttonControllerName, Dictionary<PaddleAction, TActionSet> controlMap)
            : base()
        {
            Engine engine = obj.Parent.Engine;

            _controllerName = buttonControllerName;
            _controller = engine.GetController(buttonControllerName) as ButtonController<TActionSet>;
            _inputMap = controlMap;

            _object = obj;
        }

        public PaddleMovement (GameObject obj, ButtonController<TActionSet> buttonController, Dictionary<PaddleAction, TActionSet> controlMap)
            : base()
        {
            _controller = buttonController;
            _inputMap = controlMap;

            _object = obj;
        }

        #region Properties

        public PaddleDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public Vector2 Origin
        {
            get { return _center; }
            set { _center = value; }
        }

        public float Range
        {
            get { return _range; }
            set { _range = value; }
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
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
            if (_controller == null) {
                return;
            }

            float time = (float)_object.Parent.Engine.GameTime.ElapsedGameTime.TotalSeconds;
            float diff = 0;

            if (_direction == PaddleDirection.Horizontal) {
                if (_controller.ButtonHeld(_inputMap[PaddleAction.Left])) {
                    diff = time * -_speed;
                }
                else if (_controller.ButtonHeld(_inputMap[PaddleAction.Right])) {
                    diff = time * _speed;
                }
            }
            else {
                if (_controller.ButtonHeld(_inputMap[PaddleAction.Up])) {
                    diff = time * -_speed;
                }
                else if (_controller.ButtonHeld(_inputMap[PaddleAction.Down])) {
                    diff = time * _speed;
                }
            }

            _pos += diff;
            _pos = MathHelper.Max(_pos, -_range);
            _pos = MathHelper.Min(_pos, _range);

            if (_direction == PaddleDirection.Horizontal) {
                _object.X = (float)(Math.Floor(_center.X + _pos));
            }
            else {
                _object.Y = (float)(Math.Floor(_center.Y + _pos));
            }
        }
    }
}
