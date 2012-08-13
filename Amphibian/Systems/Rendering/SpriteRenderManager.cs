using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering
{
    public class SpriteRenderManagerOptions : IRenderManagerOptions
    {
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public Effect Effect { get; set; }
    }

    public class SpriteRenderManager : IRenderManager
    {
        private SpriteBatch _spriteBatch;

        public SpriteRenderManager (SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public void Begin ()
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null);
        }

        public void Begin (IRenderManagerOptions renderOptions)
        {
            Begin(renderOptions, Matrix.Identity);
        }

        public void Begin (IRenderManagerOptions renderOptions, Matrix transformMatrix)
        {
            SpriteRenderManagerOptions options = renderOptions as SpriteRenderManagerOptions;
            if (options == null)
                throw new ArgumentException("renderOptions must be of type SpriteRenderManagerOptions");

            _spriteBatch.Begin(SpriteSortMode.Deferred,
                options.BlendState,
                options.SamplerState,
                options.DepthStencilState,
                options.RasterizerState,
                options.Effect,
                transformMatrix);
        }

        public void End ()
        {
            _spriteBatch.End();
        }
    }
}
