using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Drawing;

namespace Amphibian.Collision
{
    public sealed class CircleMask : Mask
    {
        internal Vector2 _p;
        internal float _radius;

        public CircleMask (Vector2 center, float radius)
        {
            _type = MaskType.Circle;
            _p = center;
            _radius = radius;
        }

        public override object Clone ()
        {
            CircleMask mask = new CircleMask(_p, _radius);
            mask._pos = _pos;

            return mask;
        }

        public override BoundingRectangle Bounds
        {
            get { return new BoundingRectangle(_pos.X + _p.X - (_radius * 0.5f), _pos.Y + _p.Y - (_radius * 0.5f), _radius, _radius); }
        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            Primitives2D.DrawCircle(spriteBatch, _pos + _p, _radius, 24, Color.White);
        }

        public override bool TestOverlap (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return Collision.TestOverlap(mask as PointMask, this);
                case MaskType.Circle:
                    return Collision.TestOverlap(this, mask as CircleMask);
                case MaskType.AXLine:
                    return Collision.TestOverlap(this, mask as AXLineMask);
                case MaskType.AYLine:
                    return Collision.TestOverlap(this, mask as AYLineMask);
                case MaskType.Line:
                    return Collision.TestOverlap(this, mask as LineMask);
                case MaskType.AABB:
                    return Collision.TestOverlap(this, mask as AABBMask);
                case MaskType.Triangle:
                    return Collision.TestOverlap(this, mask as TriangleMask);
                case MaskType.Composite:
                    return Collision.TestOverlap(this, mask as CompositeMask);
            }

            return false;
        }

        public override TestResult TestOverlapExt (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return CollisionTR.TestOverlap(mask as PointMask, this);
                case MaskType.Line:
                    return CollisionTR.TestOverlap(mask as LineMask, this);
                case MaskType.Circle:
                    return CollisionTR.TestOverlap(this, mask as CircleMask);
                case MaskType.AABB:
                    return CollisionTR.TestOverlap(this, mask as AABBMask);
                case MaskType.Triangle:
                    return CollisionTR.TestOverlap(this, mask as TriangleMask);
            }

            return TestResult.None;
        }

        public TestResult TestOverlapExt (PointMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (LineMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (CircleMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        public TestResult TestOverlapExt (AABBMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        public TestResult TestOverlapExt (TriangleMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }
    }
}
