using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Amphibian.Behaviors
{
    public class CircleMovement : Behavior
    {
        private Vector2 _center;
        private float _radius;
        private float _speed;

        private float _time;

        private GameObject _object;

        public CircleMovement (GameObject obj, Vector2 center, float radius, float radPerSecond)
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

            _object.X = (float)Math.Floor(_center.X + (_radius * cos));
            _object.Y = (float)Math.Floor(_center.Y + (_radius * sin));
        }
    }
}
