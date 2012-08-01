using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class SpriteInfo
    {
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float Opacity { get; set; }
        public Color BlendColor { get; set; }
        public SpriteEffects Effects { get; set; }

        public SpriteInfo ()
        {
            Scale = 1f;
            Rotation = 0f;
            Opacity = 1f;
            BlendColor = Color.White;
        }
    }
}
