using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Geometry;

namespace Amphibian.Components
{
    public sealed class PlatformPushable : IComponent
    {
        public float PushDelay { get; set; }
        public float PushSpeed { get; set; }
        public FPInt PushInterval { get; set; }
        public Entity SourceEntity { get; set; }
    }

    public enum PlatformPushDirection
    {
        Left,
        Right,
    }

    public sealed class PlatformPusher : IComponent
    {
        public float DelayExpended { get; set; }
        public PlatformPushDirection TargetDirection { get; set; }
        public Entity TargetEntity { get; set; }
    }
}
