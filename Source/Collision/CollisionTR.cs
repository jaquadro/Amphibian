using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    [Flags]
    public enum TestResult
    {
        None = 0,
        Overlapping = 1,
        Edge = 2,
    }

    static class CollisionTR
    {
        public static TestResult TestOverlap (PointMask ptMask1, PointMask ptMask2)
        {
            Vector2 p0 = ptMask1._pos + ptMask1._point;
            Vector2 p1 = ptMask2._pos + ptMask2._point;

            if (p0 == p1) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (PointMask ptMask, LineMask lnMask)
        {
            Vector2 p = ptMask._pos + ptMask._point;
            Vector2 l = lnMask.ClosestPoint(p);

            if (l == p) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (LineMask lnMask1, LineMask lnMask2)
        {
            Vector2 a = lnMask1._pos + lnMask1._p0;
            Vector2 b = a + new Vector2(lnMask1._w, lnMask1._h);

            return lnMask2.LineIntersect(a, b);
        }

        public static TestResult TestOverlap (PointMask ptMask, CircleMask cMask)
        {
            Vector2 p0 = ptMask._pos + ptMask._point;
            Vector2 p1 = cMask._pos + cMask._p;

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float d2 = dx * dx + dy * dy;
            float r2 = cMask._radius * cMask._radius;

            if (d2 < r2) {
                return TestResult.Overlapping;
            }
            else if (d2 == r2) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (LineMask lnMask, CircleMask cMask)
        {
            Vector2 c = cMask._pos + cMask._p;
            Vector2 p = lnMask.ClosestPoint(c);

            p = p - c;
            float p2 = p.LengthSquared();
            float r2 = cMask._radius * cMask._radius;

            if (p2 > r2) {
                return TestResult.None;
            }
            else if (p2 == r2) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.Overlapping;
        }

        public static TestResult TestOverlap (CircleMask cMask1, CircleMask cMask2)
        {
            Vector2 p0 = cMask1._pos + cMask1._p;
            Vector2 p1 = cMask2._pos + cMask2._p;

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float d2 = dx * dx + dy * dy;
            float r = cMask1._radius + cMask2._radius;
            float r2 = r * r;

            if (d2 < r2) {
                return TestResult.Overlapping;
            }
            else if (d2 == r2) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (PointMask ptMask, AABBMask rMask)
        {
            Vector2 p0 = ptMask._pos + ptMask._point;
            Vector2 p1 = rMask._pos + rMask._point;

            if (p0.X > p1.X && p0.X < p1.X + rMask._w && p0.Y > p1.Y && p0.Y < p1.Y + rMask._h) {
                return TestResult.Overlapping;
            }
            else if (p0.X >= p1.X && p0.X <= p1.X + rMask._w && p0.Y >= p1.Y && p0.Y <= p1.Y + rMask._h) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (LineMask lnMask, AABBMask rMask)
        {
            Vector2 p0 = lnMask._pos + lnMask._p0;
            Vector2 p1 = new Vector2(p0.X + lnMask._w, p0.Y + lnMask._h);

            return rMask.LineIntersect(p0, p1);
        }

        public static TestResult TestOverlap (CircleMask cMask, AABBMask rMask)
        {
            Vector2 p0 = cMask._pos + cMask._p;
            Vector2 p1 = rMask.ClosestPoint(p0);

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float d2 = dx * dx + dy * dy;
            float r2 = cMask._radius * cMask._radius;

            if (d2 < r2) {
                return TestResult.Overlapping;
            }
            else if (d2 == r2) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (AABBMask rMask1, AABBMask rMask2)
        {
            Vector2 p0 = rMask1._pos + rMask1._point;
            Vector2 p1 = rMask2._pos + rMask2._point;

            float x1 = p0.X + rMask1._w;
            float x2 = p1.X + rMask2._w;
            float y1 = p0.Y + rMask1._h;
            float y2 = p1.Y + rMask2._h;

            if (x1 < p1.X || p0.X > x2 || y1 < p1.Y || p0.Y > y2) {
                return TestResult.None;
            }

            if (x1 == p1.X || p0.X == x2 || y1 == p1.Y || p0.Y == y2) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.Overlapping;
        }

        public static TestResult TestOverlap (PointMask ptMask, TriangleMask triMask)
        {
            Vector2 bary = triMask.Barycentric(ptMask._pos + ptMask._point);

            if (bary.X > 0f && bary.Y > 0f && (bary.X + bary.Y) < 1f) {
                return TestResult.Overlapping;
            }
            else if (bary.X >= 0f && bary.Y >= 0f && (bary.X + bary.Y) <= 1f) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (LineMask lnMask, TriangleMask triMask)
        {
            Vector2 a = triMask._pos + triMask._p0;
            Vector2 b = triMask._pos + triMask._p1;
            Vector2 c = triMask._pos + triMask._p2;

            TestResult t = lnMask.LineIntersect(a, b);
            if (t != TestResult.None) {
                if (t == TestResult.Overlapping) {
                    return t;
                }
                Vector2 q = triMask.Barycentric(lnMask._pos + lnMask._p0 + new Vector2(lnMask._w * 0.5f, lnMask._h * 0.5f));
                if (q.X > 0 && q.Y > 0 && (q.X + q.Y) < 1f) {
                    return TestResult.Overlapping;
                }
                else {
                    return TestResult.Overlapping | TestResult.Edge;
                }
            }

            t = lnMask.LineIntersect(a, c);
            if (t != TestResult.None) {
                if (t == TestResult.Overlapping) {
                    return t;
                }
                Vector2 q = triMask.Barycentric(lnMask._pos + lnMask._p0 + new Vector2(lnMask._w * 0.5f, lnMask._h * 0.5f));
                if (q.X > 0 && q.Y > 0 && (q.X + q.Y) < 1f) {
                    return TestResult.Overlapping;
                }
                else {
                    return TestResult.Overlapping | TestResult.Edge;
                }
            }

            t = lnMask.LineIntersect(b, c);
            if (t != TestResult.None) {
                if (t == TestResult.Overlapping) {
                    return t;
                }
                Vector2 q = triMask.Barycentric(lnMask._pos + lnMask._p0 + new Vector2(lnMask._w * 0.5f, lnMask._h * 0.5f));
                if (q.X > 0 && q.Y > 0 && (q.X + q.Y) < 1f) {
                    return TestResult.Overlapping;
                }
                else {
                    return TestResult.Overlapping | TestResult.Edge;
                }
            }

            // Check for containment
            Vector2 r = triMask.Barycentric(lnMask._pos + lnMask._p0);
            if (r.X >= 0 && r.Y >= 0 && (r.X + r.Y) <= 1f) {
                return TestResult.Overlapping;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (CircleMask cMask, TriangleMask triMask)
        {
            Vector2 p0 = cMask._pos + cMask._p;
            Vector2 p1 = triMask.ClosestPoint(p0);

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float d2 = dx * dx + dy * dy;
            float r2 = cMask._radius * cMask._radius;

            if (d2 < r2) {
                return TestResult.Overlapping;
            }
            else if (d2 == r2) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (AABBMask rMask, TriangleMask triMask)
        {
            Vector2 a = triMask._pos + triMask._p0;
            Vector2 b = triMask._pos + triMask._p1;
            Vector2 c = triMask._pos + triMask._p2;

            TestResult t = rMask.LineIntersect(a, b);
            if (t != TestResult.None) {
                return t;
            }

            t = rMask.LineIntersect(a, c);
            if (t != TestResult.None) {
                return t;
            }

            t = rMask.LineIntersect(b, c);
            if (t != TestResult.None) {
                return t;
            }

            Vector2 r = rMask._pos + new Vector2(rMask._point.X + rMask._w * 0.5f, rMask._point.Y + rMask._h * 0.5f);
            Vector2 q = triMask.Barycentric(r);

            if (q.X > 0 && q.Y > 0 && (q.X + q.Y) < 1f) {
                return TestResult.Overlapping;
            }

            return TestResult.None;
        }

        public static TestResult TestOverlap (TriangleMask triMask1, TriangleMask triMask2)
        {
            Vector2 a = triMask2._pos + triMask2._p0;
            Vector2 b = triMask2._pos + triMask2._p1;
            Vector2 c = triMask2._pos + triMask2._p2;

            Vector2 d = triMask1._pos + triMask1._p0;
            Vector2 e = triMask1._pos + triMask1._p1;
            Vector2 f = triMask1._pos + triMask1._p2;

            TestResult t = LineMask.LineIntersect(a, b, d, e);
            if (t != TestResult.None) return t;

            t = LineMask.LineIntersect(a, b, e, f);
            if (t != TestResult.None) return t;

            t = LineMask.LineIntersect(a, b, d, f);
            if (t != TestResult.None) return t;

            t = LineMask.LineIntersect(b, c, d, e);
            if (t != TestResult.None) return t;

            t = LineMask.LineIntersect(b, c, e, f);
            if (t != TestResult.None) return t;

            t = LineMask.LineIntersect(a, b, d, f);
            if (t != TestResult.None) return t;

            t = LineMask.LineIntersect(a, c, d, e);
            if (t != TestResult.None) return t;

            t = LineMask.LineIntersect(a, c, e, f);
            if (t != TestResult.None) return t;

            // Check for containment
            Vector2 q = triMask2.Barycentric(d);
            if (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1f) {
                return TestResult.Overlapping;
            }

            q = triMask1.Barycentric(a);
            if (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1f) {
                return TestResult.Overlapping;
            }

            return TestResult.None;
        }
    }
}
