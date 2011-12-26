using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Utility;

namespace Amphibian.EntitySystem
{
    public static class ComponentTypeManager
    {
        private static int _nextIndex = 0;

        private static Dictionary<Type, ComponentType> _typeRegistry = new Dictionary<Type, ComponentType>();
        private static UnorderedList<ComponentType> _components = new UnorderedList<ComponentType>();

        public static event Action<ComponentType> ComponentTypeAdded;

        public static ComponentType GetTypeFor<T> ()
            where T : IComponent
        {
            return GetTypeFor(typeof(T));
        }

        public static ComponentType GetTypeFor (Type component)
        {
            ComponentType type;

            if (!_typeRegistry.TryGetValue(component, out type)) {
                type = new ComponentType(NextIndex());
                _typeRegistry.Add(component, type);
                _components.Set(type.Index, type);

                OnComponentTypeAdded(type);
            }

            return type;
        }

        internal static ComponentType GetTypeByIndex (int index)
        {
            return _components[index];
        }

        private static void OnComponentTypeAdded (ComponentType type)
        {
            if (ComponentTypeAdded != null) {
                ComponentTypeAdded(type);
            }
        }

        private static int NextIndex ()
        {
            return _nextIndex++;
        }
    }
}
