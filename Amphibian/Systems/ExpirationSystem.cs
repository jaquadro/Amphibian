using System.Collections.Generic;
using Amphibian.Components;
using Amphibian.EntitySystem;

namespace Amphibian.Systems
{
    public class ExpirationSystem : ProcessingSystem<RemovalTimeout>
    {
        private Queue<Entity> _purgeQueue = new Queue<Entity>();

        protected override void Process (Entity entity, RemovalTimeout timeoutCom)
        {
            float time = (float)SystemManager.World.GameTime.ElapsedGameTime.TotalSeconds;
            timeoutCom.TimeRemaining -= time;

            if (timeoutCom.TimeRemaining <= 0)
                _purgeQueue.Enqueue(entity);
        }

        protected override void End ()
        {
            while (_purgeQueue.Count > 0)
                EntityManager.Destroy(_purgeQueue.Dequeue());
        }
    }
}
