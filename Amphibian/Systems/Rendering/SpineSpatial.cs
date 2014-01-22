using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Components;
using Spine;

namespace Amphibian.Systems.Rendering
{
    public abstract class SpineSpatial : Spatial
    {
        public SpineSpatial (EntityWorld world)
            : base(world)
        { }

        public override void Render (IRenderManager renderManager, Entity entity, Renderable position)
        {
            SpineRenderManager spineRenderManager = renderManager as SpineRenderManager;
            if (spineRenderManager == null)
                throw new ArgumentException("renderManager must be of type SpineRenderManager");

            Render(spineRenderManager.SkeletonRenderer, entity, position);
        }

        public abstract void Render (SkeletonRenderer skeletonRenderer, Entity entity, Renderable position);
    }
}
