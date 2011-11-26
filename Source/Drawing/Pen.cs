using System;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Drawing
{
    public class Pen
    {
        public Brush Brush { get; set; }
        public int Width { get; set; }

        public Pen (Brush brush, int width)
        {
            Brush = brush;
            Width = width;
        }

        public Pen (Brush brush)
            : this(brush, 1)
        {
        }
    }
}
