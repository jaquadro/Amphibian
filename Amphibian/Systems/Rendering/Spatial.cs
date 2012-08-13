using Amphibian.Components;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering
{
    public abstract class Spatial
    {
        private EntityWorld _world;

        public Spatial (EntityWorld world)
        {
            _world = world;
        }

        public EntityWorld World
        {
            get { return _world; }
        }

        public virtual void Initialize (ContentManager contentManager) { }

        public virtual void Update () { }

        public abstract void Render (IRenderManager renderManager, Entity entity, Renderable position);

        public virtual bool InBounds (Rectangle bounds)
        {
            return true;
        }
    }
}
