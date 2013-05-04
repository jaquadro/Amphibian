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

            PlatformPhysics physicsCom;
            DirectionComponent directionCom;
            ActivityComponent activityCom;

            if (!EntityManager.GetComponent<PlatformPhysics>(entity, out physicsCom))
                return;

            EntityManager.GetComponent<DirectionComponent, ActivityComponent>(entity, out directionCom, out activityCom);

            HandleInput(physicsCom, directionCom);

            if (activityCom != null) {
                if (physicsCom.VelocityY == 0) {
                    if (physicsCom.VelocityX == 0)
                        activityCom.Activity = "Standing";
                    else
                        activityCom.Activity = "Walking";
                }
                else if (physicsCom.VelocityY < 0)
                    activityCom.Activity = "Jumping";
                else if (physicsCom.VelocityY > 0)
                    activityCom.Activity = "Falling";

                if (physicsCom.Pusher.TargetEntity != Entity.None)
                    activityCom.Activity = "Pushing";
            }
        }

        private void HandleInput (PlatformPhysics physicsCom, DirectionComponent directionCom)
        {
            if (_controller.ButtonHeld(PlatformAction.Left)) {
                physicsCom.AccelX = -FPMath.Abs(physicsCom.AccelX);
                physicsCom.AccelStateX = PlatformAccelState.Accelerate;
                if (directionCom != null)
                    directionCom.Direction = Rendering.Sprites.Direction.West;
            }
            else if (_controller.ButtonHeld(PlatformAction.Right)) {
                physicsCom.AccelX = FPMath.Abs(physicsCom.AccelX);
                physicsCom.AccelStateX = PlatformAccelState.Accelerate;
                if (directionCom != null)
                    directionCom.Direction = Rendering.Sprites.Direction.East;
            }
            else {
                physicsCom.AccelStateX = PlatformAccelState.Decelerate;
            }

            if (_controller.ButtonPressed(PlatformAction.Jump)) {
                physicsCom.VelocityY = -8;
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
