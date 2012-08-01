using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering.Spatials
{
    public interface IRenderEffects
    {
        float Scale { get; set; }
        float Rotation { get; set; }
        float Opacity { get; set; }
        Color BlendColor { get; set; }
    }
}
