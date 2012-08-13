using System;
using Amphibian.Components;
using Amphibian.Drawing;
using Amphibian.EntitySystem;

namespace Amphibian.Systems.Rendering
{
    public abstract class DrawSpatial : Spatial
    {
        public DrawSpatial (EntityWorld world)
            : base(world)
        { }

        public override void Render (IRenderManager renderManager, Entity entity, Renderable position)
        {
            DrawRenderManager drawRenderManager = renderManager as DrawRenderManager;
            if (drawRenderManager == null)
                throw new ArgumentException("renderManager must be of type DrawRenderManager");

            Render(drawRenderManager.DrawBatch, entity, position);
        }

        public abstract void Render (DrawBatch drawBatch, Entity entity, Renderable position);
    }
}
