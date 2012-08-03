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
    public class DynamicCollisionSystem : BaseSystem
    {
        private EntityCollisionManager _manager;

        private ResourcePool<List<Entity>> _listPool;
        private Dictionary<Entity, List<Entity>> _prevPairs;
        private Dictionary<Entity, List<Entity>> _curPairs;

        public DynamicCollisionSystem ()
        {
            _manager = new EntityCollisionManager();
            _manager.FineCollision += FineCollisionHandler;

            _listPool = new ResourcePool<List<Entity>>();
            _prevPairs = new Dictionary<Entity, List<Entity>>();
            _curPairs = new Dictionary<Entity, List<Entity>>();
        }

        public bool Overlaps (Entity e1, Entity e2)
        {
            List<Entity> list;
            if (!_curPairs.TryGetValue(e1, out list) || !list.Contains(e2))
                return false;

            return true;
        }

        public bool CollisionStarted (Entity e1, Entity e2)
        {
            List<Entity> list;
            if (!_prevPairs.TryGetValue(e1, out list) || list.Contains(e2))
                return false;

            if (!_curPairs.TryGetValue(e1, out list) || !list.Contains(e2))
                return false;

            return true;
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

            if (!_curPairs.TryGetValue(e1, out list) || list.Contains(e2))
                return false;

            return true;
        }

        protected internal override void Initialize ()
        {
            EntityManager.AddedComponent += ComponentAttachHandler;
            EntityManager.RemovedComponent += ComponentDetachHandler;
        }

        protected override void ProcessInner ()
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
    }
}
