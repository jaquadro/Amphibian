using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Geometry;
using Amphibian.Systems;
using Amphibian.EntitySystem;
using Amphibian.Systems.Rendering;

namespace Amphibian.Components
{
    public class Renderable : IComponent
    {
        public int LayerIndex = 1000;
        public FPInt RenderX;
        public FPInt RenderY;
        public SpatialRef SpatialRef;
    }
}
