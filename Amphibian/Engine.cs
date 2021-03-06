﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Input;
using Amphibian.Debug;

namespace Amphibian
{
    public class ControllerEventArgs : EventArgs
    {
        private string _name;
        private InputController _controller;

        public string Name
        {
            get { return _name; }
        }

        public InputController Controller
        {
            get { return _controller; }
        }

        public ControllerEventArgs (string name, InputController controller)
        {
            _name = name;
            _controller = controller;
        }
    }

    public class Engine
    {
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private GameServiceContainer _services;
        private AmphibianGameTime _gameTime;

        private FrameStack _frameStack;

        private Dictionary<string, InputController> _input;

        private double _simulationStep;
        private double _updateAccumulator;
        private GameTime _prevTime;

        public Engine (IGraphicsDeviceService graphics)
        {
            _gameTime = new AmphibianGameTime();

            _services = new GameServiceContainer();
            _services.AddService(typeof(IGraphicsDeviceService), graphics);
            //_services.AddService(typeof(IGraphicsDeviceManager), graphics);

            _graphicsDevice = graphics.GraphicsDevice;

            _content = new ContentManager(_services);
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            _frameStack = new FrameStack(this);

            _input = new Dictionary<string, InputController>();

            // Default Values
            _simulationStep = 1 / 60.0;
            _prevTime = new GameTime(TimeSpan.Zero, TimeSpan.Zero);

            GameService = new GameServiceAdapter();
        }

        #region Properties

        public ContentManager Content
        {
            get { return _content; }
        }

        public AmphibianGameTime GameTime
        {
            get { return _gameTime; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
        }

        public GameServiceContainer Services
        {
            get { return _services; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public double SimulationStep
        {
            get { return _simulationStep; }
            set { _simulationStep = value; }
        }

        public FrameStack Frames
        {
            get { return _frameStack; }
        }

        public IGameService GameService { get; set; }

        public Action<bool> IsCursorVisibleHandler { get; set; }

        #endregion

        #region Events

        public event EventHandler<ControllerEventArgs> ControllerAdded;
        public event EventHandler<ControllerEventArgs> ControllerRemoved;

        #endregion

        #region Event Handlers

        protected virtual void OnControllerAdded (ControllerEventArgs e)
        {
            if (ControllerAdded != null) {
                ControllerAdded(this, e);
            }
        }

        protected virtual void OnControllerRemoved (ControllerEventArgs e)
        {
            if (ControllerRemoved != null) {
                ControllerRemoved(this, e);
            }
        }

        #endregion

        public void Update (GameTime gameTime)
        {
            _updateAccumulator += gameTime.ElapsedGameTime.TotalSeconds;

            TimeSpan simStart = _prevTime.TotalGameTime;
            TimeSpan simSpan = TimeSpan.FromTicks((int)(10000000 * _simulationStep));

            while (_updateAccumulator >= _simulationStep) {
                simStart.Add(simSpan);

                _gameTime.TotalGameTime = simStart;
                _gameTime.ElapsedGameTime = simSpan;

                UpdateStep();

                _updateAccumulator -= _simulationStep;
            }

            if (_updateAccumulator > 0) {
                double alpha = _updateAccumulator / _simulationStep;
                InterpolateStep(alpha);
            }

            _prevTime = gameTime;
        }

        protected void UpdateStep ()
        {
            // Refresh controllers
            foreach (InputController controller in _input.Values) {
                controller.Refresh();
            }

            ButtonController<DebugAction> c = GetController("debug") as ButtonController<DebugAction>;
            if (c != null) {
                if (c.ButtonPressed(DebugAction.CycleView)) {
                    Performance.AdvanceOutputState();
                }
            }

            // Determine update depth of frame stack
            int depth = _frameStack.Count - 1;
            for (; depth >= 0; depth--) {
                if (_frameStack[depth].BlocksUpdates)
                    break;
            }

            depth = Math.Max(0, depth);

            // Update each frame
            for (int i = _frameStack.Count - 1; i >= depth; i--)
                _frameStack[i].Update();

            // Reset controllers
            foreach (InputController controller in _input.Values)
                controller.Reset();
        }

        public bool Back ()
        {
            for (int depth = _frameStack.Count - 1; depth >= 0; depth--) {
                if (_frameStack[depth].Back())
                    return true;
                depth = Math.Min(depth, _frameStack.Count - 1);
            }

            return false;
        }

        protected void InterpolateStep (double alpha)
        {
            // Determine update depth of frame stack
            int depth = _frameStack.Count - 1;
            for (; depth >= 0; depth--) {
                if (_frameStack[depth].BlocksUpdates)
                    break;
            }

            depth = Math.Max(0, depth);

            // Update each frame
            for (int i = _frameStack.Count - 1; i >= depth; i--)
                _frameStack[i].Interpolate(alpha);
        }

        public void Draw (GameTime gameTime)
        {
            _gameTime.Copy(gameTime);

            // Determine draw depth of frame stack
            int depth = _frameStack.Count - 1;
            for (; depth >= 0; depth--) {
                if (_frameStack[depth].BlocksDrawing) {
                    break;
                }
            }

            depth = Math.Max(0, depth);

            // Draw each frame bottom-up
            for (int i = depth; i < _frameStack.Count; i++)
                _frameStack[i].Draw();
        }

        public void AddController (string name, InputController controller)
        {
            _input[name] = controller;
            OnControllerAdded(new ControllerEventArgs(name, controller));
        }

        public void RemoveController (string name)
        {
            InputController controller;
            if (_input.TryGetValue(name, out controller)) {
                _input.Remove(name);
                OnControllerRemoved(new ControllerEventArgs(name, controller));
            }
        }

        public InputController GetController (string name)
        {
            InputController controller;
            if (_input.TryGetValue(name, out controller))
                return controller;
            else
                return null;
        }

        public T GetController<T>(string name)
            where T : InputController
        {
            return GetController(name) as T;
        }
    }
}
