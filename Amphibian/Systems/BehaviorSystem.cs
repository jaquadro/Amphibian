﻿using System;
using System.Collections.Generic;
using Amphibian.Components;
using Amphibian.EntitySystem;

namespace Amphibian.Systems
{
    public class BehaviorSystem : ProcessingSystem<ScriptedComponent>
    {
        private Dictionary<Type, ScriptInfo> _infoMap = new Dictionary<Type,ScriptInfo>();

        protected internal override void Initialize ()
        {
            EntityManager.AddedComponent += HandleAddedComponent;
        }

        protected override void Process (Entity entity, ScriptedComponent scriptCom)
        {
            foreach (Behavior behaviors in scriptCom.Behaviors)
                behaviors.Update(this.SystemManager.World, entity);
        }

        public void RegisterType (Type behaviorType)
        {
            Type infoType = typeof(ScriptInfo);
            ScriptInfoTypeAttribute attr = Attribute.GetCustomAttribute(behaviorType, typeof(ScriptInfoTypeAttribute)) as ScriptInfoTypeAttribute;
            if (attr != null)
                infoType = attr.InfoType;

            ScriptInfo info = Activator.CreateInstance(infoType) as ScriptInfo;
            if (info == null)
                info = Activator.CreateInstance<ScriptInfo>();

            info.Initialize(SystemManager.World);

            _infoMap[behaviorType] = info;
        }

        private void HandleAddedComponent (Entity entity, IComponent component)
        {
            ScriptedComponent comScript = component as ScriptedComponent;
            if (comScript != null) {
                foreach (Behavior behavior in comScript.Behaviors) {
                    InitializeBehavior(behavior);
                }
            }
        }

        private void InitializeBehavior (Behavior behavior)
        {
            if (behavior == null)
                return;

            Type behaviorType = behavior.GetType();
            if (!_infoMap.ContainsKey(behaviorType)) {
                RegisterType(behaviorType);
            }

            behavior.SystemInitialize(_infoMap[behaviorType]);
        }
    }
}
