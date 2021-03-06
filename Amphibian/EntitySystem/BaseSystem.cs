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

        private List<string> _pendingRequiredSystems = new List<string>();
        private List<string> _pendingOptionalSystems = new List<string>();

        public BaseSystem ()
        { }

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

        protected EntityWorld World
        {
            get { return _systemManager.World; }
        }

        protected EntityFrame Frame
        {
            get { return _systemManager.World.Frame; }
        }

        protected Engine Engine
        {
            get { return _systemManager.World.Frame.Engine; }
        }

        internal void InitializeSystem ()
        {
            InitRequiredSystems();
            Initialize();
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
            return _enabled && _pendingRequiredSystems.Count == 0;
        }

        private void InitRequiredSystems ()
        {
            InitSystems(typeof(RequiredSystemAttribute), _pendingRequiredSystems, HandleRequiredSystemAdded);
        }

        private void InitOptionalSystems ()
        {
            InitSystems(typeof(OptionalSystemAttribute), _pendingOptionalSystems, HandleOptionalSystemAdded);
        }

        private void InitSystems (Type attribType, List<string> pendingList, Action<object> handler)
        {
            foreach (var propInfo in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic)) {
                if (propInfo.CanWrite && Attribute.IsDefined(propInfo, attribType)) {
                    BaseSystem sys = SystemManager.GetSystem(propInfo.PropertyType);
                    if (sys != null) {
                        propInfo.SetValue(this, sys, null);
                        continue;
                    }

                    pendingList.Add(this.GetType().FullName + "." + propInfo.Name);
                    SystemManager.RegisterSystemAddedHandler(propInfo.PropertyType, handler);
                }
            }
        }

        private void HandleRequiredSystemAdded (object system)
        {
            Type systemType = system.GetType();
            while (systemType != null) {
                foreach (var propInfo in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic)) {
                    if (propInfo.CanWrite && Attribute.IsDefined(propInfo, typeof(RequiredSystemAttribute))) {
                        if (propInfo.PropertyType == systemType) {
                            propInfo.SetValue(this, system, null);
                            _pendingRequiredSystems.Remove(this.GetType().FullName + "." + propInfo.Name);
                            return;
                        }
                    }
                }

                systemType = systemType.BaseType;
            }
        }

        private void HandleOptionalSystemAdded (object system)
        {
            Type systemType = system.GetType();
            while (systemType != null) {
                foreach (var propInfo in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic)) {
                    if (propInfo.CanWrite && Attribute.IsDefined(propInfo, typeof(OptionalSystemAttribute))) {
                        if (propInfo.PropertyType == systemType) {
                            propInfo.SetValue(this, system, null);
                            _pendingOptionalSystems.Remove(this.GetType().FullName + "." + propInfo.Name);
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

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalSystemAttribute : Attribute
    { }
}
