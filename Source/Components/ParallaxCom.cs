using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Geometry;
using Amphibian.EntitySystem;

namespace Amphibian.Components
{
    public sealed class ParallaxCom : IComponent
    {
        public FPInt ScrollCoefX;
        public FPInt ScrollCoefY;

    }
}
