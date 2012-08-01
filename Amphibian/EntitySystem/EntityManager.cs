using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Utility;

namespace Amphibian.EntitySystem
{

    public sealed class EntityManager
    {
        private static int _nextId = 1;

        // NB: May prefer to re-organize so that components are grouped by type instead of entity.
        // Tradeoff optimizes cache use for system processing, but may require an extra E*C lookup table to maintain
        // fast lookup of components by entity.  Probably not worth it.

        #region Fields

        private EntityWorld _world;

        // Resource pool for entities' component lists.  Whenever an entity is released from the system, its
        // component list is cleared and recycled for use by other entities when they are creating.
        private ResourcePool<UnorderedList<IComponent>> _componentListPool;

        // List of empty slots in the _active and _componentsByEntity lists
        private Stack<int> _freeIndexes;

        // Lists of empty slots in the _entitiesByComponent list.
        // Every component type has its own entity list and corresponding free index list.
        private UnorderedList<Stack<int>> _freeEntListIndexes;

        // Master entity list.  Serves as a lookup table between entity index and entity id.
        private UnorderedList<Entity> _active;

        // List of component lists.  For every entity in _active, a corresponding component list is stored
        // at the same index here.  The stored list contains references to all of the components currently
        // attached to the corresponding entity.
        private UnorderedList<UnorderedList<IComponent>> _componentsByEntity;

        // List of entity lists.  There is an entry For every component defined in the global system.  Each
        // entry lists all entities that currently have an instance of the corresponding component attached.
        // This is mainly used by systems to quickly enumerate all entities that have a specific component.
        private UnorderedList<UnorderedList<Entity>> _entitiesByComponent;

        #endregion

        public EntityManager (EntityWorld world)
        {
            _world = world;
            _componentListPool = new ResourcePool<UnorderedList<IComponent>>();

            _freeIndexes = new Stack<int>();
            _freeEntListIndexes = new UnorderedList<Stack<int>>();

            _active = new UnorderedList<Entity>();
            _entitiesByComponent = new UnorderedList<UnorderedList<Entity>>();
            _componentsByEntity = new UnorderedList<UnorderedList<IComponent>>();

            ComponentTypeManager.ComponentTypeAdded += ComponentTypeManager_ComponentTypeAdded;
        }

        #region Events

        // To minimize unnecessary garbage, Actions are used in place of EventHandlers.

        public event Action<Entity> AddedEntity;
        public event Action<Entity, IComponent> AddedComponent;
        public event Action<Entity> RemovedEntity;
        public event Action<Entity, IComponent> RemovedComponent;

        private void OnAddedEntity (Entity e)
        {
            var handler = AddedEntity;
            if (handler != null)
                handler(e);
        }

        private void OnAddedComponent (Entity e, IComponent c)
        {
            var handler = AddedComponent;
            if (handler != null)
                handler(e, c);
        }

        private void OnRemovedEntity (Entity e)
        {
            var handler = RemovedEntity;
            if (handler != null)
                handler(e);
        }

        private void OnRemovedComponent (Entity e, IComponent c)
        {
            var handler = RemovedComponent;
            if (handler != null)
                handler(e, c);
        }

        #endregion

        #region Event Handlers

        private void ComponentTypeManager_ComponentTypeAdded (ComponentType type)
        {
            _entitiesByComponent.Set(type.Index, new UnorderedList<Entity>());
            _freeEntListIndexes.Set(type.Index, new Stack<int>());
        }

        #endregion

        public Entity Create ()
        {
            // Find the next empty slot in the entity table
            int index = 0;
            if (_freeIndexes.Count > 0) {
                index = _freeIndexes.Pop();
            }
            else {
                index = _active.NextIndex();
            }

            // Create the new entity and assign it an empty component list
            Entity e = new Entity(NextId(), index);
            _active.Set(index, e);

            _componentsByEntity.Set(index, _componentListPool.TakeResource());

            OnAddedEntity(e);

            return e;
        }

        public void Destroy (Entity entity)
        {
            if (IsValid(entity)) {
                // Delete the entity from the entity table and add its index to the freelist
                _active.Set(entity.Index, Entity.None);
                _freeIndexes.Push(entity.Index);

                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];

                // NB: Consider leaving old Entity entries, and checking validness in other methods

                // For each component in the entity's component list, scan the component's corresponding
                // entity list and delete the entity from there as well.  Runtime could be as bad as E*C.

                for (int i = 0; i < comList.Count; i++) {
                    ComponentType ctype = ComponentTypeManager.GetTypeFor(comList[i].GetType());
                    UnorderedList<Entity> entList = _entitiesByComponent[ctype.Index];

                    for (int j = 0; j < entList.Count; j++) {
                        if (entity.Id == entList[j].Id) {
                            entList.Set(j, new Entity());
                            _freeEntListIndexes[ctype.Index].Push(j);
                            break;
                        }
                    }

                    OnRemovedComponent(entity, comList[i]);
                }

                // Clear the component list and return it to the resource pool for recycling
                comList.Clear();

                _componentListPool.ReturnResource(comList);
                _componentsByEntity.Set(entity.Index, null);

                OnRemovedEntity(entity);
            }
        }

        public bool IsValid (Entity entity)
        {
            return entity.Id == _active[entity.Index].Id && entity.Id != 0;
        }

        public void AddComponent (Entity entity, IComponent component)
        {
            if (IsValid(entity)) {
                // Add the component to the entity's component list
                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];
                comList.Add(component);

                // Find the component type's entity list
                ComponentType ctype = ComponentTypeManager.GetTypeFor(component.GetType());
                UnorderedList<Entity> entList = _entitiesByComponent[ctype.Index];

                // Find the next empty slot in the component entity list
                int index = 0;
                if (_freeEntListIndexes[ctype.Index].Count > 0) {
                    index = _freeEntListIndexes[ctype.Index].Pop();
                }
                else {
                    index = entList.NextIndex();
                }

                // Add the entity to the component's entity list
                entList.Set(index, entity);

                OnAddedComponent(entity, component);
            }
        }

        public void RemoveComponent (Entity entity, Type componentType)
        {
            if (IsValid(entity)) {
                IComponent component = null;

                // Find an instance of the component type in the entity's component list and remove it
                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];
                for (int i = 0; i < comList.Count; i++) {
                    if (comList[i].GetType() == componentType) {
                        component = comList[i];
                        comList.RemoveAt(i);
                        break;
                    }
                }

                // Find the component type's entity list
                ComponentType ctype = ComponentTypeManager.GetTypeFor(componentType);
                UnorderedList<Entity> entList = _entitiesByComponent[ctype.Index];

                // Find the entity in the component's entity list and remove it
                for (int i = 0; i < entList.Count; i++) {
                    if (entity.Id == entList[i].Id) {
                        entList.Set(i, new Entity());
                        _freeEntListIndexes[ctype.Index].Push(i);
                    }
                }

                if (component != null)
                    OnRemovedComponent(entity, component);
            }
        }

        public IComponent GetComponent (Entity entity, Type componentType)
        {
            if (IsValid(entity)) {
                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];

                // Find and return an instance of the request component in the entity's component list, if it exists
                for (int i = 0; i < comList.Count; i++) {
                    IComponent com = comList[i];
                    if (com.GetType() == componentType) {
                        return com;
                    }
                }
            }

            return null;
        }

        public T GetComponent<T>(Entity entity)
            where T : IComponent
        {
            return (T)GetComponent(entity, typeof(T));
        }

        public bool HasComponent (Entity entity, Type componentType)
        {
            return GetComponent(entity, componentType) != null;
        }

        public bool HasComponent<T>(Entity entity)
        {
            return HasComponent(entity, typeof(T));
        }

        public ComponentEnumerator GetComponents (Entity entity)
        {
            if (IsValid(entity)) {
                return new ComponentEnumerator(_componentsByEntity[entity.Index]);
            }
            return ComponentEnumerator.Empty;
        }

        public EntityEnumerator GetEntities (Type componentType)
        {
            if (componentType != null) {
                ComponentType ctype = ComponentTypeManager.GetTypeFor(componentType);
                return new EntityEnumerator(_entitiesByComponent[ctype.Index]);
            }
            return EntityEnumerator.Empty;
        }

        private int NextId ()
        {
            unchecked {
                _nextId++;
                if (_nextId <= 0)
                    _nextId = 1;
                return _nextId;
            }
        }

        #region Enumerators

        public struct EntityEnumerator
        {
            private static UnorderedList<Entity> _emptyEntityList = new UnorderedList<Entity>();

            private UnorderedList<Entity> _entList;
            private int _index;

            internal static EntityEnumerator Empty 
            {
                get { return new EntityEnumerator(_emptyEntityList); }
            }

            internal EntityEnumerator (UnorderedList<Entity> entityList)
            {
                _index = -1;
                _entList = entityList;
            }

            public Entity Current
            {
                get { return _entList[_index]; }
            }

            public void Dispose ()
            {
            }

            public bool MoveNext ()
            {
                _index++;
                return _index < _entList.Count;
            }

            public void Reset ()
            {
                _index = -1;
            }

            public EntityEnumerator GetEnumerator ()
            {
                return this;
            }
        }

        public struct ComponentEnumerator
        {
            private static UnorderedList<IComponent> _emptyComponentList = new UnorderedList<IComponent>();

            private UnorderedList<IComponent> _comList;
            private int _index;

            internal static ComponentEnumerator Empty 
            {
                get { return new ComponentEnumerator(_emptyComponentList); }
            }

            internal ComponentEnumerator (UnorderedList<IComponent> comList)
            {
                _index = -1;
                _comList = comList;
            }

            public IComponent Current
            {
                get { return _comList[_index]; }
            }

            public void Dispose ()
            {
            }

            public bool MoveNext ()
            {
                _index++;
                return _index < _comList.Count;
            }

            public void Reset ()
            {
                _index = -1;
            }

            public ComponentEnumerator GetEnumerator ()
            {
                return this;
            }
        }

        #endregion
    }
}
