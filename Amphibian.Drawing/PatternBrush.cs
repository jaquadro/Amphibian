using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Drawing
{
    public class PatternBrush : Brush
    {
        public PatternBrush (GraphicsDevice device, Texture2D pattern)
            : this(device, pattern, 1f)
        {
        }

        public PatternBrush (GraphicsDevice device, Texture2D pattern, float opacity)
            : base()
        {
            Alpha = opacity;

            byte[] data = new byte[pattern.Width * pattern.Height * 4];
            pattern.GetData(data);

            Texture = new Texture2D(device, pattern.Width, pattern.Height, false, SurfaceFormat.Color);
            Texture.SetData(data);
        }

        protected override void DisposeManaged ()
        {
            Texture.Dispose();
        }
    }
}
