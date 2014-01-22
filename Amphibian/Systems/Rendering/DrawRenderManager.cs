using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LilyPath;
using Amphibian.EntitySystem;

namespace Amphibian.Systems.Rendering
{
    public class DrawRenderManagerOptions : IRenderManagerOptions
    {
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }
    }

    public class DrawRenderManager : IRenderManager
    {
        public DrawRenderManager (EntityWorld world)
            : this(world, new DrawBatch(world.Frame.Engine.GraphicsDevice))
        { }

        public DrawRenderManager (EntityWorld world, DrawBatch drawBatch)
        {
            World = world;
            DrawBatch = drawBatch;
        }

        public EntityWorld World { get; private set; }
        public DrawBatch DrawBatch { get; private set; }

        public void Begin ()
        {
            DrawBatch.Begin();
        }

        public void Begin (IRenderManagerOptions renderOptions)
        {
            Begin(renderOptions, Matrix.Identity);
        }

        public void Begin (IRenderManagerOptions renderOptions, Matrix transformMatrix)
        {
            DrawRenderManagerOptions options = renderOptions as DrawRenderManagerOptions;
            if (options == null)
                throw new ArgumentException("renderOptions must be of type DrawRenderManagerOptions");

            DrawBatch.Begin(DrawSortMode.Deferred, 
                options.BlendState,
                options.SamplerState,
                options.DepthStencilState,
                options.RasterizerState);
        }

        public void End ()
        {
            DrawBatch.End();
        }
    }
}
