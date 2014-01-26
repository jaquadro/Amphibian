using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Systems.Rendering.Spatials;

namespace Amphibian.Systems.Actions
{
    public class ScaleToAction : RenderEffectsAction
    {
        private float _start;

        public float Scale { get; set; }

        protected override void Begin (IRenderEffects effects)
        {
            _start = effects.Scale;
        }

        protected override void Update (IRenderEffects effects, float percent)
        {
            effects.Scale = _start + (Scale - _start) * percent;
        }
    }
}
