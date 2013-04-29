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
}
