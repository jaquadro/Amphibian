using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering
{
    

    [Flags]
    public enum AnimationOptions
    {
        None = 0,
        Animating = 0x1,
        Infinite = 0x2,
    }

    public class AnimatedSprite : Sprite
    {
        private struct SpriteFrame
        {
            public StaticSprite Frame;
            public float Duration;

            public SpriteFrame (StaticSprite frame, float duration)
            {
                Frame = frame;
                Duration = duration;
            }
        }

        private List<SpriteFrame> _frames;

        private AnimationOptions _animOptions;

        private int _index;
        private int _restartAt;
        private int _loopCount;
        private int _loopLimit = 1;
        private int _delay;

        private float _timeAccum;
        private RefClock _refClock;

        public AnimatedSprite ()
            : base()
        {
            _frames = new List<SpriteFrame>();
        }

        #region Properties

        public override float Opacity
        {
            get { return base.Opacity; }
            set
            {
                base.Opacity = value;
                foreach (SpriteFrame frame in _frames) {
                    frame.Frame.Opacity = value;
                }
            }
        }

        public override float Left
        {
            get { return _frames.Count > 0 ? _frames[_index].Frame.Left : 0; }
        }

        public override float Right
        {
            get { return _frames.Count > 0 ? _frames[_index].Frame.Right : 0; }
        }

        public override float Top
        {
            get { return _frames.Count > 0 ? _frames[_index].Frame.Top : 0; }
        }

        public override float Bottom
        {
            get { return _frames.Count > 0 ? _frames[_index].Frame.Bottom : 0; }
        }

        public int CurrentIndex
        {
            get { return (_frames.Count == 0) ? -1 : _index; }
        }

        public StaticSprite CurrentSprite
        {
            get
            {
                if (_frames.Count == 0) {
                    return null;
                }
                return _frames[_index].Frame;
            }
        }

        public bool IsAnimating
        {
            get { return (_animOptions & AnimationOptions.Animating) == AnimationOptions.Animating; }
        }

        public bool RepeatIndefinitely
        {
            get { return (_animOptions & AnimationOptions.Infinite) == AnimationOptions.Infinite; }
            set
            {
                if (value) {
                    _animOptions |= AnimationOptions.Infinite;
                }
                else {
                    _animOptions &= ~AnimationOptions.Infinite;
                }
            }
        }

        public int RepeatIndex
        {
            get { return _restartAt; }
            set
            {
                if (value >= 0 && value < _frames.Count) {
                    _restartAt = value;
                }
            }
        }

        public int RepeatCount
        {
            get { return _loopCount; }
        }

        public int RepeatLimit
        {
            get { return _loopLimit; }
            set { _loopLimit = value; }
        }

        public RefClock ReferenceClock
        {
            get { return _refClock; }
            set { _refClock = value; }
            /*{
                if (_refClock != null) {
                    _refClock.Tick -= ReferenceClock_Tick;
                }

                _refClock = value;
                if (_refClock != null) {
                    _refClock.Tick += ReferenceClock_Tick;
                }
            }*/
        }

        #endregion

        public void AddSprite (StaticSprite sprite, float duration)
        {
            _frames.Add(new SpriteFrame(sprite, duration));
        }

        public void RemoveSprite (int index)
        {
            if (index < 0 || index >= _frames.Count) {
                return;
            }
            _frames.RemoveAt(index);
        }

        public override void Update (AmphibianGameTime gameTime)
        {
            if (_refClock != null) {
                if (_refClock.Triggered) {
                    if (_delay == 0)
                        AdvanceFrame();
                    else {
                        _delay--;
                        if (_delay == 0)
                            Start();
                    }
                }
            }
            else if (IsAnimating) {
                _timeAccum += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_timeAccum > _frames[_index].Duration) {
                    _timeAccum -= _frames[_index].Duration;
                    AdvanceFrame();
                }
            }
        }

        /*private void ReferenceClock_Tick (object sender, EventArgs e)
        {
            AdvanceFrame();
        }*/

        internal void AdvanceFrame ()
        {
            if (IsAnimating) {
                _index++;

                if (_index >= _frames.Count) {
                    _loopCount++;

                    if (RepeatIndefinitely || _loopCount < _loopLimit) {
                        _index = _restartAt;
                    }
                    else {
                        _index--;
                        Stop();
                    }
                }
            }
        }

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            if (_frames.Count > 0) {
                _frames[_index].Frame.Draw(spriteBatch, position);
            }
        }

        public void Start ()
        {
            _animOptions |= AnimationOptions.Animating;
            if (!RepeatIndefinitely && _loopCount == _loopLimit) {
                Restart();
            }
        }

        public void DelayStart (int delay)
        {
            _delay = delay;
        }

        public void Restart ()
        {
            _animOptions |= AnimationOptions.Animating;
            _index = 0;
            _loopCount = 0;
        }

        public void Stop ()
        {
            _animOptions &= ~AnimationOptions.Animating;
        }
    }
}
