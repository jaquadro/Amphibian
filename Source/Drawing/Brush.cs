using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Drawing
{
    public abstract class Brush
    {
        protected Brush ()
        {
        }

        public virtual float Alpha { get; protected set; }
        public virtual Texture2D Texture { get; protected set; }
    }
}
