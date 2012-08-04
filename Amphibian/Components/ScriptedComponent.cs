using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;

namespace Amphibian.Components
{
    public sealed class ScriptedComponent : IComponent
    {
        private List<Behavior> _behaviors;

        public ScriptedComponent()
        {
            _behaviors = new List<Behavior>(4);
        }

        public List<Behavior> Behaviors
        {
            get { return _behaviors; }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptInfoTypeAttribute : Attribute
    {
        public Type InfoType;

        public ScriptInfoTypeAttribute (Type infoType)
        {
            InfoType = infoType;
        }
    }

    public class ScriptInfo
    {
        public void Initialize (EntityWorld world)
        {
            world.SystemManager.SystemAdded += HandleSystemAdded;

            foreach (BaseSystem system in world.SystemManager.Systems) {
                HandleSystemAdded(system);
            }
        }

        protected virtual void HandleSystemAdded (BaseSystem system)
        {
        }
    }

    public abstract class Behavior
    {
        private ScriptInfo _info;

        public void SystemInitialize (ScriptInfo info)
        {
            _info = info;
        }

        public abstract void Update(EntityWorld world, Entity entity);

        protected ScriptInfo Info
        {
            get { return _info; }
        }
    }
}
