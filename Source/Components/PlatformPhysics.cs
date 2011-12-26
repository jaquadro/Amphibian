using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Geometry;
using Amphibian.EntitySystem;

namespace Amphibian.Components
{
    public enum PlatformAccelStateC
    {
        Fixed,
        Accelerate,
        Decelerate
    }

    public class PlatformPhysics : IComponent
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

        public PlatformAccelStateC AccelStateX;
        public PlatformAccelStateC AccelStateY;
    }
}
