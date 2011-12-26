using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Utility;

namespace Amphibian.EntitySystem
{
    

    /*internal struct ComponentMapping 
    {
        internal readonly ushort ComponentIndex;
        internal readonly ushort EntityIndex;

        public ComponentMapping (ushort componentIndex, ushort entityIndex) 
        {
            ComponentIndex = componentIndex;
            EntityIndex = entityIndex;
        }
    }*/

    public sealed class EntityManager
    {
        private static int _nextId = 1;

        private EntityWorld _world;
        private ResourcePool<UnorderedList<IComponent>> _componentListPool;

        // NB: May prefer to re-organize so that components are grouped by type instead of entity

        private static UnorderedList<IComponent> _emptyComponentList = new UnorderedList<IComponent>();
        private static UnorderedList<Entity> _emptyEntityList = new UnorderedList<Entity>();
        
        private Stack<int> _freeIndexes;
        private UnorderedList<Stack<int>> _freeEntListIndexes;

        private UnorderedList<Entity> _active;
        private UnorderedList<UnorderedList<IComponent>> _componentsByEntity;
        private UnorderedList<UnorderedList<Entity>> _entitiesByComponent;

        public event Action<Entity> AddedEntity;
        public event Action<Entity, IComponent> AddedComponent;
        public event Action<Entity> RemovedEntity;
        public event Action<Entity, IComponent> RemovedComponent;

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

        public Entity Create ()
        {
            int index = 0;
            if (_freeIndexes.Count > 0) {
                index = _freeIndexes.Pop();
            }
            else {
                index = _active.NextIndex();
            }

            Entity e = new Entity(NextId(), index);
            _active.Set(index, e);

            _componentsByEntity.Set(index, _componentListPool.TakeResource());

            OnAddedEntity(e);

            return e;
        }

        public void Remove (Entity entity)
        {
            if (IsValid(entity)) {
                _active.Set(entity.Index, new Entity());
                _freeIndexes.Push(entity.Index);

                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];

                // NB: Consider leaving old Entity entries, and checking validness in other methods
                for (int i = 0; i < comList.Count; i++) {
                    ComponentType ctype = ComponentTypeManager.GetTypeFor(comList[i].GetType());
                    UnorderedList<Entity> entList = _entitiesByComponent[ctype.Index];

                    for (int j = 0; j < entList.Count; j++) {
                        if (entity.Id == entList[j].Id) {
                            entList.Set(j, new Entity());
                            _freeEntListIndexes[i].Push(j);
                            break;
                        }
                    }

                    OnRemovedComponent(entity, comList[i]);
                }

                comList.Clear();

                _componentListPool.ReturnResource(comList);
                _componentsByEntity.Set(entity.Index, null);

                OnRemovedEntity(entity);
            }
        }

        public bool IsValid (Entity entity)
        {
            return entity.Id == _active[entity.Index].Id;
        }

        public void AddComponent (Entity entity, IComponent component)
        {
            if (IsValid(entity)) {
                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];
                comList.Add(component);

                ComponentType ctype = ComponentTypeManager.GetTypeFor(component.GetType());
                UnorderedList<Entity> entList = _entitiesByComponent[ctype.Index];

                int index = 0;
                if (_freeEntListIndexes[ctype.Index].Count > 0) {
                    index = _freeEntListIndexes[ctype.Index].Pop();
                }
                else {
                    index = entList.NextIndex();
                }

                entList.Set(index, entity);

                OnAddedComponent(entity, component);
            }
        }

        public void RemoveComponent (Entity entity, Type componentType)
        {
            if (IsValid(entity)) {
                IComponent component = null;

                UnorderedList<IComponent> comList = _componentsByEntity[entity.Index];
                for (int i = 0; i < comList.Count; i++) {
                    if (comList[i].GetType() == componentType) {
                        component = comList[i];
                        comList.RemoveAt(i);
                        break;
                    }
                }

                ComponentType ctype = ComponentTypeManager.GetTypeFor(componentType);
                UnorderedList<Entity> entList = _entitiesByComponent[ctype.Index];
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
                for (int i = 0; i < comList.Count; i++) {
                    IComponent com = comList[i];
                    if (com.GetType() == componentType) {
                        return com;
                    }
                }
            }

            return null;
        }

        public bool HasComponent (Entity entity, Type componentType)
        {
            return GetComponent(entity, componentType) != null;
        }

        public ComponentEnumerator GetComponents (Entity entity)
        {
            if (IsValid(entity)) {
                return new ComponentEnumerator(_componentsByEntity[entity.Index]);
            }
            return new ComponentEnumerator(_emptyComponentList);
        }

        public EntityEnumerator GetEntities (Type componentType)
        {
            if (componentType != null) {
                ComponentType ctype = ComponentTypeManager.GetTypeFor(componentType);
                return new EntityEnumerator(_entitiesByComponent[ctype.Index]);
            }
            return new EntityEnumerator(_emptyEntityList);
        }

        private int NextId ()
        {
            unchecked {
                _nextId++;
                if (_nextId == 0)
                    _nextId++;
                return _nextId;
            }
        }

        private void OnAddedEntity (Entity e)
        {
            if (AddedEntity != null)
                AddedEntity(e);
        }

        private void OnAddedComponent (Entity e, IComponent c)
        {
            if (AddedComponent != null)
                AddedComponent(e, c);
        }

        private void OnRemovedEntity (Entity e)
        {
            if (RemovedEntity != null)
                RemovedEntity(e);
        }

        private void OnRemovedComponent (Entity e, IComponent c)
        {
            if (RemovedComponent != null)
                RemovedComponent(e, c);
        }

        private void ComponentTypeManager_ComponentTypeAdded (ComponentType type)
        {
            _entitiesByComponent.Set(type.Index, new UnorderedList<Entity>());
            _freeEntListIndexes.Set(type.Index, new Stack<int>());
        }

        public struct EntityEnumerator
        {
            private UnorderedList<Entity> _entList;
            private int _index;

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
            private UnorderedList<IComponent> _comList;
            private int _index;

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
    }
}
