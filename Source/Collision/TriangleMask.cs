using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amphibian.Drawing;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Collision
{
    public class TriangleMask : Mask
    {
        internal Vector2 _p0;
        internal Vector2 _p1;
        internal Vector2 _p2;
        internal float _det;
        internal BoundingRectangle _bound;

        public TriangleMask (Vector2 p0, Vector2 p1, Vector2 p2)
        {
            _type = MaskType.Triangle;
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;

            Vector2 a = _pos + _p0;
            Vector2 b = _pos + _p1;
            Vector2 c = _pos + _p2;

            _det = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
            _det = 1f / _det;
            
            float minx = Math.Min(_p0.X, Math.Min(_p1.X, _p2.X));
            float maxx = Math.Max(_p0.X, Math.Max(_p1.X, _p2.X));
            float miny = Math.Min(_p0.Y, Math.Min(_p1.Y, _p2.Y));
            float maxy = Math.Max(_p0.Y, Math.Max(_p1.Y, _p2.Y));

            _bound = new BoundingRectangle(minx, miny, maxx - minx, maxy - miny);
        }

        public override object Clone ()
        {
            TriangleMask mask = new TriangleMask(_p0, _p1, _p2);
            mask._pos = _pos;

            return mask;
        }

        public override BoundingRectangle Bounds
        {
            get { return _bound; }
        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            Primitives2D.DrawLine(spriteBatch, _pos + _p0, _pos + _p1, Color.White);
            Primitives2D.DrawLine(spriteBatch, _pos + _p1, _pos + _p2, Color.White);
            Primitives2D.DrawLine(spriteBatch, _pos + _p0, _pos + _p2, Color.White);
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
                    return CollisionTR.TestOverlap(mask as CircleMask, this);
                case MaskType.AABB:
                    return CollisionTR.TestOverlap(mask as AABBMask, this);
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
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (AABBMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (TriangleMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        internal Vector2 Barycentric (Vector2 p)
        {
            //Vector2 a = _pos + _p0;
            //Vector2 b = _pos + _p1;
            //Vector2 c = _pos + _p2;

            Vector2 a = _p0;
            Vector2 b = _p1;
            Vector2 c = _p2;
            p = p - _pos;

            float v = (b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y);
            float u = (c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y);

            return new Vector2(v * _det, u * _det);
        }

        internal Vector2 ClosestPoint (Vector2 p)
        {
            Vector2 a = _pos + _p0;
            Vector2 b = _pos + _p1;
            Vector2 c = _pos + _p2;

            // Check if P in region outside A
            Vector2 ab = b - a;
            Vector2 ac = c - a;
            Vector2 ap = p - a;
            float d1 = Vector2.Dot(ab, ap);
            float d2 = Vector2.Dot(ac, ap);
            if (d1 <= 0f && d2 <= 0f) {
                return a;
            }

            // Check if P in region outside B
            Vector2 bp = p - b;
            float d3 = Vector2.Dot(ab, bp);
            float d4 = Vector2.Dot(ac, bp);
            if (d3 >= 0f && d4 <= d3) {
                return b;
            }

            // Check if P in edge region of AB
            float vc = d1 * d4 - d3 * d2;
            if (vc <= 0f && d1 >= 0f && d3 <= 0f) {
                float v = d1 / (d1 - d3);
                return a + v * ab;
            }

            // Check if P in region outside C
            Vector2 cp = p - c;
            float d5 = Vector2.Dot(ab, cp);
            float d6 = Vector2.Dot(ac, cp);
            if (d6 >= 0f && d5 <= d6) {
                return c;
            }

            // Check if P in edge region of AC
            float vb = d5 * d2 - d1 * d6;
            if (vb <= 0f && d2 >= 0f && d6 <= 0f) {
                float w = d2 / (d2 - d6);
                return a + w * ac;
            }

            // Check if P in edge region of BC
            float va = d3 * d6 - d5 * d4;
            if (va <= 0f && (d4 - d3) >= 0f && (d5 - d6) >= 0f) {
                float w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return b + w * (c - b);
            }

            // P inside region
            return p;
        }
    }
}
