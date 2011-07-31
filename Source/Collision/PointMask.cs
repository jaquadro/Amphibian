using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    public sealed class PointMask : Mask
    {
        internal Vector2 _point;

        public PointMask (Vector2 p)
        {
            _type = MaskType.Point;
            _point = p;
        }

        public override bool TestOverlap (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return Collision.TestOverlap(this, mask as PointMask);
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
            }

            return false;
        }

        public override TestResult TestOverlapExt (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return CollisionTR.TestOverlap(this, mask as PointMask);
                case MaskType.Line:
                    return CollisionTR.TestOverlap(this, mask as LineMask);
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
            return CollisionTR.TestOverlap(this, mask);
        }

        public TestResult TestOverlapExt (LineMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
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
