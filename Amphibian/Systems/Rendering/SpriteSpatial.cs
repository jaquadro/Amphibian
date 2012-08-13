using System;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering
{
    public abstract class SpriteSpatial : Spatial
    {
        public SpriteSpatial (EntityWorld world)
            : base(world)
        { }

        public override void Render (IRenderManager renderManager, Entity entity, Renderable position)
        {
            SpriteRenderManager spriteRenderManager = renderManager as SpriteRenderManager;
            if (spriteRenderManager == null)
                throw new ArgumentException("renderManager must be of type SpriteRenderManager");

            Render(spriteRenderManager.SpriteBatch, entity, position);
        }

        public abstract void Render (SpriteBatch spriteBatch, Entity entity, Renderable position);
    }
}
