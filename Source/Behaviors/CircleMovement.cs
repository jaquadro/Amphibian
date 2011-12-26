using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amphibian.Geometry;

namespace Amphibian.Behaviors
{
    public class CircleMovement : InterpBehavior
    {
        private class State
        {
            public FPInt ObjectX;
            public FPInt ObjectY;
        }

        private PointFP _center;
        private FPInt _radius;
        private float _speed;

        private State _current;
        private State _prev;

        private float _time;

        private GameObject _object;

        public CircleMovement (GameObject obj, PointFP center, FPInt radius, float radPerSecond)
            : base()
        {
            _object = obj;

            _current = new State();
            _prev = new State();

            _center = center;
            _radius = radius;
            _speed = radPerSecond;
        }

        public override void Execute ()
        {
            _prev = _current;

            _time = (float)_object.Parent.Engine.GameTime.TotalGameTime.TotalSeconds * _speed;

            float sin = (float)Math.Sin(_time);
            float cos = (float)Math.Cos(_time);

            FPInt nx = (FPInt)((float)_radius * cos);
            FPInt ny = (FPInt)((float)_radius * sin);

            _object.X = _center.X + nx;
            _object.Y = _center.Y + ny;

            _prev = _current;
            _current.ObjectX = _object.X;
            _current.ObjectY = _object.Y;

            _object.RenderAtPosition();
        }

        public override void Interpolate (double alpha)
        {
            FPInt midx = _current.ObjectX * (FPInt)alpha + _prev.ObjectX * (FPInt)(1.0 - alpha);
            FPInt midy = _current.ObjectY * (FPInt)alpha + _prev.ObjectY * (FPInt)(1.0 - alpha);

            _object.RenderAt(midx, midy);
        }
    }
}
