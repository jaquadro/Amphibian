using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering.Sprites
{
    public abstract class Sprite
    {
        private SpriteInfo _spriteData;

        public Sprite ()
        {
            _spriteData = new SpriteInfo();
            _spriteData.Opacity = 1;
        }

        #region Properties

        public SpriteInfo SpriteData
        {
            get { return _spriteData; }
        }

        public abstract float Left { get; }
        public abstract float Right { get; }
        public abstract float Top { get; }
        public abstract float Bottom { get; }

        public float Scale
        {
            get { return _spriteData.Scale; }
            set { _spriteData.Scale = MathHelper.Max(value, 0f); }
        }

        public float Rotation
        {
            get { return _spriteData.Rotation; }
            set { _spriteData.Rotation = MathHelper.WrapAngle(value); }
        }

        public virtual float Opacity
        {
            get { return _spriteData.Opacity; }
            set { _spriteData.Opacity = MathHelper.Clamp(value, 0f, 1f); }
        }

        protected SpriteEffects Effects
        {
            get { return _spriteData.Effects; }
            set { _spriteData.Effects = value; }
        }

        #endregion

        public abstract void Draw (SpriteBatch spriteBatch, PointFP position);

        public virtual void Update (AmphibianGameTime gameTime)
        {
        }
    }
}
