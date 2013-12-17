﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Amphibian.EntitySystem
{
    // NB: Systems should consider caching entities, then they would be faster
    // May lend support to restructing EntityManager agian

    public abstract class BaseSystem
    {
        private bool _enabled = true;

        private EntityManager _entityManager;
        private SystemManager _systemManager;

        public BaseSystem ()
        {
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
                ProcessInner();
                End();
            }
        }

        protected virtual void ProcessInner () { }

        protected virtual void End () { }

        protected virtual bool CheckProcessing ()
        {
            return _enabled;
        }

        private void InitRequiredSystems ()
        {
            foreach (var propInfo in this.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.NonPublic)) {
                if (propInfo.CanWrite && Attribute.IsDefined(propInfo, typeof(RequiredSystemAttribute))) {
                    BaseSystem sys = SystemManager.GetSystem(propInfo.PropertyType);
                    if (sys != null) {
                        propInfo.SetValue(this, sys, null);
                        continue;
                    }

                    SystemManager.RegisterSystemAddedHandler(propInfo.PropertyType, HandleRequiredSystemAdded);
                }
            }
        }

        private void HandleRequiredSystemAdded (object system)
        {
            Type systemType = system.GetType();
            while (systemType != null) {
                foreach (var propInfo in this.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.NonPublic)) {
                    if (propInfo.CanWrite && Attribute.IsDefined(propInfo, typeof(RequiredSystemAttribute))) {
                        if (propInfo.PropertyType == systemType) {
                            propInfo.SetValue(this, system, null);
                            return;
                        }
                    }
                }

                systemType = systemType.BaseType;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredSystemAttribute : Attribute
    { }
}
