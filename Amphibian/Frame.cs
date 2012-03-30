﻿using System;
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
    public abstract class Frame
    {
        private Engine _engine;
        private bool _loaded;

        private bool _blocksUpdates;
        private bool _blocksDrawing;
        private bool _blocksTime;

        private int _width;
        private int _height;

        private SamplerState _samplerState;

        public Frame ()
        {
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

        public bool Loaded
        {
            get { return _loaded; }
        }

        #endregion

        public void LoadFrame ()
        {
            if (!_loaded) {
                Load();
            }

            _loaded = true;
        }

        protected virtual void Load () { }

        public abstract void Update ();

        public virtual void Interpolate (double alpha) { }

        public abstract void Draw ();
    }

    public class EntityFrame : Frame
    {
        private EntityWorld _entityWorld;

        public EntityFrame ()
            : base()
        {
            _entityWorld = new EntityWorld();
            _entityWorld.Frame = this;
        }

        public EntityFrame (Engine engine)
            : base(engine)
        {
            _entityWorld = new EntityWorld();
            _entityWorld.Frame = this;
        }

        public EntityWorld EntityWorld
        {
            get { return _entityWorld; }
        }

        public override void Update ()
        {
            _entityWorld.GameTime = Engine.GameTime;
            _entityWorld.SystemManager.Update(ExecutionType.Update);
        }

        public override void Draw ()
        {
            _entityWorld.SystemManager.Update(ExecutionType.Draw);
        }
    }
}