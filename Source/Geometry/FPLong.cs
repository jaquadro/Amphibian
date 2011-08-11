using System;

namespace Amphibian.Geometry
{
    [SerializableAttribute]
    public struct FPLong
    {
        internal const int ShiftBy = 16; // 65536
        internal const int ShiftByHalf = ShiftBy / 2;
        internal const int SubMask = (1 << ShiftBy) - 1;
        internal const int RoundingFactor = (1 << (ShiftBy - 1));

        internal const long OneL = 1 << ShiftBy;
        internal const int OneI = 1 << ShiftBy;

        internal static FPLong OneFP = new FPLong(1);

        internal long _raw;

        #region Constructors

        public FPLong (int main)
        {
            _raw = main << ShiftBy;
        }

        public FPLong (int main, int sub)
        {
            _raw = (main << ShiftBy) + sub;
        }

        public static FPLong FromRaw (long raw)
        {
            return new FPLong() { _raw = raw };
        }

        #endregion

        #region Properties

        public int Floor
        {
            get { return (int)(_raw >> ShiftBy); }
        }

        public int Ceil
        {
            //get { return (int)(_raw >> ShiftBy) + ((_raw & SubMask) != 0 ? 1 : 0); }
            get { return (int)((_raw + ((OneL - _raw) & SubMask)) >> ShiftBy); }
        }

        public int Round
        {
            get { return (int)((_raw + RoundingFactor) >> ShiftBy); }
        }

        public FPLong Inverse
        {
            get { return new FPLong() { _raw = -this._raw }; }
        }

        #endregion

        #region Arithmetic Operators

        public static FPLong operator + (FPLong v1, FPLong v2)
        {
            return new FPLong() { _raw = v1._raw + v2._raw };
        }

        public static FPLong operator + (FPLong v1, int v2)
        {
            return new FPLong() { _raw = v1._raw + (v2 << ShiftBy) };
        }

        public static FPLong operator + (int v1, FPLong v2)
        {
            return new FPLong() { _raw = (v1 << ShiftBy) + v2._raw };
        }

        public static FPLong operator - (FPLong v1, FPLong v2)
        {
            return new FPLong() { _raw = v1._raw - v2._raw };
        }

        public static FPLong operator - (FPLong v1, int v2)
        {
            return new FPLong() { _raw = v1._raw - (v2 << ShiftBy) };
        }

        public static FPLong operator - (int v1, FPLong v2)
        {
            return new FPLong() { _raw = (v1 << ShiftBy) - v2._raw };
        }

        public static FPLong operator * (FPLong v1, FPLong v2)
        {
            return new FPLong() { _raw = (v1._raw >> ShiftByHalf) * (v2._raw >> ShiftByHalf) };
        }

        public static FPLong operator * (FPLong v1, int v2)
        {
            return new FPLong() { _raw = v1._raw  * v2 };
        }

        public static FPLong operator * (int v1, FPLong v2)
        {
            return new FPLong() { _raw = v1 * v2._raw };
        }

        public static FPLong operator / (FPLong v1, FPLong v2)
        {
            return new FPLong() { _raw = (v1._raw << ShiftBy) / v2._raw };
        }

        public static FPLong operator / (FPLong v1, int v2)
        {
            return new FPLong() { _raw = v1._raw / v2 };
        }

        public static FPLong operator / (int v1, FPLong v2)
        {
            return new FPLong() { _raw = (long)(v1 << (ShiftBy * 2)) / v2._raw };
        }

        public static FPLong operator % (FPLong v1, FPLong v2)
        {
            return new FPLong() { _raw = v1._raw % v2._raw };
        }

        public static FPLong operator % (FPLong v1, int v2)
        {
            return new FPLong() { _raw = v1._raw % (v2 << ShiftBy) };
        }

        public static FPLong operator % (int v1, FPLong v2)
        {
            return new FPLong() { _raw = (v1 << ShiftBy) % v2._raw };
        }

        public static FPLong operator - (FPLong value)
        {
            return new FPLong() { _raw = -value._raw };
        }

        #endregion

        #region Comparison Operators

        public static bool operator == (FPLong v1, FPLong v2)
        {
            return v1._raw == v2._raw;
        }

        public static bool operator == (FPLong v1, int v2)
        {
            return v1._raw == (v2 << ShiftBy);
        }

        public static bool operator == (int v1, FPLong v2)
        {
            return (v1 << ShiftBy) == v2._raw;
        }

        public static bool operator != (FPLong v1, FPLong v2)
        {
            return v1._raw != v2._raw;
        }

        public static bool operator != (FPLong v1, int v2)
        {
            return v1._raw != (v2 << ShiftBy);
        }

        public static bool operator != (int v1, FPLong v2)
        {
            return (v1 << ShiftBy) != v2._raw;
        }

        public static bool operator >= (FPLong v1, FPLong v2)
        {
            return v1._raw >= v2._raw;
        }

        public static bool operator >= (FPLong v1, int v2)
        {
            return v1._raw >= (v2 << ShiftBy);
        }

        public static bool operator >= (int v1, FPLong v2)
        {
            return (v1 << ShiftBy) >= v2._raw;
        }

        public static bool operator <= (FPLong v1, FPLong v2)
        {
            return v1._raw <= v2._raw;
        }

        public static bool operator <= (FPLong v1, int v2)
        {
            return v1._raw <= (v2 << ShiftBy);
        }

        public static bool operator <= (int v1, FPLong v2)
        {
            return (v1 << ShiftBy) <= v2._raw;
        }

        public static bool operator > (FPLong v1, FPLong v2)
        {
            return v1._raw > v2._raw;
        }

        public static bool operator > (FPLong v1, int v2)
        {
            return v1._raw > (v2 << ShiftBy);
        }

        public static bool operator > (int v1, FPLong v2)
        {
            return (v1 << ShiftBy) > v2._raw;
        }

        public static bool operator < (FPLong v1, FPLong v2)
        {
            return v1._raw < v2._raw;
        }

        public static bool operator < (FPLong v1, int v2)
        {
            return v1._raw < (v2 << ShiftBy);
        }

        public static bool operator < (int v1, FPLong v2)
        {
            return (v1 << ShiftBy) < v2._raw;
        }

        #endregion

        #region Bit Operators

        public static FPLong operator << (FPLong v, int amount)
        {
            return new FPLong() { _raw = v._raw << amount };
        }

        public static FPLong operator >> (FPLong v, int amount)
        {
            return new FPLong() { _raw = v._raw >> amount };
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator FPLong (byte src)
        {
            return new FPLong() { _raw = (long)src << ShiftBy };
        }

        public static implicit operator FPLong (short src)
        {
            return new FPLong() { _raw = (long)src << ShiftBy };
        }

        public static implicit operator FPLong (int src)
        {
            return new FPLong() { _raw = (long)src << ShiftBy };
        }

        public static implicit operator FPLong (FPInt src)
        {
            return new FPLong() { _raw = src._raw << (ShiftBy - FPInt.ShiftBy) };
        }

        #endregion

        #region Explicit Conversions

        public static explicit operator int (FPLong src)
        {
            return (int)(src._raw >> ShiftBy);
        }

        public static explicit operator float (FPLong src)
        {
            return (float)src._raw / (float)OneL;
        }

        public static explicit operator double (FPLong src)
        {
            return (double)src._raw / (double)OneL;
        }

        public static explicit operator FPInt (FPLong src)
        {
            return new FPInt() { _raw = (int)(src._raw >> (ShiftBy - FPInt.ShiftBy)) };
        }

        public static explicit operator FPLong (long src)
        {
            return new FPLong() { _raw = src << ShiftBy };
        }

        public static explicit operator FPLong (float src)
        {
            return new FPLong() { _raw = (long)(src * OneL) };
        }

        public static explicit operator FPLong (double src)
        {
            return new FPLong() { _raw = (long)(src * OneL) };
        }

        #endregion

        #region Object Overrides

        public override bool Equals (object obj)
        {
            if (obj is FPLong) {
                return ((FPLong)obj)._raw == this._raw;
            }
            else {
                return false;
            }
        }

        public override int GetHashCode ()
        {
            return _raw.GetHashCode();
        }

        public override string ToString ()
        {
            return (_raw >> ShiftBy) + "+" + ((_raw & SubMask) / OneL);
        }

        #endregion
    }
}
