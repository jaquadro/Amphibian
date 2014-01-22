using Amphibian.Components;
using Amphibian.EntitySystem;
using Amphibian.Systems.Rendering;

namespace Amphibian.Systems
{
    public class AnimationSystem : ProcessingSystem<Renderable>
    {
        [RequiredSystem]
        protected RenderSystem RenderSystem { get; set; }

        protected override void Process (Entity entity, Renderable renderCom)
        {
            Spatial spat = RenderSystem.SpatialManager.GetSpatial(renderCom.SpatialRef);
            if (spat != null)
                spat.Update();
        }
    }
}
