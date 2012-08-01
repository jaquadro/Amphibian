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

    public abstract class Behavior
    {
        public abstract void Update(EntityWorld world, Entity entity);
    }
}
