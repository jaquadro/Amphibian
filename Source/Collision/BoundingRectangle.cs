using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    public struct BoundingRectangle
    {
        private float _x1;
        private float _y1;
        private float _x2;
        private float _y2;

        public BoundingRectangle (float x, float y, float width, float height)
        {
            _x1 = x;
            _y1 = y;
            _x2 = x + width;
            _y2 = y + height;
        }

        public float Left
        {
            get { return _x1; }
        }

        public float Top
        {
            get { return _y1; }
        }

        public float Right
        {
            get { return _x2; }
        }

        public float Bottom
        {
            get { return _y2; }
        }

        public float Height
        {
            get { return _y2 - _y1; }
        }

        public float Width
        {
            get { return _x2 - _x1; }
        }

        public bool Overlaps (BoundingRectangle rect)
        {
            return !(_x1 > rect._x2 || rect._x1 > _x2 || _y1 > rect._y2 || rect._y1 > _y2);
        }
    }
}
