using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework;
using Amphibian.Components;

namespace Amphibian.Systems
{
    public class CameraSystem : TagSystem
    {
        #region Fields

        private static string _tag = "camera_track";

        private Rectangle _view;
        private Frame _frame;

        private float _lastTime;
        private float _animOriginX;
        private float _animOriginY;
        private float _animOriginTime;
        private int _animDestX;
        private int _animDestY;
        private float _animDestTime;
        private bool _scrolling;

        #endregion

        public CameraSystem (Frame frame, Rectangle view)
            : base(_tag)
        {
            _frame = frame;
            _view = view;

            ClampToFrame = true;
        }

        public CameraSystem (Frame frame, int width, int height)
            : this(frame, new Rectangle(0, 0, width, height))
        {
        }

        public static string Tag
        {
            get { return _tag; }
        }

        protected override void ProcessInner ()
        {
            base.ProcessInner();

            _lastTime = (float)_frame.Engine.GameTime.TotalGameTime.TotalSeconds;

            if (_scrolling) {
                AnimateLine();
            }
        }

        public override void Process (Entity entity)
        {
            Position positionCom = EntityManager.GetComponent(entity, typeof(Position)) as Position;
            if (positionCom != null) {
                ScrollTo(positionCom.X.Floor, positionCom.Y.Floor);
            }
        }

        #region Properties

        public int X
        {
            get { return _view.X + (_view.Width >> 1); }
            set
            {
                PreviousX = X;

                _view.X = value - (_view.Width >> 1);
                if (ClampToFrame)
                    _view.X = Math.Min(Math.Max(_view.X, 0), _frame.Width - _view.Width);
            }
        }

        public int Y
        {
            get { return _view.Y + (_view.Height >> 1); }
            set
            {
                PreviousY = Y;

                _view.Y = value - (_view.Height >> 1);
                if (ClampToFrame)
                    _view.Y = Math.Min(Math.Max(_view.Y, 0), _frame.Height - _view.Height);
            }
        }

        public int PreviousX { get; private set; }
        public int PreviousY { get; private set; }

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

        public bool IsScrolling
        {
            get { return _scrolling; }
        }

        public bool ClampToFrame { get; set; }

        #endregion

        public void ScrollTo (int x, int y)
        {
            X = x;
            Y = y;
        }

        public void JumpTo (int x, int y)
        {
            X = x;
            Y = y;
            PreviousX = X;
            PreviousY = Y;
        }

        public void ScrollByDuration (int x, int y, float duration)
        {
            _animOriginX = X;
            _animOriginY = Y;
            _animDestX = x;
            _animDestY = y;

            _animOriginTime = _lastTime;
            _animDestTime = _lastTime + duration;
            _scrolling = true;
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
            _scrolling = true;
        }

        public void CancelScroll ()
        {
            _animOriginX = 0;
            _animOriginY = 0;
            _animOriginTime = 0;
            _animDestX = 0;
            _animDestY = 0;
            _animDestTime = 0;
            _scrolling = false;
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

            if (normalized >= 1)
            {
                ScrollTo(_animDestX, _animDestY);
                _scrolling = false;
            }
            else
            {

                float x = _animOriginX + (_animDestX - _animOriginX) * normalized;
                float y = _animOriginY + (_animDestY - _animOriginY) * normalized;

                ScrollTo((int)x, (int)y);
            }
        }

        #endregion
    }
}
