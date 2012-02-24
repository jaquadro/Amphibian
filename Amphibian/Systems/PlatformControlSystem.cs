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
        private class ControllerInfo
        {
            public string ControllerName { get; set; }
            public ButtonController<TActionSet> Controller { get; set; }
            public Dictionary<PlatformAction, TActionSet> InputMap { get; set; }
        }

        private static string _tag = "platform_control";
        private static Dictionary<PlatformAction, PlatformAction> _identityMap;

        private List<ControllerInfo> _controllers;

        static PlatformControlSystem ()
        {
            _identityMap = new Dictionary<PlatformAction, PlatformAction>(PlatformActionEquality.Default);

            foreach (PlatformAction action in EnumHelper.GetValues<PlatformAction>()) {
                _identityMap[action] = action;
            }
        }

        public PlatformControlSystem ()
            : base(_tag)
        {
            _controllers = new List<ControllerInfo>();
        }

        public static string Tag
        {
            get { return _tag; }
        }

        public static Dictionary<PlatformAction, PlatformAction> IdentityMap
        {
            get { return _identityMap; }
        }

        public void AddController (string buttonControllerName, Dictionary<PlatformAction, TActionSet> controlMap)
        {
            Engine engine = this.SystemManager.World.Frame.Engine;

            ControllerInfo info = new ControllerInfo()
            {
                ControllerName = buttonControllerName,
                Controller = engine.GetController(buttonControllerName) as ButtonController<TActionSet>,
                InputMap = controlMap,
            };

            _controllers.Add(info);
        }

        public void RemoveController (string buttonControllerName)
        {
            foreach (ControllerInfo info in _controllers) {
                if (info.ControllerName == buttonControllerName) {
                    _controllers.Remove(info);
                    break;
                }
            }
        }

        protected internal override void Initialize ()
        {
            Engine engine = this.SystemManager.World.Frame.Engine;
            engine.ControllerAdded += HandleControllerAdded;
            engine.ControllerRemoved += HandleControllerRemoved;
        }

        public override void Process (Entity entity)
        {
            if (_controllers.Count == 0)
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
            foreach (ControllerInfo info in _controllers) {
                if (info.Controller.ButtonHeld(info.InputMap[PlatformAction.Left])) {
                    physics.AccelX = -FPMath.Abs(physics.AccelX);
                    physics.AccelStateX = PlatformAccelState.Accelerate;
                    break;
                }
                else if (info.Controller.ButtonHeld(info.InputMap[PlatformAction.Right])) {
                    physics.AccelX = FPMath.Abs(physics.AccelX);
                    physics.AccelStateX = PlatformAccelState.Accelerate;
                    break;
                }
                else {
                    physics.AccelStateX = PlatformAccelState.Decelerate;
                }
            }

            foreach (ControllerInfo info in _controllers) {
                if (info.Controller.ButtonPressed(info.InputMap[PlatformAction.Jump])) {
                    physics.VelocityY = -12;
                    break;
                }
            }
        }

        private void HandleControllerAdded (Object sender, ControllerEventArgs e)
        {
            foreach (ControllerInfo info in _controllers) {
                if (info.ControllerName == e.Name) {
                    info.Controller = e.Controller as ButtonController<TActionSet>;
                }
            }
        }

        private void HandleControllerRemoved (Object sender, ControllerEventArgs e)
        {
            foreach (ControllerInfo info in _controllers) {
                if (info.ControllerName == e.Name) {
                    _controllers.Remove(info);
                    break;
                }
            }
        }
    }
}
