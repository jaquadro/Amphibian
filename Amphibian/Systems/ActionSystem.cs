using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Systems.Actions;
using Amphibian.Utility;

namespace Amphibian.Systems
{
    public class ActionComponent : IComponent
    {
        public ActionComponent ()
        {
            Actions = new UnorderedList<EntityAction>(4);
        }

        public UnorderedList<EntityAction> Actions { get; set; }
    }

    public class ActionSystem : ProcessingSystem<ActionComponent>
    {
        private ResourcePool<ActionComponent> _freeComponents = new ResourcePool<ActionComponent>();

        protected override void Process (Entity entity, ActionComponent component)
        {
            for (int i = 0; i < component.Actions.Count; i++) {
                var action = component.Actions[i];
                if (action.Update(World, entity)) {
                    component.Actions.RemoveAt(i--);

                    if (action.OnCompleted != null)
                        action.OnCompleted(entity);
                    if (action.Pool != null)
                        action.Pool.Release(action);
                }
            }

            if (component.Actions.Count == 0) {
                EntityManager.RemoveComponent(entity, component.GetType());
                _freeComponents.ReturnResource(component);
            }
        }

        public void Start (Entity entity, EntityAction action)
        {
            ActionComponent actionCom;
            if (!EntityManager.GetComponent<ActionComponent>(entity, out actionCom))
                EntityManager.AddComponent(entity, actionCom = _freeComponents.TakeResource());

            if (actionCom.Actions.Contains(action))
                action.Restart();
            else
                actionCom.Actions.Add(action);
        }
    }
}
