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

    public class PlatformControlSystem : TagSystem
    {
        private static string _tag = "platform_control";

        private string _controllerName;
        private ButtonController<PlatformAction> _controller;

        public static string Tag
        {
            get { return _tag; }
        }

        public PlatformControlSystem ()
            : base(_tag)
        {
        }

        public PlatformControlSystem (string buttonControllerName)
            : this()
        {
            Controller = buttonControllerName;
        }

        public string Controller
        {
            get { return _controllerName; }
            set
            {
                _controllerName = value;

                if (this.SystemManager != null) {
                    Engine engine = this.SystemManager.World.Frame.Engine;
                    _controller = engine.GetController(value) as ButtonController<PlatformAction>;
                }
            }
        }

        protected internal override void Initialize ()
        {
            Engine engine = this.SystemManager.World.Frame.Engine;
            engine.ControllerAdded += HandleControllerAdded;
            engine.ControllerRemoved += HandleControllerRemoved;

            if (_controller == null)
                Controller = Controller;
        }

        public override void Process (Entity entity)
        {
            if (_controller == null)
                return;

            PlatformPhysics physicsCom = null;

            foreach (IComponent com in EntityManager.GetComponents(entity)) {
                if (com is PlatformPhysics) {
                    physicsCom = com as PlatformPhysics;
                    break;
                }
            }

            if (physicsCom == null)
                return;

            HandleInput(physicsCom);
        }

        private void HandleInput (PlatformPhysics physics)
        {
            if (_controller.ButtonHeld(PlatformAction.Left)) {
                physics.AccelX = -FPMath.Abs(physics.AccelX);
                physics.AccelStateX = PlatformAccelState.Accelerate;
            }
            else if (_controller.ButtonHeld(PlatformAction.Right)) {
                physics.AccelX = FPMath.Abs(physics.AccelX);
                physics.AccelStateX = PlatformAccelState.Accelerate;
            }
            else {
                physics.AccelStateX = PlatformAccelState.Decelerate;
            }

            if (_controller.ButtonPressed(PlatformAction.Jump)) {
                physics.VelocityY = -12;
            }
        }

        private void HandleControllerAdded (Object sender, ControllerEventArgs e)
        {
            if (_controllerName == e.Name)
                _controller = e.Controller as ButtonController<PlatformAction>;
        }

        private void HandleControllerRemoved (Object sender, ControllerEventArgs e)
        {
            if (_controllerName == e.Name)
                _controller = null;
        }
    }
}
