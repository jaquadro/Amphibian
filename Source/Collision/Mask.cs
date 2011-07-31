using System;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    public enum MaskType
    {
        Point,
        Circle,
        AXLine,
        AYLine,
        Line,
        AABB,
        Triangle
    }

    public abstract class Mask
    {
        internal MaskType _type;
        internal Vector2 _pos;

        public abstract bool TestOverlap (Mask mask);

        public abstract TestResult TestOverlapExt (Mask mask);

        public Vector2 Position
        {
            get { return _pos; }
            set { _pos = value; }
        }
    }
}
