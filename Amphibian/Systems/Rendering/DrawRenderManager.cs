using System;
using Amphibian.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private DrawBatch _drawBatch;

        public DrawRenderManager (DrawBatch drawBatch)
        {
            _drawBatch = drawBatch;
        }

        public DrawBatch DrawBatch
        {
            get { return _drawBatch; }
        }

        public void Begin ()
        {
            _drawBatch.Begin();
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

            _drawBatch.Begin(options.BlendState,
                options.SamplerState,
                options.DepthStencilState,
                options.RasterizerState);
        }

        public void End ()
        {
            _drawBatch.End();
        }
    }
}
