using Amphibian.Systems.Rendering.Spatials;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Actions
{
    public class ColorAction : RenderEffectsAction
    {
        private Color _startColor;

        public Color? StartColor { get; set; }
        public Color EndColor { get; set; }

        protected override void Begin (IRenderEffects effects)
        {
            if (StartColor == null)
                StartColor = effects.BlendColor;
            _startColor = StartColor.Value;
        }

        protected override void Update (IRenderEffects effects, float percent)
        {
            float r = _startColor.R + (EndColor.R - _startColor.R) * percent;
            float g = _startColor.G + (EndColor.G - _startColor.G) * percent;
            float b = _startColor.B + (EndColor.B - _startColor.B) * percent;
            float a = _startColor.A + (EndColor.A - _startColor.A) * percent;

            effects.BlendColor = new Color((byte)r, (byte)g, (byte)b, (byte)a);
        }

        public override void Reset ()
        {
            base.Reset();
            StartColor = null;
        }
    }
}
