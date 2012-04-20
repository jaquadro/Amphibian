using System;

namespace Amphibian.Systems.Rendering.Sprites
{
    [Flags]
    public enum AnimationOptions
    {
        None = 0,
        Animating = 0x1,
        Infinite = 0x2,
    }
}
