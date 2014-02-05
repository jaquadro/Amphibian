using System;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering
{
    public abstract class SpriteSpatial : Spatial
    {
        public override Type RenderManagerType
        {
            get { return typeof(SpriteRenderManager); }
        }

        public override void Render (IRenderManager renderManager, Entity entity, Renderable position)
        {
            SpriteRenderManager spriteRenderManager = renderManager as SpriteRenderManager;
            if (spriteRenderManager == null)
                throw new ArgumentException("renderManager must be of type SpriteRenderManager");

            Render(spriteRenderManager.SpriteBatch, spriteRenderManager.World, entity, position);
        }

        public abstract void Render (SpriteBatch spriteBatch, EntityWorld world, Entity entity, Renderable position);
    }
}
