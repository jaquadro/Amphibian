using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Geometry;
using Amphibian.EntitySystem;
using Amphibian.Utility;

namespace Amphibian.Components
{
    public enum PlatformAccelState
    {
        Fixed,
        Accelerate,
        Decelerate
    }

    public enum PlatformState
    {
        NonSolid,
        BackgroundSolid,
        Solid,
        Platform,
    }

    public sealed class PlatformPhysics : IComponent
    {
        public FPInt AccelX;
        public FPInt AccelY;

        public FPInt DecelX;
        public FPInt DecelY;

        public FPInt MaxVelocityX;
        public FPInt MaxVelocityY;

        public FPInt MinVelocityX;
        public FPInt MinVelocityY;

        public FPInt VelocityX;
        public FPInt VelocityY;

        public PlatformAccelState AccelStateX;
        public PlatformAccelState AccelStateY;

        public PlatformState State = PlatformState.BackgroundSolid;

        /// <summary>
        /// Entities that should be affected by changes to this component
        /// </summary>
        public UnorderedList<Entity> DependentEntities = new UnorderedList<Entity>(2);

        public PlatformPusher Pusher = null;
        public PlatformPushable Pushable = null;
    }

    public class Physics : IComponent
    {
        public FPInt AccelX;
        public FPInt AccelY;

        public FPInt DecelX;
        public FPInt DecelY;

        public FPInt MaxVelocityX;
        public FPInt MaxVelocityY;

        public FPInt MinVelocityX;
        public FPInt MinVelocityY;

        public FPInt VelocityX;
        public FPInt VelocityY;

        public PlatformAccelState AccelStateX;
        public PlatformAccelState AccelStateY;
    }
}
