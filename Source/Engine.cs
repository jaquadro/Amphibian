using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Input;

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
        private IServiceContainer _services;
        private GameTime _gameTime;

        private List<Frame> _frameStack;
        private List<Frame> _sortFrameStack;

        private Dictionary<string, InputController> _input;

        

        public Engine (GraphicsDeviceManager graphics)
        {
            _services = new ServiceContainer();
            _services.AddService(typeof(IGraphicsDeviceService), graphics);
            _services.AddService(typeof(IGraphicsDeviceManager), graphics);

            _graphicsDevice = graphics.GraphicsDevice;

            _content = new ContentManager(_services);
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            _frameStack = new List<Frame>();
            _sortFrameStack = new List<Frame>();

            _input = new Dictionary<string, InputController>();
        }

        #region Properties

        public ContentManager Content
        {
            get { return _content; }
        }

        public GameTime GameTime
        {
            get { return _gameTime; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
        }

        public IServiceContainer Services
        {
            get { return _services; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
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
            _gameTime = gameTime;

            // Refresh controllers
            foreach (InputController controller in _input.Values) {
                controller.Refresh();
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

        public void Interpolate (double alpha)
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
            _gameTime = gameTime;

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
