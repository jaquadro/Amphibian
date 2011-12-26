using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    internal struct ComponentEntry {
        public short ComponentIndex;
        public short EntityIndex;

        public ComponentEntry (short componentIndex, short entityIndex)
        {
            ComponentIndex = componentIndex;
            EntityIndex = entityIndex;
        }
    }

    public interface IComponent
    {

    }

    public sealed class ComponentType
    {
        private int _id;

        public int Id
        {
            get { return _id; }
        }
    }

    public static class ComponentTypeManager
    {
        private static Dictionary<Type, int> _typeRegistry = new Dictionary<Type,int>();
        private static UnorderedList<ComponentType> _components = new UnorderedList<ComponentType>();

        public static event Action<ComponentType> ComponentTypeAdded;

        public static int GetTypeIndexFor<T> ()
            where T : IComponent
        {
            int typeIndex = 0;
            Type request = typeof(T);

            if (!_typeRegistry.TryGetValue(request, out typeIndex)) {
                ComponentType type = new ComponentType();

                _components.Add(type, out typeIndex);
                _typeRegistry.Add(request, typeIndex);

                OnComponentTypeAdded(type);
            }

            return typeIndex;
        }

        public static ComponentType GetTypeFor<T> ()
            where T : IComponent
        {
            int index = GetTypeIndexFor<T>();
            return _components[index];
        }

        public static int GetTypeIndexFor (Type component)
        {
            int typeIndex = 0;

            if (!_typeRegistry.TryGetValue(component, out typeIndex)) {
                ComponentType type = new ComponentType();

                _components.Add(type, out typeIndex);
                _typeRegistry.Add(component, typeIndex);

                OnComponentTypeAdded(type);
            }

            return typeIndex;
        }

        public static ComponentType GetTypeFor (Type component)
        {
            int index = GetTypeIndexFor(component);
            return _components[index];
        }

        public static ComponentType GetType (int index)
        {
            return _components[index];
        }

        private static void OnComponentTypeAdded (ComponentType type)
        {
            if (ComponentTypeAdded != null) {
                ComponentTypeAdded(type);
            }
        }
    }

    public sealed class Entity
    {
        private int _id;
        private bool _free;

        private UnorderedList<ComponentEntry> _components;

        private EntityManager _entityManager;

        internal Entity (int id)
        {
            _id = id;
            _components = new UnorderedList<ComponentEntry>();
        }

        public int Id
        {
            get { return _id; }
        }

        public bool IsActive
        {
            get { return !_free; }
        }

        internal bool Free
        {
            get { return _free; }
            set { _free = value; }
        }

        internal UnorderedList<ComponentEntry> Components
        {
            get { return _components; }
        }

        internal void Reset ()
        {
            _free = false;
            _components.Clear();
        }

        public override string ToString ()
        {
            return "Entity[" + _id + "]";
        }
    }

    
}
