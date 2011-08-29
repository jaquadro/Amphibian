using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Amphibian
{
    public class Component
    {
        private static string _className;

        private string _name;
        private Frame _parent;

        private int _drawOrder;
        private bool _loaded;

        #region Properties

        public string ClassName
        {
            get { return _className ?? this.GetType().Name; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) {
                    throw new ArgumentException("Component must be assigned a valid name");
                }
                _name = value;
            }
        }

        public Frame Parent
        {
            get { return _parent; }
            set
            {
                if (value == _parent) {
                    return;
                }

                if (_parent != null) {
                    _parent.RemoveComponent(this);
                }

                _parent = value;

                if (value != null) {
                    _parent.AddComponent(this);
                }
            }
        }

        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                _drawOrder = value;
                if (_parent != null) {
                    _parent.OrderComponent(this);
                }
            }
        }

        public bool Loaded
        {
            get { return _loaded; }
        }

        #endregion

        protected virtual void Load ()
        {

        }

        public void LoadComponent ()
        {
            if (!_loaded) {
                Load();
            }

            _loaded = true;
        }

        public virtual void Update ()
        {

        }

        public virtual void Interpolate (double alpha)
        {

        }

        public virtual void Draw ()
        {

        }

        static private Dictionary<Type, int> _componentCounts = new Dictionary<Type, int>();

        private void GenerateUniqueName ()
        {
            Type t = this.GetType();

            if (!_componentCounts.ContainsKey(t)) {
                _componentCounts.Add(t, 0);
            }

            _componentCounts[t]++;

            _name = t.Name + _componentCounts[t];
        }
    }
}
