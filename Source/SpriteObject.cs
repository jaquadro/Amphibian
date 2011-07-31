using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

// Frame Objects have...
// - Sprite
// - Collision mask
// - Behaviors
// - State (role into behaviors?  separate flags?)

namespace Amphibian
{
    public class GameObject : Component
    {
        protected Vector2 _position;
        protected List<Behavior> _behaviors;

        public GameObject ()
            : base()
        {
            _behaviors = new List<Behavior>();
        }

        #region Properties

        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        #endregion

        public void AddBehavior (Behavior behavior)
        {
            Type bType = behavior.GetType();

            foreach (Behavior b in _behaviors) {
                if (bType == _behaviors.GetType()) {
                    return;
                }
            }

            _behaviors.Add(behavior);
        }

        public void RemoveBehavior (Behavior behavior)
        {
            RemoveBehavior(behavior.GetType());
        }

        public void RemoveBehavior (Type behaviorType)
        {
            foreach (Behavior b in _behaviors) {
                if (behaviorType == b.GetType()) {
                    _behaviors.Remove(b);
                    return;
                }
            }
        }

        public override void Update ()
        {
            base.Update();

            foreach (Behavior behavior in _behaviors) {
                behavior.Execute();
            }
        }
    }

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
                this.Sprite.Draw(Parent.Engine.SpriteBatch, _position);
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

        #endregion

        protected override Sprite GetSprite ()
        {
            return _sequence.CurrentSprite;
        }
    }
}
