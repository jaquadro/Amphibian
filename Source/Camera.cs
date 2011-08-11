﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian
{
    public class Camera : IUpdateable
    {
        #region Private Fields

        private Rectangle _view;
        private Frame _frame;

        private bool _enabled = true;
        private int _updatePriority = 100;

        private float _lastTime;
        private float _animOriginX;
        private float _animOriginY;
        private float _animOriginTime;
        private float _animDestX;
        private float _animDestY;
        private float _animDestTime;

        #endregion

        public Camera (Frame frame, Rectangle view)
        {
            _frame = frame;
            _view = view;
        }

        public Camera (Frame frame, int width, int height)
        {
            _frame = frame;
            _view = new Rectangle(0, 0, width, height);
        }

        #region Properties

        public int X
        {
            get { return _view.X + (_view.Width >> 1); }
            set
            {
                value = value - (_view.Width >> 1);
                _view.X = Math.Min(Math.Max(value, 0), _frame.Width - _view.Width);
            }
        }

        public int Y
        {
            get { return _view.Y + (_view.Height >> 1); }
            set
            {
                value = value - (_view.Height >> 1);
                _view.Y = Math.Min(Math.Max(value, 0), _frame.Height - _view.Height);
            }
        }

        public int Left
        {
            get { return _view.Left; }
        }

        public int Right
        {
            get { return _view.Right; }
        }

        public int Top
        {
            get { return _view.Top; }
        }

        public int Bottom
        {
            get { return _view.Bottom; }
        }

        public Rectangle Bounds
        {
            get { return _view; }
        }

        #endregion

        public void ScrollTo (int x, int y)
        {
            x = x - (_view.Width >> 1);
            y = y - (_view.Height >> 1);

            _view.X = Math.Min(Math.Max(x, 0), _frame.Width - _view.Width);
            _view.Y = Math.Min(Math.Max(y, 0), _frame.Height - _view.Height);
        }

        public void ScrollByDuration (int x, int y, float duration)
        {
            _animOriginX = X;
            _animOriginY = Y;
            _animDestX = x;
            _animDestY = y;

            _animOriginTime = _lastTime;
            _animDestTime = _lastTime + duration;
        }

        public void ScrollBySpeed (int x, int y, float pxPerSecond)
        {
            _animOriginX = X;
            _animOriginY = Y;
            _animDestX = x;
            _animDestY = y;

            Vector2 p1 = new Vector2(_animOriginX);
            Vector2 p2 = new Vector2(_animOriginY);
            float dist = Vector2.Distance(p1, p2);

            _animOriginTime = _lastTime;
            _animDestTime = _lastTime + (dist / pxPerSecond);
        }

        public void CancelScroll ()
        {
            _animOriginX = 0;
            _animOriginY = 0;
            _animOriginTime = 0;
            _animDestX = 0;
            _animDestY = 0;
            _animDestTime = 0;
        }

        public Matrix GetTranslationMatrix ()
        {
            return Matrix.CreateTranslation(-_view.X, -_view.Y, 0);
        }

        #region Private Implementation

        // Updates the camera to a position along a line between two points given a time step.
        private void AnimateLine ()
        {
            float range = _animDestTime - _animOriginTime;
            float elapsed = _lastTime - _animOriginTime;
            float normalized = elapsed / range;

            float x = _animOriginX + (_animDestX - _animOriginX) * normalized;
            float y = _animOriginY + (_animDestY - _animOriginY) * normalized;

            ScrollTo((int)x, (int)y);
        }

        #endregion

        #region IUpdateable Members

        public bool Enabled
        {
            get { return _enabled; }
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public void Update (GameTime gameTime)
        {
            _lastTime = (float)gameTime.TotalGameTime.TotalSeconds;

            if (_lastTime > _animOriginTime && _lastTime < _animDestTime) {
                AnimateLine();
            }
        }

        public int UpdateOrder
        {
            get { return _updatePriority; }
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}