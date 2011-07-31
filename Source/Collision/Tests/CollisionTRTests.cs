using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision.Tests
{
    [TestFixture]
    class CollisionTRTests
    {
        // Point -- Point Tests

        [Test]
        public void TestPointPointNone ()
        {
            PointMask m1 = new PointMask(new Vector2(30, 30));
            PointMask m2 = new PointMask(new Vector2(30, 40));

            m1.Position = new Vector2(20, 30);
            m2.Position = new Vector2(20, 30);

            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestPointPointEqual ()
        {
            PointMask m1 = new PointMask(new Vector2(30, 30));
            PointMask m2 = new PointMask(new Vector2(30, 40));

            m1.Position = new Vector2(20, 30);
            m2.Position = new Vector2(20, 20);

            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        // Point -- Line Tests

        [Test]
        public void TestPointLineNone ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            LineMask m2 = new LineMask(new Vector2(0, 0), 10, 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(41, 80);

            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestPointLineEdge ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            LineMask m2 = new LineMask(new Vector2(0, 0), 10, 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 100);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        // Point -- Circle Tests

        [Test]
        public void TestPointCircleNone ()
        {
            PointMask m1 = new PointMask(new Vector2(20, 30));
            CircleMask m2 = new CircleMask(new Vector2(40, 40), 10);

            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestPointCircleEdge ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            CircleMask m2 = new CircleMask(new Vector2(40, 80), 10);

            // Cardinal Points
            m1.Position = new Vector2(30, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestPointCircleInside ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            CircleMask m2 = new CircleMask(new Vector2(40, 80), 10);

            m1.Position = new Vector2(31, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(40, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Point -- Rectangle Tests

        [Test]
        public void TestPointRectNone ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 10, 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(39, 85);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(51, 85);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(45, 79);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(45, 101);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestPointRectEdge ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 10, 20);

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 100);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Corners
            m1.Position = new Vector2(40, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 100);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 100);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestPointRectInside ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 10, 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(45, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Point -- Triangle Tests

        [Test]
        public void TestPointTriangleNone ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(39, 85);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(45, 91);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(46, 84);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestPointTriangleEdge ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Corners
            m1.Position = new Vector2(40, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestPointTriangleInside ()
        {
            PointMask m1 = new PointMask(new Vector2(0, 0));
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(41, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(45, 89);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(44, 86);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Line -- Line Tests

        [Test]
        public void TestLineLineNone ()
        {
            // Colinear Horz
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            LineMask m2 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(51, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(29, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            // Colinear Vert
            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));
            m2 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 69);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(40, 91);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            // Colinear Angle
            m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 10));
            m2 = new LineMask(new Vector2(0, 0), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(51, 92);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(29, 69);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            // Parallel
            m1.Position = new Vector2(41, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(39, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            // Differing
            m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            m1.Position = new Vector2(34, 85);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(30, 90);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(30, 100);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestLineLineEdge ()
        {
            // Colinear Horz
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            LineMask m2 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(50, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(35, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(30, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Colinear Vert
            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));
            m2 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Colinear Angle
            m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 10));
            m2 = new LineMask(new Vector2(0, 0), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(30, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(35, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Tangent
            m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            m1.Position = new Vector2(35, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Endpoints
            m1.Position = new Vector2(50, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(30, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestLineLinePartial ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            LineMask m2 = new LineMask(new Vector2(0, 0), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m2 = new LineMask(new Vector2(0, 0), new Vector2(10, -10));
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Line -- Circle Tests

        [Test]
        public void TestLineCircleNone ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 10);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(35, 69);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));
            m1.Position = new Vector2(29, 75);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 10));
            m1.Position = new Vector2(55, 75);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestLineCircleEdge ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 10);

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(35, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(35, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));
            m1.Position = new Vector2(30, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Endpoints
            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));
            m1.Position = new Vector2(40, 60);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            m1.Position = new Vector2(20, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestLineCirclePartial ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(15, 15));
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 10);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(20, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 30));
            m1.Position = new Vector2(35, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestLineCircleInside ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 10));
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 10);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(35, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestLineCircleInsideEdge ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 0));
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 10);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(30, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));
            m1.Position = new Vector2(40, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Line -- Rectangle Tests

        [Test]
        public void TestLineRectNone ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(20, 10));
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 20, 10);

            // Sides
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(19, 78);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(61, 82);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(18, 69);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(22, 91);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            // Corners
            m1.Position = new Vector2(30, 88);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(55, 72);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            // Vert Lines
            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));

            m1.Position = new Vector2(39, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(61, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(50, 69);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(50, 91);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            // Horiz Lines
            m1 = new LineMask(new Vector2(0, 0), new Vector2(20, 0));

            m1.Position = new Vector2(40, 79);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(40, 91);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(19, 90);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(61, 90);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestLineRectEdge ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(20, 10));
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 20, 10);

            // Slanted -- Sides
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(20, 78);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(60, 82);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(38, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(42, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Corners
            m1.Position = new Vector2(38, 89);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(58, 79);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Vert Lines
            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 10));

            m1.Position = new Vector2(40, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(60, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Horiz Lines
            m1 = new LineMask(new Vector2(0, 0), new Vector2(20, 0));

            m1.Position = new Vector2(35, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(20, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(60, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestLineRectPartial ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(20, 10));
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 20, 10);

            // Sides
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(24, 78);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(55, 82);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(38, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(42, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            // Through
            m1 = new LineMask(new Vector2(0, 0), new Vector2(30, 5));
            m1.Position = new Vector2(35, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 20));
            m1.Position = new Vector2(45, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(30, 0));
            m1.Position = new Vector2(35, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), new Vector2(0, 20));
            m1.Position = new Vector2(45, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestLineRectInside ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 5));
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 20, 10);

            // Sides
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(42, 82);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestLineRectInsideEdge ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), new Vector2(10, 5));
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 20, 10);

            // Sides
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 82);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(50, 82);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(42, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(42, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Line -- Triangle Tests

        [Test]
        public void TestLineTriangleNone ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), 10, 0);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(29, 85);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(46, 85);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(35, 79);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(40, 91);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestLineTriangleEdge ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), 10, 10);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Angled
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(30, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(30, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(35, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(50, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Horiz
            m1 = new LineMask(new Vector2(0, 0), 10, 0);
            m1.Position = new Vector2(30, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(35, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Vert
            m1 = new LineMask(new Vector2(0, 0), 0, 10);
            m1.Position = new Vector2(40, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestLineTrianglePartial ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), 10, 10);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Angled
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(35, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(42, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new LineMask(new Vector2(0, 0), 10, -10);
            m1.Position = new Vector2(42, 88);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(35, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(36, 95);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            // Horiz
            m1 = new LineMask(new Vector2(0, 0), 10, 0);
            m1.Position = new Vector2(32, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(35, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(39, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            // Vert
            m1 = new LineMask(new Vector2(0, 0), 0, 10);
            m1.Position = new Vector2(45, 78);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(45, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(49, 82);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestLineTriangleInside ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), 5, 5);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Angled
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(42, 83);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestLineTriangleInsideEdge ()
        {
            LineMask m1 = new LineMask(new Vector2(0, 0), 5, 5);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Angled
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 82);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(43, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            // Horiz
            m1 = new LineMask(new Vector2(0, 0), 5, 0);
            m1.Position = new Vector2(40, 88);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(43, 88);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            // Vert
            m1 = new LineMask(new Vector2(0, 0), 0, 5);
            m1.Position = new Vector2(42, 82);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(42, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Circle -- Circle Tests

        [Test]
        public void TestCircleCircleNone ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(9, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(15, 55);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestCircleCircleEdge ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(10, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(70, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 50);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(40, 110);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestCircleCirclePartial ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(20, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestCircleCircleInside ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestCircleCircleInsideEdge ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(30, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(50, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(40, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(40, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestCircleCircleEqual ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            CircleMask m2 = new CircleMask(new Vector2(0, 0), 10);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Circle -- Rectangle Tests

        [Test]
        public void TestCircleRectNone ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 10, 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(29, 90);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(32, 72);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestCircleRectEdge ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 10, 20);

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(30, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(60, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 110);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestCircleRectPartial ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 10, 20);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(35, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(38, 78);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestCircleRectInside ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 30, 40);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(55, 100);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new CircleMask(new Vector2(0, 0), 20);
            m2 = new AABBMask(new Vector2(0, 0), 10, 12);

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(45, 86);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestCircleRectInsideEdge ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 30, 40);

            // Circle in Rect
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(50, 100);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(60, 100);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(55, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(55, 110);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Circle -- Triangle Tests

        [Test]
        public void TestCircleTriangleNone ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(29, 85);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(55, 75);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestCircleTriangleEdge ()
        {
            CircleMask m1 = new CircleMask(new Vector2(0, 0), 10);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(30, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 100);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Corners
            m1.Position = new Vector2(40, 70);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(60, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        // Rectangle -- Rectangle Tests

        [Test]
        public void TestRectRectNone ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 20);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 20, 10);

            // Not overlapping
            m1.Position = new Vector2(0, 0);
            m2.Position = new Vector2(30, 10);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestRectRectEdge ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 20);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 20, 10);

            // Edges
            m2.Position = new Vector2(10, 0);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(0, 20);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(-20, 0);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(0, -10);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Corners
            m2.Position = new Vector2(10, 20);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(10, -10);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(-20, 20);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(-20, -10);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestRectRectPartial ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 20);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 20, 10);

            m2.Position = new Vector2(9, 0);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m2.Position = new Vector2(0, 8);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestRectRectInside ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 20);
            AABBMask m2 = new AABBMask(new Vector2(-5, -5), 20, 30);

            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestRectRectInsideEdge ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 20);
            AABBMask m2 = new AABBMask(new Vector2(-5, -5), 15, 30);

            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestRectRectEqual ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 20);
            AABBMask m2 = new AABBMask(new Vector2(0, 0), 10, 20);

            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Rectangle -- Triangle Tests

        [Test]
        public void TestRectTriangleNone ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 10);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(29, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(40, 91);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(46, 74);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestRectTriangleEdge ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 20);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Edges
            m2.Position = new Vector2(10, 0);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(0, 20);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(-10, 0);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(0, -10);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            // Corners
            m2.Position = new Vector2(10, 20);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(10, -10);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(0, 20);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m2.Position = new Vector2(-10, -10);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestRectTrianglePartial ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 10, 10);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(32, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(38, 88);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(32, 78);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new AABBMask(new Vector2(0, 0), 5, 20);
            m1.Position = new Vector2(42, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestRectTriangleInside ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 20, 20);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(35, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1 = new AABBMask(new Vector2(0, 0), 5, 5);
            m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 20), new Vector2(20, 20));

            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(42, 93);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestRectTriangleInsideEdge ()
        {
            AABBMask m1 = new AABBMask(new Vector2(0, 0), 20, 20);
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Tri inside Rect
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(35, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(35, 80);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(30, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        // Triange -- Trangle Tests

        [Test]
        public void TestTriangleTriangleNone ()
        {
            TriangleMask m1 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(29, 80);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(45, 91);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);

            m1.Position = new Vector2(46, 74);
            Assert.IsTrue(m2.TestOverlapExt(m1) == TestResult.None);
        }

        [Test]
        public void TestTriangleTriangleEdge ()
        {
            TriangleMask m1 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(30, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);

            m1.Position = new Vector2(45, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) != TestResult.None);
        }

        [Test]
        public void TestTriangleTrianglePartial ()
        {
            TriangleMask m1 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(32, 75);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(45, 88);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(32, 78);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestTriangleTriangleInside ()
        {
            TriangleMask m1 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 20), new Vector2(20, 20));

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(42, 88);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }

        [Test]
        public void TestTriangleTriangleInsideEdge ()
        {
            TriangleMask m1 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10));
            TriangleMask m2 = new TriangleMask(new Vector2(0, 0), new Vector2(0, 20), new Vector2(20, 20));

            // Edges
            m2.Position = new Vector2(40, 80);
            m1.Position = new Vector2(40, 88);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(45, 90);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);

            m1.Position = new Vector2(45, 85);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Overlapping) != TestResult.None);
            Assert.IsTrue((m2.TestOverlapExt(m1) & TestResult.Edge) == TestResult.None);
        }
    }
}
