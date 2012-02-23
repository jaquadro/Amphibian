using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Collision;
//using TiledLib;
using Amphibian.Drawing;
using Amphibian.Geometry;
using Treefrog.Runtime;
using Amphibian.Debug;
using Amphibian.EntitySystem;
using Amphibian.Systems;

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

        private EntityWorld _entityWorld;

        private SamplerState _samplerState;

        // For now, just one collision layer -- could add more later
        private TileCollisionManager _tileColManager;
        private CollisionManager _colManager;

        private Level _level;
        private List<TileLayer> _layers = new List<TileLayer>();

        public Frame ()
        {
            _entityWorld = new EntityWorld();
            _entityWorld.Frame = this;

            _colManager = new CollisionManager();

            _samplerState = new SamplerState()
            {
                Filter = TextureFilter.Point
            };
        }

        public Frame (Engine engine)
            : this()
        {
            engine.PushFrame(this);
        }

        #region Properties

        public Engine Engine
        {
            get { return _engine; }
            internal set
            {
                _engine = value;
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

        public EntityWorld EntityWorld
        {
            get { return _entityWorld; }
        }

        public bool Loaded
        {
            get { return _loaded; }
        }

        public CollisionManager CollisionManager
        {
            get { return _colManager; }
        }

        #endregion

        protected virtual void Load ()
        {
            LevelIndex index = _engine.Content.Load<LevelIndex>("proto1");
            _level = _engine.Content.Load<Level>(index.ByName("Level 1").Asset);
            _level.ScaleX = 2f;
            _level.ScaleY = 2f;

            _width = _level.Width * 2;
            _height = _level.Height * 2;

            foreach (Layer layer in _level.Layers) {
                TileLayer tileLayer = layer as TileLayer;
                if (tileLayer != null) {
                    _tileColManager = new TileCollisionManager(tileLayer.Width, tileLayer.Height, tileLayer.TileWidth * 2, tileLayer.TileHeight * 2);
                    break;
                }
            }

            foreach (Layer layer in _level.Layers) {
                if (layer.Properties["type"] != null && layer.Properties["type"].Value == "collision") {
                    TileLayer tileLayer = layer as TileLayer;
                    if (tileLayer != null) {
                        initColMap(tileLayer);
                    }
                }
            }
        }

        private void initColMap (TileLayer layer)
        {
            for (int x = 0; x < layer.Width; x++) {
                for (int y = 0; y < layer.Height; y++) {
                    if (layer.Tiles[x, y] != null) {
                        foreach (Tile tile in layer.Tiles[x, y]) {
                            Property poolType = tile.Tileset.Properties["type"];
                            Property tileTid = tile.Properties["tid"];
                            if (poolType != null && poolType.Value == "collision" && tileTid != null) {
                                _tileColManager.AddObject((int)tileTid, x, y);
                            }
                        }
                    }
                }
            }
        }

        public void InitES ()
        {
            BackgroundCollisionSystem bcs = _entityWorld.SystemManager.GetSystem(typeof(BackgroundCollisionSystem)) as BackgroundCollisionSystem;
            if (bcs != null)
                bcs.TileCollisionManager = _tileColManager;
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
            //using (new PerformanceRuler(1, "Dynamic Collision", Color.Orange)) {
                _colManager.Update();
            //}

            _entityWorld.GameTime = _engine.GameTime;
            _entityWorld.SystemManager.Update(ExecutionType.Update);
        }

        public virtual void Interpolate (double alpha)
        {

        }

        public virtual void Draw ()
        {
            CameraSystem cameraSys = _entityWorld.SystemManager.GetSystem(typeof(CameraSystem)) as CameraSystem;
            if (cameraSys == null)
                return;

            _engine.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, cameraSys.GetTranslationMatrix());
            _engine.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            if (ADebug.RenderCollisionGeometry) {
                _tileColManager.Draw(_engine.SpriteBatch, cameraSys.Bounds);
            }

            _engine.SpriteBatch.End();

            _entityWorld.SystemManager.Update(ExecutionType.Draw);
        }

        public bool TestBackdrop (Mask mask)
        {
            return _tileColManager.OverlapsAny(mask);
        }

        public bool TestBackdropEdge (Mask mask)
        {
            return _tileColManager.OverlapsEdgeAny(mask);
        }

        public bool TestBackdrop (FPInt x, FPInt y)
        {
            return _tileColManager.OverlapsAny(x, y);
        }

        public bool TestBackdropEdge (FPInt x, FPInt y)
        {
            return _tileColManager.OverlapsEdgeAny(x, y);
        }

        public bool TestBackdrop (AXLine line)
        {
            return _tileColManager.OverlapsAny(line);
        }

        public bool TestBackdropEdge (AXLine line)
        {
            return _tileColManager.OverlapsEdgeAny(line);
        }
    }
}
