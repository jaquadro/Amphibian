using System;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class AnimationInfo
    {
        internal AnimationOptions Options { get; set; }
        internal float TimeAccum { get; set; }
        internal int Delay { get; set; }
        internal int LoopCount { get; set; }

        public RefClock RefClock { get; set; }
        public int CurrentFrameIndex { get; set; }
    }
}
