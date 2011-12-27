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

    public class FrogSpatial : Spatial
    {
        private AnimatedSprite _sequence;

        public FrogSpatial (EntityWorld world)
            : base(world)
        {
            _sequence = new AnimatedSprite();
        }

        public override void Initialize (ContentManager contentManager)
        {
            StaticSprite frame1 = new StaticSprite();
            StaticSprite frame2 = new StaticSprite();

            frame1.Load(contentManager, "Froggy", new Rectangle(0, 0, 34, 29));
            frame1.Origin = new Vector2(17, 28);
            frame2.Load(contentManager, "Froggy", new Rectangle(34, 0, 34, 29));
            frame2.Origin = new Vector2(18, 28);

            frame1.Scale = 2f;
            frame2.Scale = 2f;
            _sequence.Scale = 2f;

            _sequence.AddSprite(frame1, 0.5f);
            _sequence.AddSprite(frame2, 0.5f);

            _sequence.RepeatIndefinitely = true;
            _sequence.Start();
        }

        public override void Render (SpriteBatch spriteBatch, Renderable position)
        {
            _sequence.Update(World.GameTime);

            _sequence.Draw(spriteBatch, new PointFP(position.RenderX, position.RenderY));
        }
    }
}
