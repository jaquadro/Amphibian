using System;

namespace Amphibian.Geometry
{
    [SerializableAttribute]
    public struct PointFP : IEquatable<PointFP>
    {
        public FPInt X;
        public FPInt Y;

        #region Constructors

        public PointFP (FPInt x, FPInt y)
        {
            X = x;
            Y = y;
        }

        public PointFP (int x, int y)
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

        #region Comparison Operators

        public static bool operator == (PointFP v1, PointFP v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y);
        }

        public static bool operator != (PointFP v1, PointFP v2)
        {
            return (v1.X != v2.X) || (v1.Y != v2.Y);
        }

        #endregion

        #region Object Overrides

        public override bool Equals (object obj)
        {
            if (obj is PointFP) {
                return (PointFP)obj == this;
            }
            return false;
        }

        public bool Equals (PointFP other)
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
    }
}
