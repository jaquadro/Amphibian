using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amphibian.Geometry;

namespace Amphibian.Behaviors
{
    public class CircleMovement : Behavior
    {
        private PointFP _center;
        private FPInt _radius;
        private float _speed;

        private float _time;

        private GameObject _object;

        public CircleMovement (GameObject obj, PointFP center, FPInt radius, float radPerSecond)
            : base()
        {
            _object = obj;

            _center = center;
            _radius = radius;
            _speed = radPerSecond;
        }

        public override void Execute ()
        {
            _time = (float)_object.Parent.Engine.GameTime.TotalGameTime.TotalSeconds * _speed;

            float sin = (float)Math.Sin(_time);
            float cos = (float)Math.Cos(_time);

            FPInt nx = (FPInt)((float)_radius * cos);
            FPInt ny = (FPInt)((float)_radius * sin);

            _object.X = _center.X + nx;
            _object.Y = _center.Y + ny;
        }
    }
}
