﻿using System;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class SpriteInfo
    {
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float Opacity { get; set; }
        public SpriteEffects Effects { get; set; }

        public SpriteInfo ()
        {
            Opacity = 1f;
        }
    }
}
