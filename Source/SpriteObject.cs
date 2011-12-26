using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amphibian.Geometry;

// Frame Objects have...
// - Sprite
// - Collision mask
// - Behaviors
// - State (role into behaviors?  separate flags?)

namespace Amphibian
{
    public abstract class SpriteObject : GameObject
    {
        protected SpriteObject ()
            : base()
        {
        }

        #region Properties

        public Sprite Sprite
        {
            get { return GetSprite(); }
        }
 
        #endregion

        protected abstract Sprite GetSprite ();

        public override void Draw ()
        {
            base.Draw();

            if (this.Sprite != null) {
                this.Sprite.Draw(Parent.Engine.SpriteBatch, RenderPosition);
            }
        }
    }

    public class StaticSpriteObject : SpriteObject
    {
        protected StaticSprite _sprite;

        public StaticSpriteObject ()
            : base()
        {
            _sprite = new StaticSprite();
        }

        #region Properties

        public new StaticSprite Sprite
        {
            get { return _sprite; }
        }

        #endregion

        protected override Sprite GetSprite ()
        {
            return _sprite;
        }
    }

    public class AnimatedSpriteObject : SpriteObject
    {
        protected AnimatedSprite _sequence;

        public AnimatedSpriteObject ()
            : base()
        {
            _sequence = new AnimatedSprite();
        }

        #region Properties

        public new StaticSprite Sprite
        {
            get 
            {
                return _sequence.CurrentSprite;
            }
        }

        public AnimatedSprite SpriteSequence
        {
            get { return _sequence; }
        }

        #endregion

        protected override Sprite GetSprite ()
        {
            return _sequence.CurrentSprite;
        }
    }
}
