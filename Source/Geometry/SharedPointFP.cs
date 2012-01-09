using System;

#if XNA
using Microsoft.Xna.Framework;
#endif

namespace Amphibian.Geometry
{
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

        public override int GetHashCode ()
        {
            int hash = 23;
            hash = hash * 37 + X._raw;
            hash = hash * 37 + Y._raw;
            return hash;
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

#if XNA
        public static implicit operator SharedPointFP (Point src)
        {
            return new SharedPointFP(src.X, src.Y);
        }
#endif

        #endregion

        #region Explicit Conversions

        public static explicit operator SharedPointFP (PointFP value)
        {
            return new SharedPointFP(value.X, value.Y);
        }

#if XNA
        public static explicit operator Point (SharedPointFP src)
        {
            return new Point(src.X.Round, src.Y.Round);
        }

        public static explicit operator Vector2 (SharedPointFP src)
        {
            return new Vector2((float)src.X, (float)src.Y);
        }

        public static explicit operator SharedPointFP (Vector2 src)
        {
            return new SharedPointFP((FPInt)src.X, (FPInt)src.Y);
        }
#endif

        #endregion
    }
}
