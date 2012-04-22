using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Amphibian.Geometry;
using Amphibian.Systems.Rendering.Sprites;
using Amphibian.Systems.Rendering.Sprites.Xml;

namespace Amphibian.Systems.Rendering.Spatials
{
    public class StaticSpriteSpatial : Spatial
    {
        private class SpatialTypeRecord
        {
            public StaticSpriteDefinition Definition;
            public StaticSpriteInstance InstanceDefaults;
        }

        private static Dictionary<String, SpatialTypeRecord> _registered = new Dictionary<string, SpatialTypeRecord>();

        private SpatialTypeRecord _record;
        private StaticSprite _sprite;

        public StaticSpriteSpatial (String contentPath, EntityWorld world, ContentManager contentManager)
            : base(world)
        {
            if (!_registered.TryGetValue(contentPath, out _record))
                _record = Load(contentPath, contentManager);

            _sprite = _record.Definition.CreateSprite();
            _sprite.Scale = _record.InstanceDefaults.Transform.Scale;
            _sprite.Rotation = _record.InstanceDefaults.Transform.Rotation;
            _sprite.Opacity = _record.InstanceDefaults.Transform.Opacity;
        }

        private SpatialTypeRecord Load (String contentPath, ContentManager contentManager)
        {
            SpatialTypeRecord record = new SpatialTypeRecord();

            using (FileStream xmlsrc = File.OpenRead(contentPath)) {
                XmlSerializer ser = new XmlSerializer(typeof(StaticSpriteElement));
                //ser.UnknownAttribute += (s, e) => { Console.WriteLine("Unknown Attr: " + e.Attr.Name); };
                //ser.UnknownElement += (s, e) => { Console.WriteLine("Uknown Elem: " + e.Element.Name); };
                //ser.UnknownNode += (s, e) => { Console.WriteLine("Unknown Node: " + e.Name); };
                TextReader reader = new StreamReader(xmlsrc);

                StaticSpriteElement xmldef = ser.Deserialize(reader) as StaticSpriteElement;
                record.Definition = xmldef.BuildDefinition(contentManager);
                record.InstanceDefaults = xmldef.Instance;
            }

            _registered[contentPath] = record;
            return record;
        }

        public override void Render (SpriteBatch spriteBatch, Entity e, Renderable position)
        {
            _sprite.Draw(spriteBatch, new PointFP(position.RenderX, position.RenderY));
        }
    }
}
