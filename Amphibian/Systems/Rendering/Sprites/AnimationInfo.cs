using System;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class AnimationInfo
    {
        public AnimationOptions Options { get; set; }
        public float TimeAccum { get; set; }
        public int Delay { get; set; }
        public int LoopCount { get; set; }

        public RefClock RefClock { get; set; }
        public int CurrentFrameIndex { get; set; }
    }
}
