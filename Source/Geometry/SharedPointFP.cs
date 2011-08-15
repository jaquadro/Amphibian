using System;

namespace Amphibian.Geometry
{
    [SerializableAttribute]
    public sealed class SharedPointFP : IEquatable<SharedPointFP>
    {
        public FPInt X;
        public FPInt Y;

        #region Constructors

        public SharedPointFP (FPInt x, FPInt y)
        {
            X = x;
            Y = y;
        }

        public SharedPointFP (int x, int y)
        {
            X = (FPInt)x;
            Y = (FPInt)y;
        }

        #endregion

        #region Properties

        public static PointFP Zero
        {
            get { return new PointFP(); }
        }

        #endregion

        #region Modifiers

        public void Offset (FPInt offsetX, FPInt offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        #endregion

        #region Comparison Operators

        public static bool operator == (SharedPointFP v1, SharedPointFP v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y);
        }


        public static bool operator != (SharedPointFP v1, SharedPointFP v2)
        {
            return (v1.X != v2.X) || (v1.Y != v2.Y);
        }

        #endregion

        #region Object Overrides

        public override bool Equals (object obj)
        {
            if (obj is SharedPointFP) {
                return (SharedPointFP)obj == this;
            }
            return false;
        }

        public bool Equals (SharedPointFP other)
        {
            return other == this;
        }

        public override string ToString ()
        {
            return "(" + X + ", " + Y + ")";
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator PointFP (SharedPointFP value)
        {
            return new PointFP(value.X, value.Y);
        }

        public static implicit operator VectorFP (SharedPointFP value)
        {
            return new VectorFP(value.X, value.Y);
        }

        #endregion

        #region Explicit Conversions

        public static explicit operator SharedPointFP (PointFP value)
        {
            return new SharedPointFP(value.X, value.Y);
        }

        #endregion
    }
}
