using System;

#if XNA
using Microsoft.Xna.Framework;
#endif

namespace Amphibian.Geometry
{
    [SerializableAttribute]
    public struct RectangleFP : IEquatable<RectangleFP>
    {
        public FPInt Height;
        public FPInt Width;
        public FPInt X;
        public FPInt Y;

        #region Constructors

        public RectangleFP (FPInt x, FPInt y, FPInt width, FPInt height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleFP (int x, int y, int width, int height)
        {
            X = (FPInt)x;
            Y = (FPInt)y;
            Width = (FPInt)width;
            Height = (FPInt)height;
        }

        #endregion

        #region Properties

        public FPInt Bottom
        {
            get { return Y + Height; }
        }

        public PointFP Center
        {
            get { return new PointFP(X + (Width >> 1), Y + (Height >> 1)); }
        }

        public static RectangleFP Empty
        {
            get { return new RectangleFP(); }
        }

        public bool IsEmpty
        {
            get { return this == Empty; }
        }

        public FPInt Left
        {
            get { return X; }
        }

        public PointFP Location
        {
            get { return new PointFP(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public FPInt Right
        {
            get { return X + Width; }
        }

        public FPInt Top
        {
            get { return Y; }
        }

        #endregion

        #region Modifiers

        public void Inflate (FPInt horizontalAmount, FPInt verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount;
            Height += verticalAmount;
        }

        public void Offset (FPInt offsetX, FPInt offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        public void Offset (PointFP amount)
        {
            X += amount.X;
            Y += amount.Y;
        }

        #endregion

        #region Set Operations

        public bool Contains (FPInt x, FPInt y)
        {
            return (x >= Left) && (x <= Right) && (y >= Top) && (y <= Bottom);
        }

        public bool Contains (PointFP value)
        {
            return (value.X >= Left) && (value.X <= Right) && (value.Y >= Top) && (value.Y <= Bottom);
        }

        public bool Contains (RectangleFP value)
        {
            return (value.Left >= Left) && (value.Right <= Right) && (value.Top >= Top) && (value.Bottom <= Bottom);
        }

        public bool Intersects (RectangleFP value)
        {
            return !((value.Left >= Right) || (value.Right <= Left) || (value.Top >= Bottom) || (value.Bottom <= Top));
        }

        public static RectangleFP Intersect (RectangleFP value1, RectangleFP value2)
        {
            if (!value1.Intersects(value2)) {
                return Empty;
            }

            FPInt maxx = FPMath.Max(value1.Left, value2.Left);
            FPInt maxy = FPMath.Max(value1.Top, value2.Top);

            return new RectangleFP(
                maxx, maxy,
                FPMath.Min(value1.Right, value2.Right) - maxx,
                FPMath.Min(value1.Bottom, value2.Bottom) - maxy
                );
        }

        public static RectangleFP Union (RectangleFP value1, RectangleFP value2)
        {
            FPInt minx = FPMath.Min(value1.Left, value2.Left);
            FPInt miny = FPMath.Min(value1.Top, value2.Top);

            return new RectangleFP(
                minx, miny,
                FPMath.Max(value1.Right, value2.Right) - minx,
                FPMath.Max(value1.Bottom, value2.Bottom) - miny
                );
        }

        #endregion

        #region Comparison Operators

        public static bool operator == (RectangleFP v1, RectangleFP v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Width == v2.Width) && (v1.Height == v2.Height);
        }

        public static bool operator != (RectangleFP v1, RectangleFP v2)
        {
            return (v1.X != v2.X) || (v1.Y != v2.Y) || (v1.Width != v2.Width) || (v1.Height != v2.Height);
        }

        #endregion

        #region Implicit Conversions
#if XNA
        public static implicit operator RectangleFP (Rectangle value) {
            return new RectangleFP(value.X, value.Y, value.Width, value.Height);
        }
#endif
        #endregion

        #region Explicit Conversions
#if XNA
        public static explicit operator Rectangle (RectangleFP value)
        {
            return new Rectangle(value.X.Round, value.Y.Round, value.Width.Round, value.Height.Round);
        }
#endif
        #endregion

        #region Object Overrides

        public override bool Equals (object obj)
        {
            if (obj is RectangleFP) {
                return (RectangleFP)obj == this;
            }
            return false;
        }

        public bool Equals (RectangleFP other)
        {
            return other == this;
        }

        public override int GetHashCode ()
        {
            int hash = X.GetHashCode() * 37;
            hash ^= Y.GetHashCode() * 37;
            hash ^= Width.GetHashCode() * 37;
            return hash ^ Height.GetHashCode();
        }

        public override string ToString ()
        {
            return "(" + X + ", " + Y + ", " + Width + ", " + Height + ")";
        }

        #endregion
    }
}
