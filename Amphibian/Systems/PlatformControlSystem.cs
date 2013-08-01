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
        Action,
        Run
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

    public class PlatformControlSystem : ProcessingSystem
    {
        public PlatformControlSystem ()
            : base(typeof(InputComponent))
        { }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity e in entities)
                Process(e);
        }

        protected override void Process(Entity entity)
        {
            PlatformPhysics physicsCom;
            DirectionComponent directionCom;
            ActivityComponent activityCom;
            InputComponent inputCom;

            if (!EntityManager.GetComponent<PlatformPhysics, InputComponent>(entity, out physicsCom, out inputCom))
                return;

            var controller = SystemManager.World.Frame.Engine.GetController<ButtonController<PlatformAction>>(inputCom.Controller);
            if (controller == null)
                return;

            EntityManager.GetComponent<DirectionComponent, ActivityComponent>(entity, out directionCom, out activityCom);

            HandleInput(controller, physicsCom, directionCom, activityCom);

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

                //if (physicsCom.Pusher.TargetEntity != Entity.None)
                //    activityCom.Activity = "Pushing";
            }
        }

        private void HandleInput (ButtonController<PlatformAction> controller, PlatformPhysics physicsCom, DirectionComponent directionCom, ActivityComponent activityCom)
        {
            if (controller.ButtonHeld(PlatformAction.Left)) {
                physicsCom.AccelX = -FPMath.Abs(physicsCom.AccelX);
                physicsCom.AccelStateX = PlatformAccelState.Accelerate;
                if (directionCom != null)
                    directionCom.Direction = Rendering.Sprites.Direction.West;
            }
            else if (controller.ButtonHeld(PlatformAction.Right)) {
                physicsCom.AccelX = FPMath.Abs(physicsCom.AccelX);
                physicsCom.AccelStateX = PlatformAccelState.Accelerate;
                if (directionCom != null)
                    directionCom.Direction = Rendering.Sprites.Direction.East;
            }
            else {
                physicsCom.AccelStateX = PlatformAccelState.Decelerate;
            }

            if (controller.ButtonPressed(PlatformAction.Jump) && (activityCom.Activity == "Standing" || activityCom.Activity == "Walking"))
                physicsCom.VelocityY = (FPInt)(-8.5);

            if (controller.ButtonHeld(PlatformAction.Jump) && physicsCom.VelocityY < 0)
                physicsCom.AccelY = (FPInt)0.3;
            if (controller.ButtonReleased(PlatformAction.Jump) || physicsCom.VelocityY >= 0)
                physicsCom.AccelYMem.Reset();

            if (controller.ButtonHeld(PlatformAction.Run)) {
                if (activityCom.Activity == "Standing" || activityCom.Activity == "Walking") {
                    physicsCom.MaxVelocityX = physicsCom.MaxVelocityXMem.InitValue * 2;
                    physicsCom.MinVelocityX = physicsCom.MinVelocityXMem.InitValue * 2;
                }
            }
            else if (activityCom.Activity == "Standing" || activityCom.Activity == "Walking") {
                physicsCom.MaxVelocityXMem.Reset();
                physicsCom.MinVelocityXMem.Reset();
            }
        }
    }
}
