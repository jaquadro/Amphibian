using System.Collections.Generic;
using Amphibian.Geometry;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class Counter : Sprite
    {
        private CounterDefinition _definition;

        private int _value;
        private List<int> _cachedDigits;
        private PointFP _cachedSize;

        public Counter (CounterDefinition definition)
        {
            _definition = definition;
            _cachedDigits = new List<int>();
        }

        public Counter (CounterDefinition definition, SpriteInfo spriteInfo)
            : base(spriteInfo)
        {
            _definition = definition;
        }

        public CounterDefinition Definition
        {
            get { return _definition; }
        }

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value) {
                    _value = value;

                    _definition.ParseValue(value, _cachedDigits);
                    _cachedSize = _definition.CalculateSize(_cachedDigits);
                }
            }
        }

        public override float Left
        {
            get { return 0; }
        }

        public override float Top
        {
            get { return 0; }
        }

        public override float Right
        {
            get { return (float)_cachedSize.X; }
        }

        public override float Bottom
        {
            get { return (float)_cachedSize.Y; }
        }

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            _definition.Draw(spriteBatch, position, SpriteInfo, _cachedDigits);
        }
    }
}
