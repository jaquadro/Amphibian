using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision.Tests
{
    [TestFixture]
    class MaskInternalTests
    {
        // AXLine Line Segment Intersection Tests

        [Test]
        public void TestAXLineIntersectLineNone ()
        {
            AXLineMask axline = new AXLineMask(new Vector2(0, 0), 10);

            // Against Pos Line
            Vector2 c = new Vector2(40, 80);
            Vector2 d = new Vector2(50, 90);

            axline.Position = new Vector2(29, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(34, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(39, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(41, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(46, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(51, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(45, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new Vector2(50, 80);
            d = new Vector2(40, 90);

            axline.Position = new Vector2(39, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(34, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(29, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(51, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(46, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(41, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(55, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against H Line
            c = new Vector2(40, 80);
            d = new Vector2(50, 80);

            axline.Position = new Vector2(29, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(51, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(40, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(40, 81);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against V Line
            c = new Vector2(40, 80);
            d = new Vector2(40, 90);

            axline.Position = new Vector2(29, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(41, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 79);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 91);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLineEdge ()
        {
            AXLineMask axline = new AXLineMask(new Vector2(0, 0), 10);

            // Against Pos Line
            Vector2 c = new Vector2(40, 80);
            Vector2 d = new Vector2(50, 90);

            axline.Position = new Vector2(30, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(40, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(45, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(50, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(45, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new Vector2(50, 80);
            d = new Vector2(40, 90);

            axline.Position = new Vector2(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(30, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(50, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(45, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(40, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(55, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against H Line
            c = new Vector2(40, 80);
            d = new Vector2(50, 80);

            axline.Position = new Vector2(30, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(50, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            // Against V Line
            c = new Vector2(40, 80);
            d = new Vector2(40, 90);

            axline.Position = new Vector2(30, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(40, 85);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(35, 90);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLineColinear ()
        {
            AXLineMask axline = new AXLineMask(new Vector2(0, 0), 10);

            // Against H Line
            Vector2 c = new Vector2(40, 80);
            Vector2 d = new Vector2(50, 80);

            axline.Position = new Vector2(35, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(40, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));

            axline.Position = new Vector2(45, 80);
            Assert.IsFalse(axline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAXLineIntersectLine ()
        {
            AXLineMask axline = new AXLineMask(new Vector2(0, 0), 10);

            // Against V Line
            Vector2 c = new Vector2(40, 80);
            Vector2 d = new Vector2(40, 90);

            axline.Position = new Vector2(35, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));

            // Against Pos Line
            c = new Vector2(40, 80);
            d = new Vector2(50, 90);

            axline.Position = new Vector2(40, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));

            // Against Neg Line
            c = new Vector2(50, 80);
            d = new Vector2(40, 90);

            axline.Position = new Vector2(40, 85);
            Assert.IsTrue(axline.IntersectsLine(c, d));
        }

        // AYLine Line Segment Intersection Tests

        [Test]
        public void TestAYLineIntersectLineNone ()
        {
            AYLineMask ayline = new AYLineMask(new Vector2(0, 0), 10);

            // Against Pos Line
            Vector2 c = new Vector2(40, 80);
            Vector2 d = new Vector2(50, 90);

            ayline.Position = new Vector2(40, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 74);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 79);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 86);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(39, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(51, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new Vector2(50, 80);
            d = new Vector2(40, 90);

            ayline.Position = new Vector2(40, 79);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 74);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 86);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(39, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(51, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against H Line
            c = new Vector2(40, 80);
            d = new Vector2(50, 80);

            ayline.Position = new Vector2(39, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(51, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 81);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against V Line
            c = new Vector2(40, 80);
            d = new Vector2(40, 90);

            ayline.Position = new Vector2(39, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(41, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 69);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 91);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLineEdge ()
        {
            AYLineMask ayline = new AYLineMask(new Vector2(0, 0), 10);

            // Against Pos Line
            Vector2 c = new Vector2(40, 80);
            Vector2 d = new Vector2(50, 90);

            ayline.Position = new Vector2(40, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new Vector2(50, 80);
            d = new Vector2(40, 90);

            ayline.Position = new Vector2(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against H Line
            c = new Vector2(40, 80);
            d = new Vector2(50, 80);

            ayline.Position = new Vector2(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(50, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(45, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            // Against V Line
            c = new Vector2(40, 80);
            d = new Vector2(40, 90);

            ayline.Position = new Vector2(40, 70);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 90);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLineColinear ()
        {
            AYLineMask ayline = new AYLineMask(new Vector2(0, 0), 10);

            // Against H Line
            Vector2 c = new Vector2(40, 80);
            Vector2 d = new Vector2(50, 80);

            ayline.Position = new Vector2(40, 75);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 80);
            Assert.IsFalse(ayline.IntersectsLine(c, d));

            ayline.Position = new Vector2(40, 85);
            Assert.IsFalse(ayline.IntersectsLine(c, d));
        }

        [Test]
        public void TestAYLineIntersectLine ()
        {
            AYLineMask ayline = new AYLineMask(new Vector2(0, 0), 10);

            // Against H Line
            Vector2 c = new Vector2(40, 80);
            Vector2 d = new Vector2(50, 80);

            ayline.Position = new Vector2(45, 75);
            Assert.IsTrue(ayline.IntersectsLine(c, d));

            // Against Pos Line
            c = new Vector2(40, 80);
            d = new Vector2(50, 90);

            ayline.Position = new Vector2(45, 80);
            Assert.IsTrue(ayline.IntersectsLine(c, d));

            // Against Neg Line
            c = new Vector2(50, 80);
            d = new Vector2(40, 90);

            ayline.Position = new Vector2(45, 80);
            Assert.IsTrue(ayline.IntersectsLine(c, d));
        }
    }
}
