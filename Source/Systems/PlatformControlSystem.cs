using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Input;
using Amphibian.Components;
using Amphibian.Geometry;
using Amphibian.Utility;

namespace Amphibian.Systems
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

    public class PlatformActionEquality : IEqualityComparer<PlatformAction>
    {
        private static PlatformActionEquality _instance;

        public static PlatformActionEquality Default
        {
            get
            {
                if (_instance == null)
                    _instance = new PlatformActionEquality();
                return _instance;
            }
        }

        public bool Equals (PlatformAction val1, PlatformAction val2)
        {
            return val1 == val2;
        }

        public int GetHashCode (PlatformAction val)
        {
            return ((int)val).GetHashCode();
        }
    }

    public class PlatformControlSystem<TActionSet> : TagSystem
        where TActionSet : struct
    {
        private static string _tag = "platform_control";
        private static Dictionary<PlatformAction, PlatformAction> _identityMap;

        private string _controllerName;

        private ButtonController<TActionSet> _controller;
        private Dictionary<PlatformAction, TActionSet> _inputMap;

        static PlatformControlSystem ()
        {
            _identityMap = new Dictionary<PlatformAction, PlatformAction>(PlatformActionEquality.Default);

            foreach (PlatformAction action in EnumHelper.GetValues<PlatformAction>()) {
                _identityMap[action] = action;
            }
        }

        public PlatformControlSystem (ButtonController<TActionSet> buttonController, Dictionary<PlatformAction, TActionSet> controlMap)
            : base(_tag)
        {
            _controller = buttonController;
            _inputMap = controlMap;
        }

        public PlatformControlSystem (Engine engine, string buttonControllerName, Dictionary<PlatformAction, TActionSet> controlMap)
            : base(_tag)
        {
            _controllerName = buttonControllerName;
            _controller = engine.GetController(buttonControllerName) as ButtonController<TActionSet>;
            _inputMap = controlMap;

            engine.ControllerAdded += HandleControllerAdded;
            engine.ControllerRemoved += HandleControllerRemoved;
        }

        public static string Tag
        {
            get { return _tag; }
        }

        public static Dictionary<PlatformAction, PlatformAction> IdentityMap
        {
            get { return _identityMap; }
        }

        public override void Process (Entity entity)
        {
            PlatformPhysics physicsCom = null;

            foreach (IComponent com in EntityManager.GetComponents(entity)) {
                if (com is PlatformPhysics) {
                    physicsCom = com as PlatformPhysics;
                    break;
                }
            }

            if (physicsCom == null)
                return;

            if (_controller == null)
                return;

            HandleInput(physicsCom);
        }

        private void HandleInput (PlatformPhysics physics)
        {
            if (_controller.ButtonHeld(_inputMap[PlatformAction.Left])) {
                physics.AccelX = -FPMath.Abs(physics.AccelX);
                physics.AccelStateX = PlatformAccelState.Accelerate;
            }
            else if (_controller.ButtonHeld(_inputMap[PlatformAction.Right])) {
                physics.AccelX = FPMath.Abs(physics.AccelX);
                physics.AccelStateX = PlatformAccelState.Accelerate;
            }
            else {
                physics.AccelStateX = PlatformAccelState.Decelerate;
            }

            if (_controller.ButtonPressed(_inputMap[PlatformAction.Jump])) {
                physics.VelocityY = -12;
            }
        }

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
    }
}
