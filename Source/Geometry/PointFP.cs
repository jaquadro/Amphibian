using System;

#if XNA
using Microsoft.Xna.Framework;
#endif

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

        #region Modifiers

        public void Offset (FPInt offsetX, FPInt offsetY)
        {
            X += offsetX;
            Y += offsetY;
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

        #region Implicit Conversions
#if XNA
        public static implicit operator PointFP (Point src)
        {
            return new PointFP(src.X, src.Y);
        }
#endif
        #endregion

        #region Explicit Conversions
#if XNA
        public static explicit operator Point (PointFP src)
        {
            return new Point(src.X.Round, src.Y.Round);
        }

        public static explicit operator Vector2 (PointFP src)
        {
            return new Vector2((float)src.X, (float)src.Y);
        }

        public static explicit operator PointFP (Vector2 src)
        {
            return new PointFP((FPInt)src.X, (FPInt)src.Y);
        }
#endif
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
