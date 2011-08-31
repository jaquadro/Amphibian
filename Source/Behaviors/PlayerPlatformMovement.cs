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

    public class PlayerPlatformMovement<TActionSet> : PlatformMovement
        where TActionSet : struct
    {
        private string _controllerName;
        private ButtonController<TActionSet> _controller;
        private Dictionary<PlatformAction, TActionSet> _inputMap;

        #region Constructors

        private PlayerPlatformMovement (GameObject obj)
            : base(obj)
        {
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

            base.Execute();
        }

        private void HandleInput ()
        {
            if (_controller.ButtonHeld(_inputMap[PlatformAction.Left])) {
                AccelX = -FPMath.Abs(AccelX);
                AccelStateX = PlatformAccelState.Accelerate;
            }
            else if (_controller.ButtonHeld(_inputMap[PlatformAction.Right])) {
                AccelX = FPMath.Abs(AccelX);
                AccelStateX = PlatformAccelState.Accelerate;
            }
            else {
                AccelStateX = PlatformAccelState.Decelerate;
            }

            if (_controller.ButtonPressed(_inputMap[PlatformAction.Jump])) {
                VelocityY = -12;
            }
        }
    }
}
