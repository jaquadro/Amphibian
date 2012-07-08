﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Utility;

namespace Amphibian.EntitySystem
{
    public enum ExecutionType
    {
        Draw,
        Update,
    }

    internal sealed class SystemGroupKey : IComparable<SystemGroupKey>
    {
        private static int _nextId;

        private readonly int _id;

        public readonly int Key;

        public SystemGroupKey (int key)
        {
            Key = key;
            _id = _nextId++;
        }

        #region IComparable<SystemGroupKey> Members

        public int CompareTo (SystemGroupKey other)
        {
            int result = Key - other.Key;
            if (result == 0)
                result = _id - other._id;
            return result;
        }

        #endregion
    }

    public sealed class SystemManager
    {
        private EntityWorld _world;

        private Dictionary<Type, BaseSystem> _systems;

        private List<BaseSystem> _drawSystems;
        private List<BaseSystem> _updateSystems;

        public event Action<BaseSystem> SystemAdded;

        public SystemManager (EntityWorld world)
        {
            _world = world;

            _systems = new Dictionary<Type, BaseSystem>();
            _drawSystems = new List<BaseSystem>();
            _updateSystems = new List<BaseSystem>();
        }

        public EntityWorld World
        {
            get { return _world; }
        }

        public void AddSystem (BaseSystem system, ExecutionType execType)
        {
            AddSystem(system.GetType(), system, execType, 0);
        }

        public void AddSystem<T> (BaseSystem system, ExecutionType execType)
        {
            AddSystem(typeof(T), system, execType, 0);
        }

        public void AddSystem (BaseSystem system, ExecutionType execType, int priority)
        {
            AddSystem(system.GetType(), system, execType, priority);
        }

        public void AddSystem<T> (BaseSystem system, ExecutionType execType, int priority)
        {
            AddSystem(typeof(T), system, execType, priority);
        }

        private void AddSystem (Type systemType, BaseSystem system, ExecutionType execType, int priority)
        {
            if (_systems.ContainsKey(systemType)) {
                return;
            }

            system.SystemManager = this;
            system.EntityManager = _world.EntityManager;

            _systems.Add(systemType, system);
            switch (execType) {
                case ExecutionType.Draw:
                    _drawSystems.Add(system);
                    break;

                case ExecutionType.Update:
                    _updateSystems.Add(system);
                    break;
            }

            system.Initialize();

            OnSystemAdded(system);
        }

        public BaseSystem GetSystem (Type systemType) {
            BaseSystem system = null;
            _systems.TryGetValue(systemType, out system);
            return system;
        }

        public void Update (ExecutionType execType)
        {
            switch (execType) {
                case ExecutionType.Draw:
                    for (int i = 0; i < _drawSystems.Count; i++) {
                        _drawSystems[i].Process();
                    }
                    break;

                case ExecutionType.Update:
                    for (int i = 0; i < _updateSystems.Count; i++) {
                        _updateSystems[i].Process();
                    }
                    break;
            }
        }

        private void OnSystemAdded (BaseSystem type)
        {
            if (SystemAdded != null) {
                SystemAdded(type);
            }
        }
    }
}
