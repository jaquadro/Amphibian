using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Amphibian.Geometry;

namespace Amphibian.Collision.Tests
{
    [TestFixture]
    class MaskInternalTests
    {
        // AXLine Line Segment Intersection Tests

        [Test]
        public void TestAXLineIntersectLineNone ()
        {
            AXLineMask axline = new AXLineMask(new PointFP(0, 0), 10);

            // Against Pos Line
            PointFP c = new PointFP(40, 80);
            PointFP d = new PointFP(50, 90);

            axline.Position = new PointFP(29, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(34, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(39, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(41, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(46, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(51, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(45, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new PointFP(50, 80);
            d = new PointFP(40, 90);

            axline.Position = new PointFP(39, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(34, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(29, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(51, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(46, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(41, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(55, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against H Line
            c = new PointFP(40, 80);
            d = new PointFP(50, 80);

            axline.Position = new PointFP(29, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(51, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(40, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(40, 81);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against V Line
            c = new PointFP(40, 80);
            d = new PointFP(40, 90);

            axline.Position = new PointFP(29, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(41, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLineEdge ()
        {
            AXLineMask axline = new AXLineMask(new PointFP(0, 0), 10);

            // Against Pos Line
            PointFP c = new PointFP(40, 80);
            PointFP d = new PointFP(50, 90);

            axline.Position = new PointFP(30, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(40, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(45, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(50, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(45, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new PointFP(50, 80);
            d = new PointFP(40, 90);

            axline.Position = new PointFP(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(30, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(50, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(45, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(40, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(55, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against H Line
            c = new PointFP(40, 80);
            d = new PointFP(50, 80);

            axline.Position = new PointFP(30, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(50, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against V Line
            c = new PointFP(40, 80);
            d = new PointFP(40, 90);

            axline.Position = new PointFP(30, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(40, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(35, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLineColinear ()
        {
            AXLineMask axline = new AXLineMask(new PointFP(0, 0), 10);

            // Against H Line
            PointFP c = new PointFP(40, 80);
            PointFP d = new PointFP(50, 80);

            axline.Position = new PointFP(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new PointFP(45, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLine ()
        {
            AXLineMask axline = new AXLineMask(new PointFP(0, 0), 10);

            // Against V Line
            PointFP c = new PointFP(40, 80);
            PointFP d = new PointFP(40, 90);

            axline.Position = new PointFP(35, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));

            // Against Pos Line
            c = new PointFP(40, 80);
            d = new PointFP(50, 90);

            axline.Position = new PointFP(40, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new PointFP(50, 80);
            d = new PointFP(40, 90);

            axline.Position = new PointFP(40, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));
        }

        // AYLine Line Segment Intersection Tests

        [Test]
        public void TestAYLineIntersectLineNone ()
        {
            AYLineMask ayline = new AYLineMask(new PointFP(0, 0), 10);

            // Against Pos Line
            PointFP c = new PointFP(40, 80);
            PointFP d = new PointFP(50, 90);

            ayline.Position = new PointFP(40, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 74);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 79);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 86);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(39, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(51, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new PointFP(50, 80);
            d = new PointFP(40, 90);

            ayline.Position = new PointFP(40, 79);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 74);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 86);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(39, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(51, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against H Line
            c = new PointFP(40, 80);
            d = new PointFP(50, 80);

            ayline.Position = new PointFP(39, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(51, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against V Line
            c = new PointFP(40, 80);
            d = new PointFP(40, 90);

            ayline.Position = new PointFP(39, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(41, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLineEdge ()
        {
            AYLineMask ayline = new AYLineMask(new PointFP(0, 0), 10);

            // Against Pos Line
            PointFP c = new PointFP(40, 80);
            PointFP d = new PointFP(50, 90);

            ayline.Position = new PointFP(40, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new PointFP(50, 80);
            d = new PointFP(40, 90);

            ayline.Position = new PointFP(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against H Line
            c = new PointFP(40, 80);
            d = new PointFP(50, 80);

            ayline.Position = new PointFP(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(50, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(45, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against V Line
            c = new PointFP(40, 80);
            d = new PointFP(40, 90);

            ayline.Position = new PointFP(40, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLineColinear ()
        {
            AYLineMask ayline = new AYLineMask(new PointFP(0, 0), 10);

            // Against H Line
            PointFP c = new PointFP(40, 80);
            PointFP d = new PointFP(50, 80);

            ayline.Position = new PointFP(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new PointFP(40, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLine ()
        {
            AYLineMask ayline = new AYLineMask(new PointFP(0, 0), 10);

            // Against H Line
            PointFP c = new PointFP(40, 80);
            PointFP d = new PointFP(50, 80);

            ayline.Position = new PointFP(45, 75);
            Assert.IsTrue(ayline.IntersectsLine(c, d));

            // Against Pos Line
            c = new PointFP(40, 80);
            d = new PointFP(50, 90);

            ayline.Position = new PointFP(45, 80);
            Assert.IsTrue(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new PointFP(50, 80);
            d = new PointFP(40, 90);

            ayline.Position = new PointFP(45, 80);
            Assert.IsTrue(ayline.IntersectsLine(c, d));
        }
    }
}
