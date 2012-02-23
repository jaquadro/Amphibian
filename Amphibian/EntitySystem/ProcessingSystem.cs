using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    public abstract class ProcessingSystem : BaseSystem
    {
        private Type _primaryKey;

        public ProcessingSystem (Type primaryKey)
        {
            _primaryKey = primaryKey;
        }

        protected override void ProcessInner ()
        {
            ProcessEntities(EntityManager.GetEntities(_primaryKey));
        }

        protected virtual void ProcessEntities (EntityManager.EntityEnumerator entities) { }
    }
}
