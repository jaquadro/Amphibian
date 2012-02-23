using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Amphibian.Geometry;

// Properties of a sprite
// - Image
// - Mask (on some sprites) -- or defer to object?
// - Scale
// - Rotation
// - Other effects?  (e.g. transp)

namespace Amphibian.Systems.Rendering
{
    public abstract class Sprite
    {
        private float _scale;
        private float _rotation;
        private float _transp;
        private SpriteEffects _effects;

        protected Sprite ()
        {
            _scale = 1f;
            _rotation = 0f;
            _transp = 1f;
            _effects = SpriteEffects.None;
        }

        #region Properties

        public abstract float Left { get; }
        public abstract float Right { get; }
        public abstract float Top { get; }
        public abstract float Bottom { get; }

        public float Scale
        {
            get { return _scale; }
            set { _scale = MathHelper.Max(value, 0f); }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = MathHelper.WrapAngle(value); }
        }

        public virtual float Opacity
        {
            get { return _transp; }
            set { _transp = MathHelper.Clamp(value, 0f, 1f); }
        }

        protected SpriteEffects Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }

        public bool IsFlippedHorizontally
        {
            get { return (_effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally; }
            set
            {
                if (value) {
                    _effects = _effects | SpriteEffects.FlipHorizontally;
                }
                else {
                    _effects = _effects & ~SpriteEffects.FlipHorizontally;
                }
            }
        }

        public bool IsFlippedVertically
        {
            get { return (_effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically; }
            set
            {
                if (value) {
                    _effects = _effects | SpriteEffects.FlipVertically;
                }
                else {
                    _effects = _effects & ~SpriteEffects.FlipVertically;
                }
            }
        }

        #endregion

        public virtual void Update (AmphibianGameTime gameTime)
        {
        }

        public abstract void Draw (SpriteBatch spriteBatch, PointFP position);

        public void FlipHorizontally ()
        {
            if (IsFlippedHorizontally) {
                IsFlippedHorizontally = false;
            }
            else {
                IsFlippedHorizontally = true;
            }
        }

        public void FlipVertically ()
        {
            if (IsFlippedVertically) {
                IsFlippedVertically = false;
            }
            else {
                IsFlippedVertically = true;
            }
        }
    }
}
