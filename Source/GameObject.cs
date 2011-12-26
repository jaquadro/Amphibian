using System;
using System.Collections.Generic;
using Amphibian.Geometry;

namespace Amphibian
{
    public class GameObject : Component
    {
        protected SharedPointFP _position;
        private SharedPointFP _renderAt;
        private bool _useRenderAt;

        protected List<Behavior> _behaviors;
        protected List<InterpBehavior> _interpBehaviors;

        public GameObject ()
            : base()
        {
            _position = new SharedPointFP(0, 0);
            _renderAt = new SharedPointFP(0, 0);
            _behaviors = new List<Behavior>();
            _interpBehaviors = new List<InterpBehavior>();
        }

        #region Properties

        public FPInt X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public FPInt Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        internal SharedPointFP Position
        {
            get { return _position; }
        }

        /*internal SharedPointFP RenderAt
        {
            get { return _renderAt; }
            set { _renderAt = value; }
        }*/

        internal PointFP RenderPosition
        {
            get { return _useRenderAt ? _renderAt : _position; }
        }

        #endregion

        internal void RenderAt (FPInt x, FPInt y)
        {
            _useRenderAt = true;
            _renderAt.X = x;
            _renderAt.Y = y;
        }

        internal void RenderAtPosition ()
        {
            _useRenderAt = false;
        }

        public void AddBehavior (Behavior behavior)
        {
            Type bType = behavior.GetType();
            foreach (Behavior b in _behaviors) {
                if (bType == _behaviors.GetType()) {
                    return;
                }
            }

            _behaviors.Add(behavior);

            if (behavior is InterpBehavior) {
                _interpBehaviors.Add(behavior as InterpBehavior);
            }
        }

        public void RemoveBehavior (Behavior behavior)
        {
            RemoveBehavior(behavior.GetType());
        }

        public void RemoveBehavior (InterpBehavior behavior)
        {
            RemoveBehavior(behavior.GetType());
        }

        public void RemoveBehavior (Type behaviorType)
        {
            foreach (Behavior b in _behaviors) {
                if (behaviorType == b.GetType()) {
                    _behaviors.Remove(b);
                    return;
                }
            }

            foreach (InterpBehavior b in _interpBehaviors) {
                if (behaviorType == b.GetType()) {
                    _interpBehaviors.Remove(b);
                    return;
                }
            }
        }

        public override void Update ()
        {
            base.Update();

            foreach (Behavior behavior in _behaviors) {
                behavior.Execute();
            }
        }

        public override void Interpolate (double alpha)
        {
            base.Update();

            foreach (InterpBehavior behavior in _interpBehaviors) {
                behavior.Interpolate(alpha);
            }
        }

        public void Offset (FPInt offsetX, FPInt offsetY)
        {
            _position.X += offsetX;
            _position.Y += offsetY;
        }
    }
}
