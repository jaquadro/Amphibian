using System;
using System.Collections.Generic;
using System.Xml;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Amphibian.Geometry;
using Amphibian.Systems.Rendering.Sprites;
using Amphibian.Systems.Rendering.Sprites.Xml;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering.Spatials
{
    public class AnimatedSpriteSpatial : Spatial
    {
        private class SpatialTypeRecord
        {
            public AnimatedSpriteDefinition Definition;
            public IAnimatedSpriteInstance InstanceDefaults;
        }

        private static Dictionary<String, SpatialTypeRecord> _registered = new Dictionary<string, SpatialTypeRecord>();

        private SpatialTypeRecord _record;
        private AnimatedSprite _sprite;

        public AnimatedSpriteSpatial (String contentPath, EntityWorld world, ContentManager contentManager)
            : base(world)
        {
            if (!_registered.TryGetValue(contentPath, out _record))
                _record = Load(contentPath, contentManager);

            _sprite = _record.Definition.CreateSprite();
            _sprite.Scale = _record.InstanceDefaults.Transform.Scale;
            _sprite.Rotation = _record.InstanceDefaults.Transform.Rotation;
            _sprite.Opacity = _record.InstanceDefaults.Transform.Opacity;

            _sprite.Start();
        }

        private SpatialTypeRecord Load (String contentPath, ContentManager contentManager)
        {
            SpatialTypeRecord record = new SpatialTypeRecord();

            using (XmlReader reader = XmlReader.Create(contentPath)) {
                XmlAnimatedSpriteElement xmldef = new XmlAnimatedSpriteElement();
                xmldef.ReadXml(reader);

                record.Definition = xmldef.BuildDefinition(contentManager);
                record.InstanceDefaults = xmldef.Instance;
            }

            _registered[contentPath] = record;
            return record;
        }

        public override void Render (SpriteBatch spriteBatch, Entity e, Renderable position)
        {
            _sprite.Update(World.GameTime);

            _sprite.Draw(spriteBatch, new PointFP(position.RenderX, position.RenderY));
        }
    }
}
