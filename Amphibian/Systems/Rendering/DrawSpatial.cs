using System;
using Amphibian.Components;
using Amphibian.EntitySystem;
using LilyPath;

namespace Amphibian.Systems.Rendering
{
    public abstract class DrawSpatial : Spatial
    {
        public override Type RenderManagerType
        {
            get { return typeof(DrawRenderManager); }
        }

        public override void Render (IRenderManager renderManager, Entity entity, Renderable position)
        {
            DrawRenderManager drawRenderManager = renderManager as DrawRenderManager;
            if (drawRenderManager == null)
                throw new ArgumentException("renderManager must be of type DrawRenderManager");

            Render(drawRenderManager.DrawBatch, drawRenderManager.World, entity, position);
        }

        public abstract void Render (DrawBatch drawBatch, EntityWorld world, Entity entity, Renderable position);
    }
}
