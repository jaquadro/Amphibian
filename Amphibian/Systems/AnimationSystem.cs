using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Components;
using Amphibian.Systems.Rendering;

namespace Amphibian.Systems
{
    public class AnimationSystem : ProcessingSystem
    {
        public AnimationSystem ()
            : base(typeof(Renderable))
        {
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity e in entities) {
                Process(e);
            }
        }

        protected override void Process (Entity entity) {
            Renderable renderCom = EntityManager.GetComponent(entity, typeof(Renderable)) as Renderable;
            if (renderCom == null)
                return;

            RenderSystem renderSys = SystemManager.GetSystem(typeof(RenderSystem)) as RenderSystem;
            Spatial spat = renderSys.SpatialManager.GetSpatial(renderCom.SpatialRef);

            if (spat != null) {
                spat.Update();
            }
        }
    }
}
