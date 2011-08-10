using System;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    public static class Collision
    {
        // Point -- [____] Collision Tests

        public static bool TestOverlap (PointMask ptMask1, PointMask ptMask2)
        {
            return false;
        }

        public static bool TestOverlap (PointMask ptMask, CircleMask cMask)
        {
            Vector2 p0 = ptMask._pos + ptMask._point;
            Vector2 p1 = cMask._pos + cMask._p;

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float d2 = dx * dx + dy * dy;
            float r2 = cMask._radius * cMask._radius;

            return (d2 < r2);
        }

        public static bool TestOverlap (PointMask ptMask, AXLineMask xlMask)
        {
            return false;
        }

        public static bool TestOverlap (PointMask ptMask, AYLineMask ylMask)
        {
            return false;
        }

        public static bool TestOverlap (PointMask ptMask, LineMask lnMask)
        {
            return false;
        }

        public static bool TestOverlap (PointMask ptMask, AABBMask rMask)
        {
            Vector2 p0 = ptMask._pos + ptMask._point;
            Vector2 p1 = rMask._pos + rMask._point;

            return (p0.X > p1.X && p0.X < p1.X + rMask._w && p0.Y > p1.Y && p0.Y < p1.Y + rMask._h);
        }

        public static bool TestOverlap (PointMask ptMask, TriangleMask triMask)
        {
            Vector2 bary = triMask.Barycentric(ptMask._pos + ptMask._point);

            return (bary.X > 0f && bary.Y > 0f && (bary.X + bary.Y) < 1f);
        }

        // Circle -- [____] Collision Tests

        public static bool TestOverlap (CircleMask cMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, cMask);
        }

        public static bool TestOverlap (CircleMask cMask1, CircleMask cMask2)
        {
            Vector2 p0 = cMask1._pos + cMask1._p;
            Vector2 p1 = cMask2._pos + cMask2._p;

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float d2 = dx * dx + dy * dy;
            float r = cMask1._radius + cMask2._radius;
            float r2 = r * r;

            return (d2 < r2);
        }

        public static bool TestOverlap (CircleMask cMask, AXLineMask xlMask)
        {
            Vector2 c1 = cMask._pos + cMask._p;
            Vector2 c2 = xlMask.ClosestPoint(c1);

            float d2 = Vector2.DistanceSquared(c1, c2);
            float r2 = cMask._radius * cMask._radius;

            return (r2 < d2);
        }

        public static bool TestOverlap (CircleMask cMask, AYLineMask ylMask)
        {
            Vector2 c1 = cMask._pos + cMask._p;
            Vector2 c2 = ylMask.ClosestPoint(c1);

            float d2 = Vector2.DistanceSquared(c1, c2);
            float r2 = cMask._radius * cMask._radius;

            return (r2 < d2);
        }

        public static bool TestOverlap (CircleMask cMask, LineMask lnMask)
        {
            Vector2 c = cMask._pos + cMask._p;
            Vector2 p = lnMask.ClosestPoint(c);

            p = p - c;
            float p2 = p.LengthSquared();
            float r2 = cMask._radius * cMask._radius;

            return (p2 < r2);
        }

        public static bool TestOverlap (CircleMask cMask, AABBMask rMask)
        {
            Vector2 p0 = cMask._pos + cMask._p;
            Vector2 p1 = rMask.ClosestPoint(p0);

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float d2 = dx * dx + dy * dy;
            float r2 = cMask._radius * cMask._radius;

            return (d2 < r2);
        }

        public static bool TestOverlap (CircleMask cMask, TriangleMask triMask)
        {
            Vector2 p0 = cMask._pos + cMask._p;
            Vector2 p1 = triMask.ClosestPoint(p0);

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float d2 = dx * dx + dy * dy;
            float r2 = cMask._radius * cMask._radius;

            return (d2 < r2);
        }

        // AXLine -- [____] Collision Tests

        public static bool TestOverlap (AXLineMask xlMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, xlMask);
        }

        public static bool TestOverlap (AXLineMask xlMask, CircleMask cMask)
        {
            return TestOverlap(cMask, xlMask);
        }

        public static bool TestOverlap (AXLineMask xlMask1, AXLineMask xlMask2)
        {
            return false;
        }

        public static bool TestOverlap (AXLineMask xlMask, AYLineMask ylMask)
        {
            Vector2 c = xlMask._pos + xlMask._p;
            Vector2 d = c + new Vector2(xlMask._w, 0);

            return ylMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AXLineMask xlMask, LineMask lnMask)
        {
            Vector2 c = lnMask._pos + lnMask._p0;
            Vector2 d = c + new Vector2(lnMask._w, lnMask._h);

            return xlMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AXLineMask xlMask, AABBMask rMask)
        {
            float py = xlMask._pos.Y + xlMask._p.Y;
            float px1 = xlMask._pos.X + xlMask._p.X;
            float px2 = px1 + xlMask._w;

            Vector2 r1 = rMask._pos + rMask._point;
            Vector2 r2 = r1 + new Vector2(rMask._w, rMask._h);

            return !(py > r2.Y || py < r1.Y || px1 > r2.X || px2 < r1.X);
        }

        public static bool TestOverlap (AXLineMask xlMask, TriangleMask triMask)
        {
            Vector2 a = triMask._pos + triMask._p0;
            Vector2 b = triMask._pos + triMask._p0;
            Vector2 c = triMask._pos + triMask._p0;

            if (xlMask.IntersectsLine(a, b) || xlMask.IntersectsLine(b, c) || xlMask.IntersectsLine(a, c)) {
                return true;
            }

            Vector2 q = triMask.Barycentric(xlMask._pos + xlMask._p);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        // AYLine -- [____] Collision Tests

        public static bool TestOverlap (AYLineMask ylMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, ylMask);
        }

        public static bool TestOverlap (AYLineMask ylMask, CircleMask cMask)
        {
            return TestOverlap(cMask, ylMask);
        }

        public static bool TestOverlap (AYLineMask ylMask, AXLineMask xlMask)
        {
            return TestOverlap(xlMask, ylMask);
        }

        public static bool TestOverlap (AYLineMask ylMask1, AYLineMask ylMask2)
        {
            return false;
        }

        public static bool TestOverlap (AYLineMask ylMask, LineMask lnMask)
        {
            Vector2 c = lnMask._pos + lnMask._p0;
            Vector2 d = c + new Vector2(lnMask._w, lnMask._h);

            return ylMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AYLineMask ylMask, AABBMask rMask)
        {
            float px = ylMask._pos.X + ylMask._p.X;
            float py1 = ylMask._pos.Y + ylMask._p.Y;
            float py2 = py1 + ylMask._h;

            Vector2 r1 = rMask._pos + rMask._point;
            Vector2 r2 = r1 + new Vector2(rMask._w, rMask._h);

            return !(px > r2.X || px < r1.X || py1 > r2.Y || py2 < r1.Y);
        }

        public static bool TestOverlap (AYLineMask ylMask, TriangleMask triMask)
        {
            Vector2 a = triMask._pos + triMask._p0;
            Vector2 b = triMask._pos + triMask._p0;
            Vector2 c = triMask._pos + triMask._p0;

            if (ylMask.IntersectsLine(a, b) || ylMask.IntersectsLine(b, c) || ylMask.IntersectsLine(a, c)) {
                return true;
            }

            Vector2 q = triMask.Barycentric(ylMask._pos + ylMask._p);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }
        
        // Line -- [____] Collision Tests

        public static bool TestOverlap (LineMask lnMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask, CircleMask cMask)
        {
            return TestOverlap(cMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask, AXLineMask xlMask)
        {
            return TestOverlap(xlMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask, AYLineMask ylMask)
        {
            return TestOverlap(ylMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask1, LineMask lnMask2)
        {
            Vector2 a = lnMask1._pos + lnMask1._p0;
            Vector2 b = a + new Vector2(lnMask1._w, lnMask1._h);

            return lnMask2.IntersectsLine(a, b);
        }

        public static bool TestOverlap (LineMask lnMask, AABBMask rMask)
        {
            Vector2 p0 = lnMask._pos + lnMask._p0;
            Vector2 p1 = new Vector2(p0.X + lnMask._w, p0.Y + lnMask._h);

            return rMask.IntersectsLine(p0, p1);
        }

        public static bool TestOverlap (LineMask lnMask, TriangleMask triMask)
        {
            Vector2 a = triMask._pos + triMask._p0;
            Vector2 b = triMask._pos + triMask._p1;
            Vector2 c = triMask._pos + triMask._p2;

            TestResult t = lnMask.LineIntersect(a, b);

            if (lnMask.IntersectsLine(a, b) || lnMask.IntersectsLine(b, c) || lnMask.IntersectsLine(a, c)) {
                return true;
            }

            // Check for containment
            Vector2 r = triMask.Barycentric(lnMask._pos + lnMask._p0);

            return (r.X >= 0 && r.Y >= 0 && (r.X + r.Y) <= 1f);
        }

        // AABB -- [____] Collision Tests

        public static bool TestOverlap (AABBMask rMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, CircleMask cMask)
        {
            return TestOverlap(cMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, AXLineMask xlMask)
        {
            return TestOverlap(xlMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, AYLineMask ylMask)
        {
            return TestOverlap(ylMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, LineMask lnMask)
        {
            return TestOverlap(lnMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask1, AABBMask rMask2)
        {
            Vector2 p0 = rMask1._pos + rMask1._point;
            Vector2 p1 = rMask2._pos + rMask2._point;

            float x1 = p0.X + rMask1._w;
            float x2 = p1.X + rMask2._w;
            float y1 = p0.Y + rMask1._h;
            float y2 = p1.Y + rMask2._h;

            return !(x1 < p1.X || p0.X > x2 || y1 < p1.Y || p0.Y > y2);
        }

        public static bool TestOverlap (AABBMask rMask, TriangleMask triMask)
        {
            Vector2 a = triMask._pos + triMask._p0;
            Vector2 b = triMask._pos + triMask._p1;
            Vector2 c = triMask._pos + triMask._p2;

            if (rMask.IntersectsLine(a, b) || rMask.IntersectsLine(b, c) || rMask.IntersectsLine(a, c)) {
                return true;
            }

            Vector2 r = rMask._pos + new Vector2(rMask._point.X + rMask._w * 0.5f, rMask._point.Y + rMask._h * 0.5f);
            Vector2 q = triMask.Barycentric(r);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1f);
        }

        // Triangle -- [____] Collision Tests

        public static bool TestOverlap (TriangleMask triMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, CircleMask cMask)
        {
            return TestOverlap(cMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, AXLineMask xlMAsk)
        {
            return TestOverlap(xlMAsk, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, AYLineMask ylMask)
        {
            return TestOverlap(ylMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, LineMask lnMask)
        {
            return TestOverlap(lnMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, AABBMask rMask)
        {
            return TestOverlap(rMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask1, TriangleMask triMask2)
        {
            Vector2 a = triMask2._pos + triMask2._p0;
            Vector2 b = triMask2._pos + triMask2._p1;
            Vector2 c = triMask2._pos + triMask2._p2;

            Vector2 d = triMask1._pos + triMask1._p0;
            Vector2 e = triMask1._pos + triMask1._p1;
            Vector2 f = triMask1._pos + triMask1._p2;

            if (LineMask.IntersectsLine(a, b, d, e) || LineMask.IntersectsLine(a, b, e, f) ||
                LineMask.IntersectsLine(a, b, d, f) || LineMask.IntersectsLine(b, c, d, e) ||
                LineMask.IntersectsLine(b, c, e, f) || LineMask.IntersectsLine(b, c, d, f) ||
                LineMask.IntersectsLine(a, c, d, e) || LineMask.IntersectsLine(a, c, e, f)) 
            {
                return true;
            }

            // Check for containment
            Vector2 q = triMask2.Barycentric(d);
            if (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1f) {
                return true;
            }

            q = triMask1.Barycentric(a);
            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1f);
        }

        // Composite -- [____] Collision Tests

        public static bool TestOverlap (Mask mask, CompositeMask comMask)
        {
            if (!mask.Bounds.Overlaps(comMask.Bounds)) {
                return false;
            }

            foreach (Mask m in comMask._components) {
                if (mask.TestOverlap(m)) {
                    return true;
                }
            }

            return false;
        }

        public static bool TestOverlap (CompositeMask comMask, Mask mask)
        {
            return TestOverlap(mask, comMask);
        }

        public static bool TestOverlap (CompositeMask comMask1, CompositeMask comMask2)
        {
            if (!comMask1.Bounds.Overlaps(comMask2.Bounds)) {
                return false;
            }

            foreach (Mask m1 in comMask1._components) {
                if (!m1.Bounds.Overlaps(comMask2.Bounds)) {
                    return false;
                }

                foreach (Mask m2 in comMask2._components) {
                    if (m1.TestOverlap(m2)) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
