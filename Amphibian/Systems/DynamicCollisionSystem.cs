using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Collision;
using Amphibian.Components;
using Amphibian.Utility;

namespace Amphibian.Systems
{
    public class DynamicCollisionSystem : ProcessingSystem
    {
        private static Func<Entity, bool> _noCondition = (e) => { return true; };

        private EntityCollisionManager _manager;

        private List<Entity> _empty;
        private ResourcePool<List<Entity>> _listPool;
        private Dictionary<Entity, List<Entity>> _prevPairs;
        private Dictionary<Entity, List<Entity>> _curPairs;

        private Dictionary<Type, Func<Entity, bool>> _componentLookupCache;
        private Dictionary<Type, Func<Entity, bool>> _componentCollisionStartedCache;

        public DynamicCollisionSystem ()
            : base(typeof(Collidable))
        {
            _manager = new EntityCollisionManager();
            _manager.FineCollision += FineCollisionHandler;

            _empty = new List<Entity>();
            _listPool = new ResourcePool<List<Entity>>();
            _prevPairs = new Dictionary<Entity, List<Entity>>();
            _curPairs = new Dictionary<Entity, List<Entity>>();

            _componentLookupCache = new Dictionary<Type, Func<Entity, bool>>();
            _componentCollisionStartedCache = new Dictionary<Type, Func<Entity, bool>>();
        }

        public bool Overlaps (Entity e1, Entity e2)
        {
            List<Entity> list;
            if (!_curPairs.TryGetValue(e1, out list) || !list.Contains(e2))
                return false;

            return true;
        }

        public CollisionEnumerator Overlaps (Entity e)
        {
            return Overlaps(e, _noCondition);
        }

        public CollisionEnumerator Overlaps (Entity e, Type component)
        {
            return Overlaps(e, GetComponentCondition(component));
        }

        public CollisionEnumerator Overlaps (Entity e, Func<Entity, bool> condition)
        {
            List<Entity> curList;
            if (!_curPairs.TryGetValue(e, out curList))
                return CollisionEnumerator.Empty;

            return new CollisionEnumerator(curList, condition);
        }

        public bool CollisionStarted (Entity e1, Entity e2)
        {
            List<Entity> list;
            if (_prevPairs.TryGetValue(e1, out list) && list.Contains(e2))
                return false;

            if (!_curPairs.TryGetValue(e1, out list) || !list.Contains(e2))
                return false;

            return true;
        }

        public CollisionStartEnumerator CollisionStarted (Entity e)
        {
            return CollisionStarted(e, _noCondition);
        }

        public CollisionStartEnumerator CollisionStarted (Entity e, Type component)
        {
            return CollisionStarted(e, GetComponentCondition(component));
        }

        public CollisionStartEnumerator CollisionStarted (Entity e, Func<Entity, bool> condition)
        {
            List<Entity> curList;
            if (!_curPairs.TryGetValue(e, out curList))
                return CollisionStartEnumerator.Empty;

            List<Entity> prevList;
            if (!_prevPairs.TryGetValue(e, out prevList))
                prevList = _empty;

            return new CollisionStartEnumerator(curList, prevList, condition);
        }

        public bool CollisionHeld (Entity e1, Entity e2)
        {
            List<Entity> list;
            if (!_prevPairs.TryGetValue(e1, out list) || !list.Contains(e2))
                return false;

            if (!_curPairs.TryGetValue(e1, out list) || !list.Contains(e2))
                return false;

            return true;
        }

        public bool CollisionEnded (Entity e1, Entity e2)
        {
            List<Entity> list;
            if (!_prevPairs.TryGetValue(e1, out list) || !list.Contains(e2))
                return false;

            if (_curPairs.TryGetValue(e1, out list) && list.Contains(e2))
                return false;

            return true;
        }

        protected internal override void Initialize ()
        {
            EntityManager.AddedComponent += ComponentAttachHandler;
            EntityManager.RemovedComponent += ComponentDetachHandler;
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities) {
                Process(entity);
            }
        }

        private void Process (Entity entity)
        {
            Position comPosition = null;
            Collidable comCollidable = null;

            foreach (IComponent com in EntityManager.GetComponents(entity)) {
                if (com is Position) {
                    comPosition = com as Position;
                }
                else if (com is Collidable) {
                    comCollidable = com as Collidable;
                }
            }

            if (comPosition == null || comCollidable == null)
                return;

            comCollidable.CollisionMask.Position.X = comPosition.X;
            comCollidable.CollisionMask.Position.Y = comPosition.Y;
        }

        protected override void End ()
        {
            Reset();
            _manager.Update();
        }

        private void FineCollisionHandler (Entity e1, Entity e2)
        {
            AddObjectToList(e1, e2);
            AddObjectToList(e2, e1);
        }

        private void ComponentAttachHandler (Entity e, IComponent c)
        {
            Collidable comCollision = c as Collidable;
            if (comCollision != null) {
                _manager.AddObject(e, comCollision);
            }
        }

        private void ComponentDetachHandler (Entity e, IComponent c)
        {
            if (c is Collidable) {
                _manager.RemoveObject(e);
            }
        }

        private void AddObjectToList (Entity first, Entity second)
        {
            List<Entity> list;
            if (!_curPairs.TryGetValue(first, out list)) {
                list = _listPool.TakeResource();
                _curPairs.Add(first, list);
            }

            list.Add(second);
        }

        public void Reset ()
        {
            foreach (List<Entity> list in _prevPairs.Values) {
                list.Clear();
                _listPool.ReturnResource(list);
            }

            _prevPairs.Clear();

            Dictionary<Entity, List<Entity>> refreshed = _prevPairs;
            _prevPairs = _curPairs;
            _curPairs = refreshed;
        }

        private Func<Entity, bool> GetComponentCondition (Type componentType)
        {
            Func<Entity, bool> condition;
            if (_componentLookupCache.TryGetValue(componentType, out condition))
                return condition;

            condition = (e) => {
                return EntityManager.HasComponent(e, componentType);
            };

            _componentLookupCache[componentType] = condition;
            return condition;
        }

        public struct CollisionEnumerator
        {
            private static List<Entity> _emptyEntityList = new List<Entity>();
            private static Func<Entity, bool> _noConidition = (e) => { return true; };

            private List<Entity> _curList;

            private int _index;
            private Func<Entity, bool> _condition;

            internal static CollisionEnumerator Empty
            {
                get { return new CollisionEnumerator(_emptyEntityList); }
            }

            internal CollisionEnumerator (List<Entity> curList)
                : this(curList, _noConidition)
            { }

            internal CollisionEnumerator (List<Entity> curList, Func<Entity, bool> condition)
            {
                _index = -1;
                _curList = curList;
                _condition = condition;
            }

            public Entity Current
            {
                get { return _curList[_index]; }
            }

            public void Dispose () { }

            public bool MoveNext ()
            {
                _index++;

                while (_index < _curList.Count) {
                    Entity e = _curList[_index];
                    if (_condition(e))
                        return true;

                    _index++;
                }

                return false;
            }

            public void Reset ()
            {
                _index = -1;
            }

            public CollisionEnumerator GetEnumerator ()
            {
                return this;
            }
        }

        public struct CollisionStartEnumerator
        {
            private static List<Entity> _emptyEntityList = new List<Entity>();
            private static Func<Entity, bool> _noConidition = (e) => { return true; };

            private List<Entity> _curList;
            private List<Entity> _prevList;

            private int _index;
            private Func<Entity, bool> _condition;

            internal static CollisionStartEnumerator Empty
            {
                get { return new CollisionStartEnumerator(_emptyEntityList, _emptyEntityList); }
            }

            internal CollisionStartEnumerator (List<Entity> curList, List<Entity> prevList)
                : this(curList, prevList, _noConidition)
            { }

            internal CollisionStartEnumerator (List<Entity> curList, List<Entity> prevList, Func<Entity, bool> condition)
            {
                _index = -1;
                _curList = curList;
                _prevList = prevList;
                _condition = condition;
            }

            public Entity Current
            {
                get { return _curList[_index]; }
            }

            public void Dispose () { }

            public bool MoveNext ()
            {
                _index++;

                while (_index < _curList.Count) {
                    Entity e = _curList[_index];
                    if (!_prevList.Contains(e) && _condition(e))
                        return true;

                    _index++;
                }

                return false;
            }

            public void Reset ()
            {
                _index = -1;
            }

            public CollisionStartEnumerator GetEnumerator ()
            {
                return this;
            }
        }

        /*public struct EntityEnumerator
        {
            private static List<Entity> _emptyEntityList = new List<Entity>();

            private List<Entity> _entList;
            private int _index;
            private Func<Entity, bool> _condition;

            internal static EntityEnumerator Empty
            {
                get { return new EntityEnumerator(_emptyEntityList, null); }
            }

            internal EntityEnumerator (List<Entity> entityList, Func<Entity, bool> condition)
            {
                _index = -1;
                _entList = entityList;
                _condition = condition;
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
                do {
                    _index++;
                } while (_index < _entList.Count && !_condition(_entList[_index]));

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
        }*/
    }
}
