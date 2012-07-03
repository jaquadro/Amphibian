﻿using System;
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
    public class DirectionalAniamtedSpriteSpatial : Spatial
    {
        private class SpatialTypeRecord
        {
            public DirectionalAnimatedSpriteDefinition Definition;
            public DirectionalAnimatedSpriteInstance InstanceDefaults;
        }

        private static Dictionary<String, SpatialTypeRecord> _registered = new Dictionary<string, SpatialTypeRecord>();

        private SpatialTypeRecord _record;
        private DirectionalAnimatedSprite _sprite;

        public DirectionalAniamtedSpriteSpatial (String contentPath, EntityWorld world, ContentManager contentManager)
            : base(world)
        {
            if (!_registered.TryGetValue(contentPath, out _record))
                _record = Load(contentPath, contentManager);

            _sprite = _record.Definition.CreateSprite();
            _sprite.Scale = _record.InstanceDefaults.Transform.Scale;
            _sprite.Rotation = _record.InstanceDefaults.Transform.Rotation;
            _sprite.Opacity = _record.InstanceDefaults.Transform.Opacity;

            _sprite.CurrentDirection = (Direction)Enum.Parse(typeof(Direction), _record.InstanceDefaults.InitialDirection);
            _sprite.CurrentSequence.Start();
        }

        private SpatialTypeRecord Load (String contentPath, ContentManager contentManager)
        {
            SpatialTypeRecord record = new SpatialTypeRecord();

            using (FileStream xmlsrc = File.OpenRead(contentPath)) {
                XmlSerializer ser = new XmlSerializer(typeof(DirectionalAnimationSetElement));
                //ser.UnknownAttribute += (s, e) => { Console.WriteLine("Unknown Attr: " + e.Attr.Name); };
                //ser.UnknownElement += (s, e) => { Console.WriteLine("Uknown Elem: " + e.Element.Name); };
                //ser.UnknownNode += (s, e) => { Console.WriteLine("Unknown Node: " + e.Name); };
                TextReader reader = new StreamReader(xmlsrc);

                DirectionalAnimatedSpriteElement xmldef = ser.Deserialize(reader) as DirectionalAnimatedSpriteElement;
                record.Definition = xmldef.BuildDefinition(contentManager);
                record.InstanceDefaults = xmldef.Instance;
            }

            _registered[contentPath] = record;
            return record;
        }

        public override void Render (SpriteBatch spriteBatch, Entity e, Renderable position)
        {
            DirectionComponent directionCom = null;

            foreach (IComponent com in World.EntityManager.GetComponents(e)) {
                if (com is DirectionComponent) {
                    directionCom = com as DirectionComponent;
                    break;
                }
            }

            if (directionCom != null && directionCom.Direction != _sprite.CurrentDirection) {
                _sprite.CurrentDirection = directionCom.Direction;
                _sprite.CurrentSequence.Restart();
            }

            _sprite.Update(World.GameTime);

            _sprite.Draw(spriteBatch, new PointFP(position.RenderX, position.RenderY));
        }
    }
}