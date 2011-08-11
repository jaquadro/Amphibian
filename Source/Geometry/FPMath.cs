using System;

namespace Amphibian.Geometry
{
    public static class FPMath
    {
        #region Min / Max

        public static FPInt Max (FPInt v1, FPInt v2)
        {
            return (v1._raw > v2._raw) ? v1 : v2;
        }

        public static FPLong Max (FPLong v1, FPLong v2)
        {
            return (v1._raw > v2._raw) ? v1 : v2;
        }

        public static FPInt Min (FPInt v1, FPInt v2)
        {
            return (v1._raw < v2._raw) ? v1 : v2;
        }

        public static FPLong Min (FPLong v1, FPLong v2)
        {
            return (v1._raw < v2._raw) ? v1 : v2;
        }

        #endregion

        #region Absolute Value

        public static FPInt Abs (FPInt v)
        {
            if (v._raw < 0) {
                return v.Inverse;
            }
            else {
                return v;
            }
        }

        public static FPLong Abs (FPLong v)
        {
            if (v._raw < 0) {
                return v.Inverse;
            }
            else {
                return v;
            }
        }

        #endregion

        #region Square Root

        public static FPInt Sqrt (FPInt f, int numberOfIterations)
        {
            if (f._raw < 0) //NaN in Math.Sqrt
                throw new ArithmeticException("Input Error");
            if (f._raw == 0)
                return 0;
            FPInt k = f + FPInt.OneFP >> 1;
            for (int i = 0; i < numberOfIterations; i++)
                k = (k + (f / k)) >> 1;

            if (k._raw < 0)
                throw new ArithmeticException("Overflow");
            else
                return k;
        }

        public static FPInt Sqrt (FPInt f)
        {
            byte numberOfIterations = 8;
            if (f._raw > 100 * FPInt.OneL)
                numberOfIterations = 12;
            if (f._raw > 1000 * FPInt.OneL)
                numberOfIterations = 16;
            return Sqrt(f, numberOfIterations);
        }

        public static FPLong Sqrt (FPLong f, int numberOfIterations)
        {
            if (f._raw < 0) //NaN in Math.Sqrt
                throw new ArithmeticException("Input Error");
            if (f._raw == 0)
                return 0;
            FPLong k = f + FPLong.OneFP >> 1;
            for (int i = 0; i < numberOfIterations; i++)
                k = (k + (f / k)) >> 1;

            if (k._raw < 0)
                throw new ArithmeticException("Overflow");
            else
                return k;
        }

        public static FPLong Sqrt (FPLong f)
        {
            byte numberOfIterations = 8;
            if (f._raw > 100 * FPLong.OneL)
                numberOfIterations = 12;
            if (f._raw > 1000 * FPLong.OneL)
                numberOfIterations = 16;
            return Sqrt(f, numberOfIterations);
        }

        #endregion
    }
}
