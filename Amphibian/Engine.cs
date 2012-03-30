using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Input;
using Amphibian.Drawing;
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

        private List<Frame> _frameStack;
        private List<Frame> _sortFrameStack;

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

            _frameStack = new List<Frame>();
            _sortFrameStack = new List<Frame>();

            _input = new Dictionary<string, InputController>();

            // Default Values
            _simulationStep = 1 / 60.0;
            _prevTime = new GameTime(TimeSpan.Zero, TimeSpan.Zero);

            Brushes.Initialize(_graphicsDevice);
            Pens.Initialize(_graphicsDevice);
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

        public void PushFrame (Frame frame) {
            if (frame.Engine != null) {
                throw new Exception("Frame already bound to engine");
            }

            if (!_frameStack.Contains(frame)) {
                _frameStack.Add(frame);

                frame.Engine = this;
                frame.LoadFrame();
            }
        }

        public Frame PopFrame ()
        {
            if (_frameStack.Count == 0) {
                return null;
            }

            Frame frame = _frameStack[_frameStack.Count - 1];
            _frameStack.RemoveAt(_frameStack.Count - 1);

            frame.Engine = null;

            return frame;
        }

        public void Update (GameTime gameTime)
        {
            _updateAccumulator += gameTime.ElapsedGameTime.TotalSeconds;

            TimeSpan simStart = _prevTime.TotalGameTime;
            TimeSpan simSpan = TimeSpan.FromSeconds(_simulationStep);

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
            for (int i = _frameStack.Count - 1; i >= 0; i--) {
                _sortFrameStack.Add(_frameStack[i]);
                if (_frameStack[i].BlocksUpdates) {
                    break;
                }
            }

            // Update each frame
            foreach (Frame frame in _sortFrameStack) {
                frame.Update();
            }

            // Reset controllers
            foreach (InputController controller in _input.Values) {
                controller.Reset();
            }

            _sortFrameStack.Clear();
        }

        protected void InterpolateStep (double alpha)
        {
            // Determine update depth of frame stack
            for (int i = _frameStack.Count - 1; i >= 0; i--) {
                _sortFrameStack.Add(_frameStack[i]);
                if (_frameStack[i].BlocksUpdates) {
                    break;
                }
            }

            // Update each frame
            foreach (Frame frame in _sortFrameStack) {
                frame.Interpolate(alpha);
            }

            _sortFrameStack.Clear();
        }

        public void Draw (GameTime gameTime)
        {
            _gameTime.Copy(gameTime);

            for (int i = _frameStack.Count - 1; i >= 0; i--) {
                _sortFrameStack.Add(_frameStack[i]);
                if (_frameStack[i].BlocksDrawing) {
                    break;
                }
            }

            foreach (Frame frame in _sortFrameStack) {
                frame.Draw();
            }

            _sortFrameStack.Clear();
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
            return _input[name];
        }
    }
}
