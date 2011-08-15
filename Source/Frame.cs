using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Collision;
using TiledLib;
using Amphibian.Drawing;
using Amphibian.Geometry;

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

        private SamplerState _samplerState;

        private Dictionary<string, List<Component>> _comDict;

        // For now, just one collision layer -- could add more later
        private TileCollisionManager _tileColManager;
        private Map _map;
        private List<TileLayer> _layers;

        public Frame ()
        {
            _components = new List<Component>();
            _updating = new List<Component>();
            _comDict = new Dictionary<string, List<Component>>();

            _samplerState = new SamplerState()
            {
                Filter = TextureFilter.Point
            };
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
            // Load map
            _map = _engine.Content.Load<Map>("purple_caves_lev");
            _layers = new List<TileLayer>();
            _tileColManager = new TileCollisionManager(_map.Width, _map.Height, _map.TileWidth * 2, _map.TileHeight * 2);

            foreach (Layer layer in _map.Layers) {
                if (layer is TileLayer) {
                    Property prop;
                    if (layer.Properties.TryGetValue("type", out prop) && prop.RawValue == "collision") {
                        initColMap(layer as TileLayer);
                    }
                    //else {
                        TileLayer tileLayer = layer as TileLayer;
                        tileLayer.ScaleX = 2f;
                        tileLayer.ScaleY = 2f;

                        _layers.Add(tileLayer);
                    //}
                }
            }
        }

        private void initColMap (TileLayer layer)
        {
            for (int x = 0; x < layer.Width; x++) {
                for (int y = 0; y < layer.Height; y++) {
                    if (layer.Tiles[x, y] != null) {
                        _tileColManager.AddObject(layer.Tiles[x, y].Id - 65, x, y);
                    }
                }
            }
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
            _engine.GraphicsDevice.SamplerStates[0] = _samplerState;

            foreach (Component c in _components) {
                c.Draw();
            }

            foreach (TileLayer tl in _layers) {
                _map.Draw(_engine.SpriteBatch, _camera.Bounds, tl);
            }

            //_tileColManager.Draw(_engine.SpriteBatch, _camera.Bounds);

            _engine.SpriteBatch.End();
        }

        public bool OverlapsBackdrop (Mask mask)
        {
            return _tileColManager.OverlapsAny(mask);
        }

        public bool OverlapsBackdrop (FPInt x, FPInt y)
        {
            return _tileColManager.OverlapsAny(x, y);
        }
    }
}
