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
using Microsoft.Xna.Framework;
using System.IO;

namespace Amphibian.Systems.Rendering.Spatials
{
    public class DirectionalAniamtedSpriteSpatial : SpriteSpatial, IRenderEffects
    {
        private class SpatialTypeRecord
        {
            public DirectionalAnimatedSpriteDefinition Definition;
            public IDirectionalAnimatedSpriteInstance InstanceDefaults;
        }

        private static Dictionary<String, SpatialTypeRecord> _registered = new Dictionary<string, SpatialTypeRecord>();

        private SpatialTypeRecord _record;
        private DirectionalAnimatedSprite _sprite;

        public DirectionalAniamtedSpriteSpatial (String contentPath, ContentManager contentManager)
        {
            if (!_registered.TryGetValue(contentPath, out _record))
                _record = Load(contentPath, contentManager);

            _sprite = _record.Definition.CreateSprite();
            _sprite.Scale = _record.InstanceDefaults.Transform.Scale;
            _sprite.Rotation = _record.InstanceDefaults.Transform.Rotation;
            _sprite.Opacity = _record.InstanceDefaults.Transform.Opacity;

            _sprite.CurrentDirection = (Direction)Enum.Parse(typeof(Direction), _record.InstanceDefaults.InitialDirection, false);
            _sprite.CurrentSequence.Start();
        }

        private SpatialTypeRecord Load (String contentPath, ContentManager contentManager)
        {
            SpatialTypeRecord record = new SpatialTypeRecord();

            using (Stream contentStream = TitleContainer.OpenStream(contentPath)) {
                using (XmlReader reader = XmlReader.Create(contentStream)) {
                    XmlDirectionalAnimatedSpriteElement xmldef = new XmlDirectionalAnimatedSpriteElement();
                    xmldef.ReadXml(reader);

                    record.Definition = xmldef.BuildDefinition(contentManager);
                    record.InstanceDefaults = xmldef.Instance;
                }
            }

            _registered[contentPath] = record;
            return record;
        }

        public override void Render (SpriteBatch spriteBatch, EntityWorld world, Entity e, Renderable position)
        {
            DirectionComponent directionCom = null;

            foreach (IComponent com in world.EntityManager.GetComponents(e)) {
                if (com is DirectionComponent) {
                    directionCom = com as DirectionComponent;
                    break;
                }
            }

            if (directionCom != null && directionCom.Direction != _sprite.CurrentDirection) {
                _sprite.CurrentDirection = directionCom.Direction;
                _sprite.CurrentSequence.Restart();
            }

            _sprite.Update(world.GameTime);

            _sprite.Draw(spriteBatch, new PointFP(position.RenderX, position.RenderY));
        }

        #region IRenderEffects

        public float Scale
        {
            get { return _sprite.Scale; }
            set { _sprite.Scale = value; }
        }

        public float Rotation
        {
            get { return _sprite.Rotation; }
            set { _sprite.Rotation = value; }
        }

        public float Opacity
        {
            get { return _sprite.Opacity; }
            set { _sprite.Opacity = value; }
        }

        public Color BlendColor
        {
            get { return _sprite.BlendColor; }
            set { _sprite.BlendColor = value; }
        }

        #endregion
    }
}
