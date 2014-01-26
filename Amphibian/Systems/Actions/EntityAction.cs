using System;
using Amphibian.EntitySystem;
using Amphibian.Utility;

namespace Amphibian.Systems.Actions
{
    public abstract class EntityAction : IPoolable
    {
        public Action<Entity> OnCompleted { get; set; }

        public Pool Pool { get; set; }

        public abstract bool Update (EntityWorld world, Entity entity);

        public virtual void Restart ()
        { }

        public virtual void Reset ()
        {
            Restart();

            Pool = null;
            OnCompleted = null;
        }
    }
}
