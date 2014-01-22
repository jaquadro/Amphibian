using System;

namespace Amphibian.EntitySystem
{
    public abstract class EventElement
    {
        public abstract void Dispatch (Entity e, IComponent com);
    }

    public class EventElement<T> : EventElement
        where T : class, IComponent
    {
        protected event Action<Entity, T> eventdelegate;

        public override void Dispatch (Entity e, IComponent com)
        {
            if (eventdelegate != null)
                eventdelegate(e, com as T);
        }

        public void Dispatch (Entity e, T com)
        {
            if (eventdelegate != null)
                eventdelegate(e, com);
        }

        public static EventElement<T> operator + (EventElement<T> kElement, Action<Entity, T> kDelegate)
        {
            kElement.eventdelegate += kDelegate;
            return kElement;
        }

        public static EventElement<T> operator - (EventElement<T> kElement, Action<Entity, T> kDelegate)
        {
            kElement.eventdelegate -= kDelegate;
            return kElement;
        }
    }

    public abstract class SystemElement
    {
        public abstract void Dispatch (BaseSystem sys);

        internal abstract void AddHandler (Action<object> kDelegate);
        internal abstract void RemoveHandler (Action<object> kDelegate);
    }

    public class SystemElement<T> : SystemElement
        where T : BaseSystem
    {
        protected event Action<T> eventdelegate;

        public override void Dispatch (BaseSystem sys)
        {
            if (eventdelegate != null)
                eventdelegate(sys as T);
        }

        public void Dispatch (T sys)
        {
            if (eventdelegate != null)
                eventdelegate(sys);
        }

        public static SystemElement<T> operator + (SystemElement<T> kElement, Action<T> kDelegate)
        {
            kElement.eventdelegate += kDelegate;
            return kElement;
        }

        public static SystemElement<T> operator - (SystemElement<T> kElement, Action<T> kDelegate)
        {
            kElement.eventdelegate -= kDelegate;
            return kElement;
        }

        internal override void AddHandler (Action<object> kDelegate)
        {
            Action<T> typedDelegate = (Action<T>)kDelegate;
            eventdelegate += typedDelegate;
        }

        internal override void RemoveHandler (Action<object> kDelegate)
        {
            Action<T> typedDelegate = (Action<T>)kDelegate;
            eventdelegate -= typedDelegate;
        }
    }
}
