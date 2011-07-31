using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// What does a frame encompass?
// - General frame data (dimensions, etc)
// - Camera
// - Level objects
// - Tile map(s)
// - Backdrops
// - Level Logic

namespace Amphibian
{
    /// <summary>
    /// Represents a full frame (level)
    /// </summary>
    public class Frame
    {
        private Engine _engine;
        private bool _loaded;

        private bool _blocksUpdates;
        private bool _blocksDrawing;
        private bool _blocksTime;

        private int _width;
        private int _height;

        private Camera _camera;

        private List<Component> _components;
        private List<Component> _updating;

        public Frame ()
        {
            _components = new List<Component>();
            _updating = new List<Component>();
        }

        public Frame (Engine engine)
            : this()
        {
            engine.PushFrame(this);

            _camera = new Camera(this, engine.GraphicsDevice.Viewport.Bounds);
        }

        #region Properties

        public Engine Engine
        {
            get { return _engine; }
            internal set
            {
                _engine = value;
                if (_engine != null && _camera == null) {
                    _camera = new Camera(this, value.GraphicsDevice.Viewport.Bounds);
                }
            }
        }

        public bool BlocksUpdates
        {
            get { return _blocksUpdates; }
            set { _blocksUpdates = value; }
        }

        public bool BlocksDrawing
        {
            get { return _blocksDrawing; }
            set { _blocksDrawing = value; }
        }

        public bool BlocksTime
        {
            get { return _blocksTime; }
            set { _blocksTime = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public Camera Camera
        {
            get { return _camera; }
        }

        public Component this[string name]
        {
            get
            {
                foreach (Component c in _components) {
                    if (c.Name == name) {
                        return c;
                    }
                }
                return null;
            }
        }

        public bool Loaded
        {
            get { return _loaded; }
        }

        #endregion

        public void AddComponent (Component component)
        {
            if (!_components.Contains(component)) {
                _components.Add(component);

                component.Parent = this;
                component.LoadComponent();

                if (_components.Count >= 2 && component.DrawOrder < _components[_components.Count - 2].DrawOrder) {
                    OrderComponent(component);
                }
            }
        }

        public void RemoveComponent (string name)
        {
            Component c = this[name];
            RemoveComponent(c);
        }

        public void RemoveComponent (Component component)
        {
            if (component != null && _components.Contains(component)) {
                _components.Remove(component);
                component.Parent = null;
            }
        }

        internal void OrderComponent (Component component)
        {
            if (_components.Remove(component)) {
                int i = 0;
                for (; i < _components.Count; i++) {
                    if (_components[i].DrawOrder >= component.DrawOrder) {
                        break;
                    }
                }

                _components.Insert(i, component);
            }
        }

        protected virtual void Load ()
        {

        }

        public void LoadFrame ()
        {
            if (!_loaded) {
                Load();
            }

            _loaded = true;
        }

        public virtual void Update ()
        {
            foreach (Component c in _components) {
                _updating.Add(c);
            }

            foreach (Component c in _updating) {
                c.Update();
            }

            _updating.Clear();
        }

        public virtual void Draw ()
        {
            _engine.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, _camera.GetTranslationMatrix());

            foreach (Component c in _components) {
                c.Draw();
            }

            _engine.SpriteBatch.End();
        }
    }
}
