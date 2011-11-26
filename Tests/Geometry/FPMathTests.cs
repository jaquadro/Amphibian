using System;
using NUnit.Framework;
using Amphibian.Geometry;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Amphibian.Tests.Geometry
{
    [TestFixture]
    class FPMathTests
    {

        [Test]
        public void FloorTest ()
        {
            FPInt i1 = (FPInt)(4.75);
            FPInt i2 = (FPInt)(4.25);
            FPInt i3 = (FPInt)(4);
            FPInt i4 = (FPInt)(0);
            FPInt i5 = (FPInt)(-4);
            FPInt i6 = (FPInt)(-4.25);
            FPInt i7 = (FPInt)(-4.75);

            Assert.AreEqual(4, i1.Floor);
            Assert.AreEqual(4, i2.Floor);
            Assert.AreEqual(4, i3.Floor);
            Assert.AreEqual(0, i4.Floor);
            Assert.AreEqual(-4, i5.Floor);
            Assert.AreEqual(-5, i6.Floor);
            Assert.AreEqual(-5, i7.Floor);

            FPLong l1 = (FPLong)(4.75);
            FPLong l2 = (FPLong)(4.25);
            FPLong l3 = (FPLong)(4);
            FPLong l4 = (FPLong)(0);
            FPLong l5 = (FPLong)(-4);
            FPLong l6 = (FPLong)(-4.25);
            FPLong l7 = (FPLong)(-4.75);

            Assert.AreEqual(4, l1.Floor);
            Assert.AreEqual(4, l2.Floor);
            Assert.AreEqual(4, l3.Floor);
            Assert.AreEqual(0, l4.Floor);
            Assert.AreEqual(-4, l5.Floor);
            Assert.AreEqual(-5, l6.Floor);
            Assert.AreEqual(-5, l7.Floor);
        }

        [Test]
        public void CeilTest ()
        {
            FPInt i1 = (FPInt)(4.75);
            FPInt i2 = (FPInt)(4.25);
            FPInt i3 = (FPInt)(4);
            FPInt i4 = (FPInt)(0);
            FPInt i5 = (FPInt)(-4);
            FPInt i6 = (FPInt)(-4.25);
            FPInt i7 = (FPInt)(-4.75);
            FPInt i8 = (FPInt)(-4.00390625);
            FPInt i9 = (FPInt)(-4.99609375);

            Assert.AreEqual(5, i1.Ceil);
            Assert.AreEqual(5, i2.Ceil);
            Assert.AreEqual(4, i3.Ceil);
            Assert.AreEqual(0, i4.Ceil);
            Assert.AreEqual(-4, i5.Ceil);
            Assert.AreEqual(-4, i6.Ceil);
            Assert.AreEqual(-4, i7.Ceil);
            Assert.AreEqual(-4, i8.Ceil);
            Assert.AreEqual(-4, i9.Ceil);

            FPLong l1 = (FPLong)(4.75);
            FPLong l2 = (FPLong)(4.25);
            FPLong l3 = (FPLong)(4);
            FPLong l4 = (FPLong)(0);
            FPLong l5 = (FPLong)(-4);
            FPLong l6 = (FPLong)(-4.25);
            FPLong l7 = (FPLong)(-4.75);

            Assert.AreEqual(5, l1.Ceil);
            Assert.AreEqual(5, l2.Ceil);
            Assert.AreEqual(4, l3.Ceil);
            Assert.AreEqual(0, l4.Ceil);
            Assert.AreEqual(-4, l5.Ceil);
            Assert.AreEqual(-4, l6.Ceil);
            Assert.AreEqual(-4, l7.Ceil);
        }

        [Test]
        public void RoundTest ()
        {
            FPInt i1 = (FPInt)(4.75);
            FPInt i2 = (FPInt)(4.25);
            FPInt i3 = (FPInt)(4);
            FPInt i4 = (FPInt)(0);
            FPInt i5 = (FPInt)(-4);
            FPInt i6 = (FPInt)(-4.25);
            FPInt i7 = (FPInt)(-4.75);

            Assert.AreEqual(5, i1.Round);
            Assert.AreEqual(4, i2.Round);
            Assert.AreEqual(4, i3.Round);
            Assert.AreEqual(0, i4.Round);
            Assert.AreEqual(-4, i5.Round);
            Assert.AreEqual(-4, i6.Round);
            Assert.AreEqual(-5, i7.Round);

            FPLong l1 = (FPLong)(4.75);
            FPLong l2 = (FPLong)(4.25);
            FPLong l3 = (FPLong)(4);
            FPLong l4 = (FPLong)(0);
            FPLong l5 = (FPLong)(-4);
            FPLong l6 = (FPLong)(-4.25);
            FPLong l7 = (FPLong)(-4.75);

            Assert.AreEqual(5, l1.Round);
            Assert.AreEqual(4, l2.Round);
            Assert.AreEqual(4, l3.Round);
            Assert.AreEqual(0, l4.Round);
            Assert.AreEqual(-4, l5.Round);
            Assert.AreEqual(-4, l6.Round);
            Assert.AreEqual(-5, l7.Round);
        }

        [Test]
        public void CastIntToLongTest ()
        {
            FPInt i1 = (FPInt)(-48160);
            FPLong l1 = (FPLong)(-48160);
            Assert.AreEqual(l1, (FPLong)i1);
        }
    }
}
