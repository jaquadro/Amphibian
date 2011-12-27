using System;
using System.Collections.Generic;

namespace Amphibian.EntitySystem
{
    public sealed class TagManager
    {
        private EntityWorld _world;
        private Dictionary<string, Entity> _entities;

        public TagManager (EntityWorld world)
        {
            _world = world;
            _entities = new Dictionary<string, Entity>();
        }

        public void Register (string tag, Entity e)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            _entities[tag] = e;
        }

        public void Unregister (string tag)
        {
            _entities.Remove(tag);
        }

        public bool IsRegistered (string tag)
        {
            return _entities.ContainsKey(tag);
        }

        public Entity GetEntity (string tag)
        {
            Entity entity;
            _entities.TryGetValue(tag, out entity);

            if (_world.EntityManager.IsValid(entity)) {
                return entity;
            }
            else {
                _entities.Remove(tag);
                return new Entity();
            }
        }
    }
}
