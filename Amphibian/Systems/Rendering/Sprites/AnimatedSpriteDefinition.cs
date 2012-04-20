using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class AnimatedSpriteDefinition
    {
        public struct SpriteFrame
        {
            public StaticSpriteDefinition Frame;
            public float Duration;

            public SpriteFrame (StaticSpriteDefinition frame, float duration)
            {
                Frame = frame;
                Duration = duration;
            }
        }

        private List<SpriteFrame> _frames;
        private int _restartAt = 0;
        private int _loopLimit = 1;

        public AnimatedSpriteDefinition ()
        {
            _frames = new List<SpriteFrame>();
        }

        public AnimatedSprite CreateSprite ()
        {
            return new AnimatedSprite(this);
        }

        public List<SpriteFrame> Frames
        {
            get { return _frames; }
        }

        public int RestartAt
        {
            get { return _restartAt; }
            set { _restartAt = value; }
        }

        public int LoopLimit
        {
            get { return _loopLimit; }
            set { _loopLimit = value; }
        }

        public bool RepeatIndefinitely
        {
            get { return _loopLimit == -1; }
            set { _loopLimit = (value ? -1 : 1); }
        }

        public void AddSprite (StaticSpriteDefinition sprite, float duration)
        {
            _frames.Add(new AnimatedSpriteDefinition.SpriteFrame(sprite, duration));
        }

        public void RemoveSprite (int index)
        {
            if (index < 0 || index >= _frames.Count) {
                return;
            }
            _frames.RemoveAt(index);
        }

        public void Draw (SpriteBatch spriteBatch, PointFP position, AnimationInfo animData, SpriteInfo spriteData)
        {
            _frames[animData.CurrentFrameIndex].Frame.Draw(spriteBatch, position, spriteData);
        }
    }
}
