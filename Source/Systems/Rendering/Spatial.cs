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

        public abstract void Initialize (ContentManager contentManager);

        public abstract void Render (SpriteBatch spriteBatch, Renderable position);
    }

    public class PotSpatial : Spatial
    {
        private StaticSprite _sprite;

        public PotSpatial (EntityWorld world)
            : base(world)
        {
            _sprite = new StaticSprite();
        }

        public override void Initialize (ContentManager contentManager)
        {
            _sprite.Load(contentManager, "pots", new Rectangle(0, 0, 32, 32));
            _sprite.Origin = new Vector2(16, 31);
            _sprite.Scale = 2f;
        }

        public override void Render (SpriteBatch spriteBatch, Renderable position)
        {
            _sprite.Draw(spriteBatch, new PointFP(position.RenderX, position.RenderY));
        }
    }
}
