using Amphibian.Systems.Rendering.Spatials;

namespace Amphibian.Systems.Actions
{
    public class AlphaAction : RenderEffectsAction
    {
        private float _start;
        private float _end;

        public float Alpha
        {
            get { return _end; }
            set { _end = value; }
        }

        protected override void Begin (IRenderEffects effects)
        {
            _start = effects.Opacity;
        }

        protected override void Update(IRenderEffects effects, float percent)
        {
            effects.Opacity = _start + (_end - _start) * percent;    
        }
    }
}
