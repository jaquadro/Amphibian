using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Collision
{
    public sealed class CompositeMask : Mask
    {
        internal List<Mask> _components;
        internal BoundingRectangle _bounds;

        public CompositeMask (Mask mask)
        {
            _type = MaskType.Point;
            _pos = mask.Position;
            _bounds = mask.Bounds;

            if (mask is CompositeMask) {
                _components = new List<Mask>((mask as CompositeMask)._components);
            }
            else {
                _components = new List<Mask>();
                _components.Add(mask);
            }
        }

        public void Add (Mask mask)
        {
            BoundingRectangle rect = mask.Bounds;

            float minx = Math.Min(_bounds.Left, rect.Left);
            float maxx = Math.Max(_bounds.Right, rect.Right);
            float miny = Math.Min(_bounds.Top, rect.Top);
            float maxy = Math.Min(_bounds.Bottom, rect.Bottom);

            _bounds = new BoundingRectangle(minx, miny, maxx - minx, maxy - miny);

            _components.Add(mask);
        }

        public void Merge (CompositeMask mask)
        {
            BoundingRectangle rect = mask.Bounds;

            float minx = Math.Min(_bounds.Left, rect.Left);
            float maxx = Math.Max(_bounds.Right, rect.Right);
            float miny = Math.Min(_bounds.Top, rect.Top);
            float maxy = Math.Min(_bounds.Bottom, rect.Bottom);

            _bounds = new BoundingRectangle(minx, miny, maxx - minx, maxy - miny);

            foreach (Mask m in mask._components) {
                _components.Add(mask);
            }
        }

        public override object Clone ()
        {
            CompositeMask mask = new CompositeMask(this);
            mask._pos = _pos;

            return mask;
        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            foreach (Mask m in _components) {
                m.Draw(spriteBatch);
            }
        }

        public override BoundingRectangle Bounds
        {
            get { return _bounds; }
        }

        public override bool TestOverlap (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return Collision.TestOverlap(mask as PointMask, this);
                case MaskType.Circle:
                    return Collision.TestOverlap(mask as CircleMask, this);
                case MaskType.AXLine:
                    return Collision.TestOverlap(mask as AXLineMask, this);
                case MaskType.AYLine:
                    return Collision.TestOverlap(mask as AYLineMask, this);
                case MaskType.Line:
                    return Collision.TestOverlap(mask as LineMask, this);
                case MaskType.AABB:
                    return Collision.TestOverlap(mask as AABBMask, this);
                case MaskType.Triangle:
                    return Collision.TestOverlap(mask as TriangleMask, this);
                case MaskType.Composite:
                    return Collision.TestOverlap(this, mask as CompositeMask);
            }

            return false;
        }

        public override TestResult TestOverlapExt (Mask mask)
        {
            /*switch (mask._type) {
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
            }*/

            return TestResult.None;
        }

        /*public TestResult TestOverlapExt (PointMask mask)
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
        }*/
    }
}
