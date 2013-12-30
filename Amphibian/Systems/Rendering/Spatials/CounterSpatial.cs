using System.Collections.Generic;
using System.IO;
using System.Xml;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Amphibian.Geometry;
using Amphibian.Systems.Rendering.Sprites;
using Amphibian.Systems.Rendering.Sprites.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering.Spatials
{
    public class CounterSpatial : SpriteSpatial, IRenderEffects
    {
        private class SpatialTypeRecord
        {
            public CounterDefinition Definition;
            public ICounterInstance InstanceDefaults;
        }

        private static Dictionary<string, SpatialTypeRecord> _registered = new Dictionary<string, SpatialTypeRecord>();

        private SpatialTypeRecord _record;
        private Counter _counter;

        public CounterSpatial (string contentPath, EntityWorld world, ContentManager contentManager)
            : base(world)
        {
            if (!_registered.TryGetValue(contentPath, out _record))
                _record = Load(contentPath, contentManager);

            _counter = _record.Definition.CreateCounter();
            if (_record.InstanceDefaults != null && _record.InstanceDefaults.Transform != null) {
                _counter.BlendColor = _record.InstanceDefaults.Transform.BlendColor;
                _counter.Scale = _record.InstanceDefaults.Transform.Scale;
                _counter.Rotation = _record.InstanceDefaults.Transform.Rotation;
                _counter.Opacity = _record.InstanceDefaults.Transform.Opacity;
            }
        }

        private SpatialTypeRecord Load (string contentPath, ContentManager contentManager)
        {
            SpatialTypeRecord record = new SpatialTypeRecord();

            using (Stream contentStream = TitleContainer.OpenStream(contentPath)) {
                using (XmlReader reader = XmlReader.Create(contentStream)) {
                    XmlCounterElement xmldef = new XmlCounterElement();
                    xmldef.ReadXml(reader);

                    record.Definition = xmldef.BuildDefinition(contentManager);
                    record.InstanceDefaults = xmldef.Instance;
                }
            }

            _registered[contentPath] = record;
            return record;
        }

        public override void Render (SpriteBatch spriteBatch, Entity e, Renderable position)
        {
            _counter.Update(World.GameTime);

            _counter.Draw(spriteBatch, new PointFP(position.RenderX, position.RenderY));
        }

        public int Value
        {
            get { return _counter.Value; }
            set { _counter.Value = value; }
        }

        #region IRenderEffects

        public float Scale
        {
            get { return _counter.Scale; }
            set { _counter.Scale = value; }
        }

        public float Rotation
        {
            get { return _counter.Rotation; }
            set { _counter.Rotation = value; }
        }

        public float Opacity
        {
            get { return _counter.Opacity; }
            set { _counter.Opacity = value; }
        }

        public Color BlendColor
        {
            get { return _counter.BlendColor; }
            set { _counter.BlendColor = value; }
        }

        #endregion
    }
}
