using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Components;

namespace Amphibian.Systems
{
    public class BehaviorSystem : ProcessingSystem
    {
        public BehaviorSystem()
            : base(typeof(ScriptedComponent))
        {
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity e in entities) {
                Process(e);
            }
        }

        private void Process (Entity entity) {
            ScriptedComponent scriptCom = EntityManager.GetComponent<ScriptedComponent>(entity);
            if (scriptCom != null) {
                foreach (Behavior behaviors in scriptCom.Behaviors) {
                    behaviors.Update(this.SystemManager.World, entity);
                }
            }
        }
    }
}
