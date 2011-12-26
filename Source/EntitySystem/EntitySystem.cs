using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    // NB: Systems should consider caching entities, then they would be faster
    // May lend support to restructing EntityManager agian

    public abstract class BaseSystem
    {
        private bool _enabled = true;

        private Type _primaryKey;
        private EntityManager _entityManager;
        private SystemManager _systemManager;

        public BaseSystem (Type primaryKey)
        {
            _primaryKey = primaryKey;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        protected internal EntityManager EntityManager
        {
            get { return _entityManager; }
            internal set { _entityManager = value; }
        }

        protected internal SystemManager SystemManager
        {
            get { return _systemManager; }
            internal set { _systemManager = value; }
        }

        protected internal virtual void Initialize () { }

        protected virtual void Begin () { }

        public virtual void Process ()
        {
            if (CheckProcessing()) {
                Begin();
                ProcessEntities(_entityManager.GetEntities(_primaryKey));
                End();
            }
        }

        protected virtual void End () { }

        protected virtual void ProcessEntities (EntityManager.EntityEnumerator entities) { }

        protected virtual bool CheckProcessing ()
        {
            return _enabled;
        }
    }
}
