using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Spine;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.EntitySystem;

namespace Amphibian.Systems.Rendering
{
    public class SpineRenderManagerOptions : IRenderManagerOptions
    {
        public BlendState BlendState { get; set; }
    }

    public class SpineRenderManager : IRenderManager
    {
        public SpineRenderManager (EntityWorld world)
            : this(world, new SkeletonRenderer(world.Frame.Engine.GraphicsDevice))
        { }

        public SpineRenderManager (EntityWorld world, SkeletonRenderer skeletonRenderer)
        {
            World = world;
            SkeletonRenderer = skeletonRenderer;
        }

        public EntityWorld World { get; private set; }
        public SkeletonRenderer SkeletonRenderer { get; private set; }

        public void Begin ()
        {
            SkeletonRenderer.Begin();
        }

        public void Begin (IRenderManagerOptions renderOptions)
        {
            Begin(renderOptions, Matrix.Identity);
        }

        public void Begin (IRenderManagerOptions renderOptions, Matrix transformMatrix)
        {
            SpineRenderManagerOptions options = renderOptions as SpineRenderManagerOptions;
            if (options == null)
                throw new ArgumentException("renderOptions must be of type SpineRenderManagerOptions");

            if (options.BlendState != null)
                SkeletonRenderer.BlendState = options.BlendState;
            SkeletonRenderer.Begin();
        }

        public void End ()
        {
            SkeletonRenderer.End();
        }
    }
}
