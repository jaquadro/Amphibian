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
            AXLineMask axline = new AXLineMask(new SharedPointFP(0, 0), 10);

            // Against Pos Line
            SharedPointFP c = new SharedPointFP(40, 80);
            SharedPointFP d = new SharedPointFP(50, 90);

            axline.Position = new SharedPointFP(29, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(34, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(39, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(41, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(46, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(51, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(45, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new SharedPointFP(50, 80);
            d = new SharedPointFP(40, 90);

            axline.Position = new SharedPointFP(39, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(34, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(29, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(51, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(46, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(41, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(55, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against H Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(50, 80);

            axline.Position = new SharedPointFP(29, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(51, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(40, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(40, 81);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against V Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(40, 90);

            axline.Position = new SharedPointFP(29, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(41, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLineEdge ()
        {
            AXLineMask axline = new AXLineMask(new SharedPointFP(0, 0), 10);

            // Against Pos Line
            SharedPointFP c = new SharedPointFP(40, 80);
            SharedPointFP d = new SharedPointFP(50, 90);

            axline.Position = new SharedPointFP(30, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(40, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(45, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(50, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(45, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new SharedPointFP(50, 80);
            d = new SharedPointFP(40, 90);

            axline.Position = new SharedPointFP(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(30, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(50, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(45, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(40, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(55, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against H Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(50, 80);

            axline.Position = new SharedPointFP(30, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(50, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against V Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(40, 90);

            axline.Position = new SharedPointFP(30, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(40, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(35, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLineColinear ()
        {
            AXLineMask axline = new AXLineMask(new SharedPointFP(0, 0), 10);

            // Against H Line
            SharedPointFP c = new SharedPointFP(40, 80);
            SharedPointFP d = new SharedPointFP(50, 80);

            axline.Position = new SharedPointFP(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new SharedPointFP(45, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLine ()
        {
            AXLineMask axline = new AXLineMask(new SharedPointFP(0, 0), 10);

            // Against V Line
            SharedPointFP c = new SharedPointFP(40, 80);
            SharedPointFP d = new SharedPointFP(40, 90);

            axline.Position = new SharedPointFP(35, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));

            // Against Pos Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(50, 90);

            axline.Position = new SharedPointFP(40, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new SharedPointFP(50, 80);
            d = new SharedPointFP(40, 90);

            axline.Position = new SharedPointFP(40, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));
        }

        // AYLine Line Segment Intersection Tests

        [Test]
        public void TestAYLineIntersectLineNone ()
        {
            AYLineMask ayline = new AYLineMask(new SharedPointFP(0, 0), 10);

            // Against Pos Line
            SharedPointFP c = new SharedPointFP(40, 80);
            SharedPointFP d = new SharedPointFP(50, 90);

            ayline.Position = new SharedPointFP(40, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 74);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 79);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 86);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(39, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(51, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new SharedPointFP(50, 80);
            d = new SharedPointFP(40, 90);

            ayline.Position = new SharedPointFP(40, 79);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 74);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 86);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(39, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(51, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against H Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(50, 80);

            ayline.Position = new SharedPointFP(39, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(51, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against V Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(40, 90);

            ayline.Position = new SharedPointFP(39, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(41, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLineEdge ()
        {
            AYLineMask ayline = new AYLineMask(new SharedPointFP(0, 0), 10);

            // Against Pos Line
            SharedPointFP c = new SharedPointFP(40, 80);
            SharedPointFP d = new SharedPointFP(50, 90);

            ayline.Position = new SharedPointFP(40, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new SharedPointFP(50, 80);
            d = new SharedPointFP(40, 90);

            ayline.Position = new SharedPointFP(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against H Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(50, 80);

            ayline.Position = new SharedPointFP(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(50, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(45, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against V Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(40, 90);

            ayline.Position = new SharedPointFP(40, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLineColinear ()
        {
            AYLineMask ayline = new AYLineMask(new SharedPointFP(0, 0), 10);

            // Against H Line
            SharedPointFP c = new SharedPointFP(40, 80);
            SharedPointFP d = new SharedPointFP(50, 80);

            ayline.Position = new SharedPointFP(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new SharedPointFP(40, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLine ()
        {
            AYLineMask ayline = new AYLineMask(new SharedPointFP(0, 0), 10);

            // Against H Line
            SharedPointFP c = new SharedPointFP(40, 80);
            SharedPointFP d = new SharedPointFP(50, 80);

            ayline.Position = new SharedPointFP(45, 75);
            Assert.IsTrue(ayline.IntersectsLine(c, d));

            // Against Pos Line
            c = new SharedPointFP(40, 80);
            d = new SharedPointFP(50, 90);

            ayline.Position = new SharedPointFP(45, 80);
            Assert.IsTrue(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new SharedPointFP(50, 80);
            d = new SharedPointFP(40, 90);

            ayline.Position = new SharedPointFP(45, 80);
            Assert.IsTrue(ayline.IntersectsLine(c, d));
        }
    }
}
