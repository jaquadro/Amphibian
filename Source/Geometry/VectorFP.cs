using System;

#if XNA
using Microsoft.Xna.Framework;
#endif

namespace Amphibian.Geometry
{
    [SerializableAttribute]
    public struct VectorFP : IEquatable<VectorFP>
    {
        public FPInt X;
        public FPInt Y;

        #region Constructors

        public VectorFP (FPInt x, FPInt y)
        {
            X = x;
            Y = y;
        }

        public VectorFP (int x, int y)
        {
            X = (FPInt)x;
            Y = (FPInt)y;
        }

        #endregion

        #region Properties

        public static VectorFP One
        {
            get { return new VectorFP(1, 1); }
        }

        public static VectorFP UnitX
        {
            get { return new VectorFP(1, 0); }
        }

        public static VectorFP UnitY
        {
            get { return new VectorFP(0, 1); }
        }

        public static VectorFP Zero
        {
            get { return new VectorFP(); }
        }

        #endregion

        #region Vector Arithmetic Operators

        public static VectorFP operator + (VectorFP v1, VectorFP v2)
        {
            return new VectorFP(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static VectorFP operator - (VectorFP v1, VectorFP v2)
        {
            return new VectorFP(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static VectorFP operator * (VectorFP v1, VectorFP v2)
        {
            return new VectorFP(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static VectorFP operator * (VectorFP value, FPInt scale)
        {
            return new VectorFP(value.X * scale, value.Y * scale);
        }

        public static VectorFP operator * (FPInt scale, VectorFP value)
        {
            return new VectorFP(value.X * scale, value.Y * scale);
        }

        public static VectorFP operator / (VectorFP v1, VectorFP v2)
        {
            return new VectorFP(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static VectorFP operator / (VectorFP value, FPInt scale)
        {
            return new VectorFP(value.X / scale, value.Y / scale);
        }

        public static VectorFP operator - (VectorFP value)
        {
            return new VectorFP(value.X.Inverse, value.Y.Inverse);
        }

        #endregion

        #region Vector Products

        public static FPInt Dot (VectorFP v1, VectorFP v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }

        #endregion

        #region Vector Metrics

        public static FPInt DistanceSquared (VectorFP v1, VectorFP v2)
        {
            FPInt x = v1.X - v2.X;
            FPInt y = v1.Y - v2.Y;
            return (x * x + y * y);
        }

        public FPInt LengthSquared ()
        {
            return (X * X + Y * Y);
        }

        #endregion

        #region Comparison Operators

        public static bool operator == (VectorFP v1, VectorFP v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y);
        }

        public static bool operator != (VectorFP v1, VectorFP v2)
        {
            return (v1.X != v2.X) || (v1.Y != v2.Y);
        }

        #endregion

        #region Object Overrides

        public override bool Equals (object obj)
        {
            if (obj is VectorFP) {
                return (VectorFP)obj == this;
            }
            return false;
        }

        public bool Equals (VectorFP other)
        {
            return other == this;
        }

        public override int GetHashCode ()
        {
            int hash = X.GetHashCode() * 37;
            return hash ^ Y.GetHashCode();
        }

        public override string ToString ()
        {
            return "(" + X + ", " + Y + ")";
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator PointFP (VectorFP value)
        {
            return new PointFP(value.X, value.Y);
        }

        public static implicit operator VectorFP (PointFP value)
        {
            return new VectorFP(value.X, value.Y);
        }

        #endregion

        #region Explicit Conversions
#if XNA
        public static explicit operator VectorFP (Vector2 value)
        {
            return new VectorFP((FPInt)value.X, (FPInt)value.Y);
        }

        public static explicit operator Vector2 (VectorFP value)
        {
            return new Vector2((float)value.X, (float)value.Y);
        }

        public static explicit operator Point (VectorFP value)
        {
            return new Point(value.X.Round, value.Y.Round);
        }
#endif
        #endregion


    }
}
