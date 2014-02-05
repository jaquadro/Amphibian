using Amphibian.Components;
using Amphibian.EntitySystem;
using Amphibian.Systems.Rendering.Spatials;

namespace Amphibian.Systems.Actions
{
    public abstract class RenderEffectsAction : TemporalAction
    {
        protected RenderSystem RenderSystem { get; set; }

        protected override void Begin (EntityWorld world, Entity entity)
        {
            RenderSystem = world.SystemManager.GetSystem<RenderSystem>();
            if (RenderSystem == null) {
                Finish();
                return;
            }

            IRenderEffects renderEffects = GetRenderEffects(world.EntityManager.GetComponent<Renderable>(entity));
            if (renderEffects == null) {
                Finish();
                return;
            }

            Begin(renderEffects);
        }

        protected virtual void Begin (IRenderEffects effects)
        { }

        protected override void Update (EntityWorld world, Entity entity, float percent)
        {
            IRenderEffects renderEffects = GetRenderEffects(world.EntityManager.GetComponent<Renderable>(entity));
            if (renderEffects != null)
                Update(renderEffects, percent);
        }

        protected virtual void Update (IRenderEffects effects, float percent)
        { }

        public override void Reset ()
        {
            base.Reset();
            RenderSystem = null;
        }

        private IRenderEffects GetRenderEffects (Renderable renderCom)
        {
            if (renderCom == null)
                return null;

            return RenderSystem.SpatialManager.GetSpatial(renderCom.SpatialRef) as IRenderEffects;
        }
    }
}
