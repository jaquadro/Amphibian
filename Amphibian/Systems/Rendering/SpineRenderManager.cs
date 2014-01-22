using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Spine;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering
{
    public class SpineRenderManagerOptions : IRenderManagerOptions
    {
        public BlendState BlendState { get; set; }
    }

    public class SpineRenderManager : IRenderManager
    {
        private SkeletonRenderer _skeletonRenderer;

        public SpineRenderManager (GraphicsDevice device)
        {
            _skeletonRenderer = new SkeletonRenderer(device);
        }

        public SkeletonRenderer SkeletonRenderer
        {
            get { return _skeletonRenderer; }
        }

        public void Begin ()
        {
            _skeletonRenderer.Begin();
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
                _skeletonRenderer.BlendState = options.BlendState;
            _skeletonRenderer.Begin();
        }

        public void End ()
        {
            _skeletonRenderer.End();
        }
    }
}
