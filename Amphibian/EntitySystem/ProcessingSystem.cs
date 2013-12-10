using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    public abstract class ProcessingSystem : BaseSystem
    {
        private Type _primaryKey;

        protected ProcessingSystem (Type primaryKey)
        {
            _primaryKey = primaryKey;
        }

        protected override void ProcessInner ()
        {
            ProcessEntities(EntityManager.GetEntities(_primaryKey));
        }

        protected virtual void ProcessEntities(EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities)
            {
                Process(entity);
            }
        }

        protected abstract void Process(Entity entity);
    }

    public abstract class ProcessingSystem<TComponent> : ProcessingSystem
        where TComponent : class, IComponent
    {
        protected ProcessingSystem ()
            : base(typeof(TComponent))
        { }

        protected override void Process (Entity entity)
        {
            Process(entity, EntityManager.GetComponent<TComponent>(entity));
        }

        protected abstract void Process (Entity entity, TComponent component);
    }

    public abstract class ProcessingSystem<TComponent1, TComponent2> : ProcessingSystem
        where TComponent1 : class, IComponent
        where TComponent2 : class, IComponent
    {
        protected ProcessingSystem ()
            : base(typeof(TComponent1))
        { }

        protected override void ProcessInner ()
        {
            int count1 = EntityManager.CountEntities<TComponent1>();
            int count2 = EntityManager.CountEntities<TComponent2>();

            if (count1 < count2)
                ProcessEntities(EntityManager.GetEntities<TComponent1>());
            else
                ProcessEntities(EntityManager.GetEntities<TComponent2>());
        }

        protected override void Process (Entity entity)
        {
            TComponent1 com1;
            TComponent2 com2;

            if (EntityManager.GetComponent<TComponent1, TComponent2>(entity, out com1, out com2))
                Process(entity, com1, com2);
        }

        protected abstract void Process (Entity entity, TComponent1 component1, TComponent2 component2);
    }

    public abstract class ProcessingSystem<TComponent1, TComponent2, TComponent3> : ProcessingSystem
        where TComponent1 : class, IComponent
        where TComponent2 : class, IComponent
        where TComponent3 : class, IComponent
    {
        protected ProcessingSystem ()
            : base(typeof(TComponent1))
        { }

        protected override void ProcessInner ()
        {
            int count1 = EntityManager.CountEntities<TComponent1>();
            int count2 = EntityManager.CountEntities<TComponent2>();
            int count3 = EntityManager.CountEntities<TComponent3>();

            int minCount = Math.Min(count1, Math.Min(count2, count3));
            if (count1 == minCount)
                ProcessEntities(EntityManager.GetEntities<TComponent1>());
            else if (count2 == minCount)
                ProcessEntities(EntityManager.GetEntities<TComponent2>());
            else
                ProcessEntities(EntityManager.GetEntities<TComponent3>());
        }

        protected override void Process (Entity entity)
        {
            TComponent1 com1;
            TComponent2 com2;
            TComponent3 com3;

            if (EntityManager.GetComponent<TComponent1, TComponent2, TComponent3>(entity, out com1, out com2, out com3))
                Process(entity, com1, com2, com3);
        }

        protected abstract void Process (Entity entity, TComponent1 component1, TComponent2 component2, TComponent3 component3);
    }
}
