using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering.Sprites
{
    public abstract class Sprite
    {
        private SpriteInfo _spriteInfo;

        public Sprite ()
        {
            _spriteInfo = new SpriteInfo();
            _spriteInfo.Opacity = 1;
        }

        public Sprite (SpriteInfo spriteInfo)
        {
            _spriteInfo = spriteInfo;
        }

        #region Properties

        public SpriteInfo SpriteInfo
        {
            get { return _spriteInfo; }
        }

        public abstract float Left { get; }
        public abstract float Right { get; }
        public abstract float Top { get; }
        public abstract float Bottom { get; }

        public float Scale
        {
            get { return _spriteInfo.Scale; }
            set { _spriteInfo.Scale = MathHelper.Max(value, 0f); }
        }

        public float Rotation
        {
            get { return _spriteInfo.Rotation; }
            set { _spriteInfo.Rotation = MathHelper.WrapAngle(value); }
        }

        public virtual float Opacity
        {
            get { return _spriteInfo.Opacity; }
            set { _spriteInfo.Opacity = MathHelper.Clamp(value, 0f, 1f); }
        }

        protected SpriteEffects Effects
        {
            get { return _spriteInfo.Effects; }
            set { _spriteInfo.Effects = value; }
        }

        #endregion

        public abstract void Draw (SpriteBatch spriteBatch, PointFP position);

        public virtual void Update (AmphibianGameTime gameTime)
        {
        }
    }
}
