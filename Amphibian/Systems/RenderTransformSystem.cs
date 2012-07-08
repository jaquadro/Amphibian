using Amphibian.Components;
using Amphibian.EntitySystem;

namespace Amphibian.Systems
{
    public class RenderTransformSystem : ProcessingSystem
    {
        private CameraSystem _cameraSystem;

        public RenderTransformSystem ()
            : base(typeof(Renderable))
        {
        }

        protected internal override void Initialize ()
        {
            _cameraSystem = SystemManager.GetSystem(typeof(CameraSystem)) as CameraSystem;

            SystemManager.SystemAdded += SystemManager_SystemAdded;
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity e in entities) {
                Process(e);
            }
        }

        private void Process (Entity entity)
        {
            Renderable renderCom = EntityManager.GetComponent(entity, typeof(Renderable)) as Renderable;
            if (renderCom == null)
                return;

            Position positionCom = EntityManager.GetComponent(entity, typeof(Position)) as Position;
            if (positionCom != null) {
                renderCom.RenderX = positionCom.X;
                renderCom.RenderY = positionCom.Y;
            }

            ParallaxCom parallaxCom = EntityManager.GetComponent(entity, typeof(ParallaxCom)) as ParallaxCom;
            if (parallaxCom != null) {
                renderCom.RenderX += _cameraSystem.Left * parallaxCom.ScrollCoefX;
                renderCom.RenderY += _cameraSystem.Top * parallaxCom.ScrollCoefY;
            }
        }

        private void SystemManager_SystemAdded (BaseSystem system)
        {
            if (system is CameraSystem) {
                _cameraSystem = system as CameraSystem;
            }
        }
    }
}
