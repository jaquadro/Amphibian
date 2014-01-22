using Amphibian.Components;
using Amphibian.EntitySystem;

namespace Amphibian.Systems
{
    public class RenderTransformSystem : ProcessingSystem<Renderable>
    {
        [OptionalSystem]
        protected CameraSystem CameraSystem { get; set; }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity e in entities) {
                Process(e);
            }
        }

        protected override void Process (Entity entity, Renderable renderCom)
        {
            Position positionCom = EntityManager.GetComponent(entity, typeof(Position)) as Position;
            if (positionCom != null) {
                renderCom.RenderX = positionCom.X;
                renderCom.RenderY = positionCom.Y;
            }

            if (CameraSystem != null) {
                ParallaxCom parallaxCom = EntityManager.GetComponent(entity, typeof(ParallaxCom)) as ParallaxCom;
                if (parallaxCom != null) {
                    renderCom.RenderX += CameraSystem.Left * parallaxCom.ScrollCoefX;
                    renderCom.RenderY += CameraSystem.Top * parallaxCom.ScrollCoefY;
                }
            }
        }
    }
}
