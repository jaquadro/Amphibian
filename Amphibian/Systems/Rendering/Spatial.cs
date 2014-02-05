using System;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering
{
    public abstract class Spatial
    {
        public virtual void Initialize (ContentManager contentManager) { }

        public virtual void Update () { }

        public abstract Type RenderManagerType { get; }

        public abstract void Render (IRenderManager renderManager, Entity entity, Renderable position);

        public virtual bool InBounds (Rectangle bounds)
        {
            return true;
        }
    }
}
