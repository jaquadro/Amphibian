using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Amphibian.Components;
using Amphibian.Geometry;

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

        public abstract void Render (SpriteBatch spriteBatch, Entity entity, Renderable position);

        public virtual bool InBounds (Rectangle bounds)
        {
            return true;
        }
    }
}
