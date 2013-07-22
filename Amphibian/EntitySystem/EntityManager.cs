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

        // NB: Enforcing an ordering on components in the component list and adding generic entity enumerators that also
        // request components on that entity, it may be possible to fetch the requested components without repeatedly
        // iterating over the component list.  Sorting of the request only needs to be paid once per system request.

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

        private Dictionary<ComponentType, EventElement> _comAddedEvents;
        private Dictionary<ComponentType, EventElement> _comRemovedEvents;

        public EntityManager (EntityWorld world)
        {
            _world = world;
            _componentListPool = new ResourcePool<UnorderedList<IComponent>>();

            _freeIndexes = new Stack<int>();
            _freeEntListIndexes = new UnorderedList<Stack<int>>();

            _active = new UnorderedList<Entity>();
            _entitiesByComponent = new UnorderedList<UnorderedList<Entity>>();
            _componentsByEntity = new UnorderedList<UnorderedList<IComponent>>();

            _comAddedEvents = new Dictionary<ComponentType, EventElement>();
            _comRemovedEvents = new Dictionary<ComponentType, EventElement>();

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

                EventElement element;
                if (_comAddedEvents.TryGetValue(ctype, out element))
                    element.Dispatch(entity, component);
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

                if (component != null) {
                    OnRemovedComponent(entity, component);

                    EventElement element;
                    if (_comRemovedEvents.TryGetValue(ctype, out element))
                        element.Dispatch(entity, component);
                }
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
            where T : class, IComponent
        {
            return GetComponent(entity, typeof(T)) as T;
        }

        private static List<Type> _typeBuffer;


        // Assumption: entity is valid and  _typeBuffer has been updated to match the entity's current component types
        private bool GetComponent<T> (Entity entity, out T com, UnorderedList<IComponent> comList)
            where T : class, IComponent
        {
            for (int i = 0; i < comList.Count; i++) {
                if (_typeBuffer[i] == typeof(T)) {
                    com = comList[i] as T;
                    return true;
                }
            }

            com = null;
            return false;
        }

        public bool GetComponent<T1> (Entity entity, out T1 com1)
            where T1 : class, IComponent
        {
            if (IsValid(entity)) {
                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];

                for (int i = 0; i < comList.Count; i++) {
                    if (comList[i].GetType() == typeof(T1)) {
                        com1 = comList[i] as T1;
                        return true;
                    }
                }
            }

            com1 = null;
            return false;
        }

        public bool GetComponent<T1, T2> (Entity entity, out T1 com1, out T2 com2)
            where T1 : class, IComponent
            where T2 : class, IComponent
        {
            if (IsValid(entity)) {
                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];

                for (int i = 0; i < comList.Count; i++)
                    _typeBuffer[i] = comList[i].GetType();

                return GetComponent<T1>(entity, out com1, comList) &
                    GetComponent<T2>(entity, out com2, comList);
            }

            com1 = null;
            com2 = null;
            return false;
        }

        public bool GetComponent<T1, T2, T3> (Entity entity, out T1 com1, out T2 com2, out T3 com3)
            where T1 : class, IComponent
            where T2 : class, IComponent
            where T3 : class, IComponent
        {
            if (IsValid(entity)) {
                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];

                for (int i = 0; i < comList.Count; i++)
                    _typeBuffer[i] = comList[i].GetType();

                return GetComponent<T1>(entity, out com1, comList) &
                    GetComponent<T2>(entity, out com2, comList) &
                    GetComponent<T3>(entity, out com3, comList);
            }

            com1 = null;
            com2 = null;
            com3 = null;
            return false;
        }

        public bool GetComponent<T1, T2, T3, T4> (Entity entity, out T1 com1, out T2 com2, out T3 com3, out T4 com4)
            where T1 : class, IComponent
            where T2 : class, IComponent
            where T3 : class, IComponent
            where T4 : class, IComponent
        {
            if (IsValid(entity)) {
                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];

                for (int i = 0; i < comList.Count; i++)
                    _typeBuffer[i] = comList[i].GetType();

                return GetComponent<T1>(entity, out com1, comList) &
                    GetComponent<T2>(entity, out com2, comList) &
                    GetComponent<T3>(entity, out com3, comList) &
                    GetComponent<T4>(entity, out com4, comList);
            }

            com1 = null;
            com2 = null;
            com3 = null;
            com4 = null;
            return false;
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

        public int CountEntities (Type componentType)
        {
            if (componentType != null) {
                ComponentType ctype = ComponentTypeManager.GetTypeFor(componentType);
                return _entitiesByComponent[ctype.Index].Count - _freeEntListIndexes[ctype.Index].Count;
            }
            return 0;
        }

        public int CountEntities<T> ()
        {
            return CountEntities(typeof(T));
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

        public void RegisterComponentAddedHandler<T> (Action<Entity, T> handler)
            where T : class, IComponent
        {
            ComponentType ctype = ComponentTypeManager.GetTypeFor<T>();
            EventElement element;
            if (!_comAddedEvents.TryGetValue(ctype, out element)) {
                element = new EventElement<T>();
                _comAddedEvents.Add(ctype, element);
            }

            EventElement<T> typedElement = element as EventElement<T>;
            if (typedElement == null)
                throw new InvalidOperationException("Unexpected EventElement type for component type T");

            typedElement += handler;
        }

        public void UnregisterComponentAddedHandler<T> (Action<Entity, T> handler)
            where T : class, IComponent
        {
            ComponentType ctype = ComponentTypeManager.GetTypeFor<T>();
            EventElement element;
            if (!_comAddedEvents.TryGetValue(ctype, out element))
                return;

            EventElement<T> typedElement = element as EventElement<T>;
            if (typedElement == null)
                throw new InvalidOperationException("Unexpected EventElement type for component type T");

            typedElement -= handler;
        }

        public void RegisterComponentRemovedHandler<T> (Action<Entity, T> handler)
            where T : class, IComponent
        {
            ComponentType ctype = ComponentTypeManager.GetTypeFor<T>();
            EventElement element;
            if (!_comRemovedEvents.TryGetValue(ctype, out element)) {
                element = new EventElement<T>();
                _comRemovedEvents.Add(ctype, element);
            }

            EventElement<T> typedElement = element as EventElement<T>;
            if (typedElement == null)
                throw new InvalidOperationException("Unexpected EventElement type for component type T");

            typedElement += handler;
        }

        public void UnregisterComponentRemovedHandler<T> (Action<Entity, T> handler)
            where T : class, IComponent
        {
            ComponentType ctype = ComponentTypeManager.GetTypeFor<T>();
            EventElement element;
            if (!_comRemovedEvents.TryGetValue(ctype, out element))
                return;

            EventElement<T> typedElement = element as EventElement<T>;
            if (typedElement == null)
                throw new InvalidOperationException("Unexpected EventElement type for component type T");

            typedElement -= handler;
        }

        static EntityManager ()
        {
            _typeBuffer = new List<Type>(32);
            for (int i = 0; i < 32; i++)
                _typeBuffer.Add(null);
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
