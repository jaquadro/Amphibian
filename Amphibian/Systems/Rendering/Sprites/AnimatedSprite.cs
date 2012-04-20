using System;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class AnimatedSprite : Sprite
    {
        private AnimatedSpriteDefinition _definition;
        private AnimationInfo _animData;

        public AnimatedSprite (AnimatedSpriteDefinition definition)
        {
            _definition = definition;
            _animData = new AnimationInfo();
        }

        public AnimatedSprite (AnimatedSpriteDefinition definition, SpriteInfo spriteInfo)
            : base(spriteInfo)
        {
            _definition = definition;
            _animData = new AnimationInfo();
        }

        public AnimatedSprite (AnimatedSpriteDefinition definition, SpriteInfo spriteInfo, AnimationInfo animInfo)
            : base(spriteInfo)
        {
            _definition = definition;
        }

        #region Properties

        public AnimatedSpriteDefinition Definition
        {
            get { return _definition; }
        }

        public AnimationInfo AnimationData
        {
            get { return _animData; }
        }

        public override float Left
        {
            get { return _definition.Frames.Count > 0 ? _definition.Frames[_animData.CurrentFrameIndex].Frame.Left : 0; }
        }

        public override float Right
        {
            get { return _definition.Frames.Count > 0 ? _definition.Frames[_animData.CurrentFrameIndex].Frame.Right : 0; }
        }

        public override float Top
        {
            get { return _definition.Frames.Count > 0 ? _definition.Frames[_animData.CurrentFrameIndex].Frame.Top : 0; }
        }

        public override float Bottom
        {
            get { return _definition.Frames.Count > 0 ? _definition.Frames[_animData.CurrentFrameIndex].Frame.Bottom : 0; }
        }

        public int CurrentIndex
        {
            get { return (_definition.Frames.Count == 0) ? -1 : _animData.CurrentFrameIndex; }
        }

        // Relevant?
        /*public StaticSpriteDefinition CurrentSprite
        {
            get
            {
                if (_definition.Frames.Count == 0) {
                    return null;
                }
                return _definition.Frames[_animData.CurrentFrameIndex].Frame;
            }
        }*/

        public bool IsAnimating
        {
            get { return (_animData.Options & AnimationOptions.Animating) == AnimationOptions.Animating; }
        }

        

        public int RepeatCount
        {
            get { return _animData.LoopCount; }
        }

        

        public RefClock ReferenceClock
        {
            get { return _animData.RefClock; }
            set { _animData.RefClock = value; }
        }

        #endregion

        public override void Update (AmphibianGameTime gameTime)
        {
            if (_animData.RefClock != null) {
                if (_animData.RefClock.Triggered) {
                    if (_animData.Delay == 0)
                        AdvanceFrame();
                    else {
                        _animData.Delay--;
                        if (_animData.Delay == 0)
                            Start();
                    }
                }
            }
            else if (IsAnimating) {
                _animData.TimeAccum += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_animData.TimeAccum > _definition.Frames[_animData.CurrentFrameIndex].Duration) {
                    _animData.TimeAccum -= _definition.Frames[_animData.CurrentFrameIndex].Duration;
                    AdvanceFrame();
                }
            }
        }

        internal void AdvanceFrame ()
        {
            if (IsAnimating) {
                _animData.CurrentFrameIndex++;

                if (_animData.CurrentFrameIndex >= _definition.Frames.Count) {
                    _animData.LoopCount++;

                    if (_definition.RepeatIndefinitely || _animData.LoopCount < _definition.RepeatLimit) {
                        _animData.CurrentFrameIndex = _definition.RepeatIndex;
                    }
                    else {
                        _animData.CurrentFrameIndex--;
                        Stop();
                    }
                }
            }
        }

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            _definition.Draw(spriteBatch, position, _animData, SpriteInfo);
        }

        public void Start ()
        {
            _animData.Options |= AnimationOptions.Animating;
            if (!_definition.RepeatIndefinitely && _animData.LoopCount == _definition.RepeatLimit) {
                Restart();
            }
        }

        public void DelayStart (int delay)
        {
            _animData.Delay = delay;
        }

        public void Restart ()
        {
            _animData.Options |= AnimationOptions.Animating;
            _animData.CurrentFrameIndex = 0;
            _animData.LoopCount = 0;
        }

        public void Stop ()
        {
            _animData.Options &= ~AnimationOptions.Animating;
        }
    }
}
