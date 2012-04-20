using System;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class StaticSprite : Sprite
    {
        private StaticSpriteDefinition _definition;

        public StaticSprite (StaticSpriteDefinition definition)
        {
            _definition = definition;
        }

        #region Properties

        public StaticSpriteDefinition Definition
        {
            get { return Definition; }
        }

        public override float Left
        {
            get { return _definition.Left; }
        }

        public override float Right
        {
            get { return _definition.Right; }
        }

        public override float Top
        {
            get { return _definition.Top; }
        }

        public override float Bottom
        {
            get { return _definition.Bottom; }
        }

        #endregion

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            _definition.Draw(spriteBatch, position, SpriteData);
        }
    }
}
