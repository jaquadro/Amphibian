using System;
using Amphibian.EntitySystem;
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
        public SpriteRenderManager (EntityWorld world)
            : this(world, new SpriteBatch(world.Frame.Engine.GraphicsDevice))
        { }

        public SpriteRenderManager (EntityWorld world, SpriteBatch spriteBatch)
        {
            World = world;
            SpriteBatch = spriteBatch;
        }

        public EntityWorld World { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public void Begin ()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null);
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

            SpriteBatch.Begin(SpriteSortMode.Deferred,
                options.BlendState,
                options.SamplerState,
                options.DepthStencilState,
                options.RasterizerState,
                options.Effect,
                transformMatrix);
        }

        public void End ()
        {
            SpriteBatch.End();
        }
    }
}
