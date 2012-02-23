using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Components;
using Amphibian.Utility;

namespace Amphibian.Systems
{
    public class ExpirationSystem : ProcessingSystem
    {
        private Queue<Entity> _purgeQueue;

        public ExpirationSystem ()
            : base(typeof(RemovalTimeout))
        {
            _purgeQueue = new Queue<Entity>();
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities) {
                Process(entity);
            }

            while (_purgeQueue.Count > 0) {
                EntityManager.Destroy(_purgeQueue.Dequeue());
            }
        }

        private void Process (Entity entity)
        {
            RemovalTimeout timeout = EntityManager.GetComponent(entity, typeof(RemovalTimeout)) as RemovalTimeout;
            if (timeout == null)
                return;

            float time = (float)SystemManager.World.GameTime.ElapsedGameTime.TotalSeconds;
            timeout.TimeRemaining -= time;

            if (timeout.TimeRemaining <= 0) {
                _purgeQueue.Enqueue(entity);
            }
        }
    }
}
