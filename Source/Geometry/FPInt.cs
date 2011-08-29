using System;

namespace Amphibian.Geometry
{
    [SerializableAttribute]
    public struct FPInt
    {
        internal const int ShiftBy = 8; // 256
        internal const int ShiftByHalf = ShiftBy / 2;
        internal const int SubMask = (1 << ShiftBy) - 1;
        internal const int RoundingFactor = (1 << (ShiftBy - 1));

        internal const long OneL = 1 << ShiftBy;
        internal const int OneI = 1 << ShiftBy;

        internal static FPInt OneFP = new FPInt(1);

        internal int _raw;

        #region Constructors

        public FPInt (int main)
        {
            _raw = main << ShiftBy;
        }

        public FPInt (int main, int sub)
        {
            _raw = (main << ShiftBy) + sub;
        }

        public static FPInt FromRaw (int raw)
        {
            return new FPInt() { _raw = raw };
        }

        #endregion

        #region Properties

        public int Floor
        {
            get { return (int)(_raw >> ShiftBy); }
        }

        public int Ceil
        {
            get { return (int)((_raw + ((OneI - _raw) & SubMask)) >> ShiftBy); }
        }

        public int Round
        {
            get { return (int)((_raw + RoundingFactor) >> ShiftBy); }
        }

        public int Trunc
        {
            get { return (_raw >= 0) ? Floor : Ceil; }
        }

        public FPInt Inverse
        {
            get { return new FPInt() { _raw = -this._raw }; }
        }

        #endregion

        #region Arithmetic Operators

        public static FPInt operator + (FPInt v1, FPInt v2)
        {
            return new FPInt() { _raw = v1._raw + v2._raw };
        }

        public static FPInt operator + (FPInt v1, int v2)
        {
            return new FPInt() { _raw = v1._raw + (v2 << ShiftBy) };
        }

        public static FPInt operator + (int v1, FPInt v2)
        {
            return new FPInt() { _raw = (v1 << ShiftBy) + v2._raw };
        }

        public static FPInt operator - (FPInt v1, FPInt v2)
        {
            return new FPInt() { _raw = v1._raw - v2._raw };
        }

        public static FPInt operator - (FPInt v1, int v2)
        {
            return new FPInt() { _raw = v1._raw - (v2 << ShiftBy) };
        }

        public static FPInt operator - (int v1, FPInt v2)
        {
            return new FPInt() { _raw = (v1 << ShiftBy) - v2._raw };
        }

        public static FPInt operator * (FPInt v1, FPInt v2)
        {
            return new FPInt() { _raw = (int)(((long)v1._raw * (long)v2._raw) >> ShiftBy) };
        }

        public static FPInt operator * (FPInt v1, int v2)
        {
            return new FPInt() { _raw = v1._raw * v2 };
        }

        public static FPInt operator * (int v1, FPInt v2)
        {
            return new FPInt() { _raw = v1 * v2._raw };
        }

        public static FPInt operator / (FPInt v1, FPInt v2)
        {
            return new FPInt() { _raw = (int)(((long)v1._raw << ShiftBy) / v2._raw) };
        }

        public static FPInt operator / (FPInt v1, int v2)
        {
            return new FPInt() { _raw = v1._raw / v2 };
        }

        public static FPInt operator / (int v1, FPInt v2)
        {
            return new FPInt() { _raw = (int)((long)v1 << (ShiftBy * 2)) / v2._raw };
        }

        public static FPInt operator % (FPInt v1, FPInt v2)
        {
            return new FPInt() { _raw = v1._raw % v2._raw };
        }

        public static FPInt operator % (FPInt v1, int v2)
        {
            return new FPInt() { _raw = v1._raw % (v2 << ShiftBy) };
        }

        public static FPInt operator % (int v1, FPInt v2)
        {
            return new FPInt() { _raw = (v1 << ShiftBy) % v2._raw };
        }

        public static FPInt operator - (FPInt value)
        {
            return new FPInt() { _raw = -value._raw };
        }

        #endregion

        #region Comparison Operators

        public static bool operator == (FPInt v1, FPInt v2)
        {
            return v1._raw == v2._raw;
        }

        public static bool operator == (FPInt v1, int v2)
        {
            return v1._raw == (v2 << ShiftBy);
        }

        public static bool operator == (int v1, FPInt v2)
        {
            return (v1 << ShiftBy) == v2._raw;
        }

        public static bool operator != (FPInt v1, FPInt v2)
        {
            return v1._raw != v2._raw;
        }

        public static bool operator != (FPInt v1, int v2)
        {
            return v1._raw != (v2 << ShiftBy);
        }

        public static bool operator != (int v1, FPInt v2)
        {
            return (v1 << ShiftBy) != v2._raw;
        }

        public static bool operator >= (FPInt v1, FPInt v2)
        {
            return v1._raw >= v2._raw;
        }

        public static bool operator >= (FPInt v1, int v2)
        {
            return v1._raw >= (v2 << ShiftBy);
        }

        public static bool operator >= (int v1, FPInt v2)
        {
            return (v1 << ShiftBy) >= v2._raw;
        }

        public static bool operator <= (FPInt v1, FPInt v2)
        {
            return v1._raw <= v2._raw;
        }

        public static bool operator <= (FPInt v1, int v2)
        {
            return v1._raw <= (v2 << ShiftBy);
        }

        public static bool operator <= (int v1, FPInt v2)
        {
            return (v1 << ShiftBy) <= v2._raw;
        }

        public static bool operator > (FPInt v1, FPInt v2)
        {
            return v1._raw > v2._raw;
        }

        public static bool operator > (FPInt v1, int v2)
        {
            return v1._raw > (v2 << ShiftBy);
        }

        public static bool operator > (int v1, FPInt v2)
        {
            return (v1 << ShiftBy) > v2._raw;
        }

        public static bool operator < (FPInt v1, FPInt v2)
        {
            return v1._raw < v2._raw;
        }

        public static bool operator < (FPInt v1, int v2)
        {
            return v1._raw < (v2 << ShiftBy);
        }

        public static bool operator < (int v1, FPInt v2)
        {
            return (v1 << ShiftBy) < v2._raw;
        }

        #endregion

        #region Bit Operators

        public static FPInt operator << (FPInt v, int amount)
        {
            return new FPInt() { _raw = v._raw << amount };
        }

        public static FPInt operator >> (FPInt v, int amount)
        {
            return new FPInt() { _raw = v._raw >> amount };
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator FPInt (byte src)
        {
            return new FPInt() { _raw = (int)src << ShiftBy };
        }

        public static implicit operator FPInt (short src)
        {
            return new FPInt() { _raw = (int)src << ShiftBy };
        }

        #endregion

        #region Explicit Conversions

        public static explicit operator int (FPInt src)
        {
            return (int)(src._raw >> ShiftBy);
        }

        public static explicit operator float (FPInt src)
        {
            return (float)src._raw / (float)OneL;
        }

        public static explicit operator double (FPInt src)
        {
            return (double)src._raw / (double)OneL;
        }

        public static explicit operator FPInt (int src)
        {
            return new FPInt() { _raw = src << ShiftBy };
        }

        public static explicit operator FPInt (long src)
        {
            return new FPInt() { _raw = (int)src << ShiftBy };
        }

        public static explicit operator FPInt (float src)
        {
            return new FPInt() { _raw = (int)(src * OneL) };
        }

        public static explicit operator FPInt (double src)
        {
            return new FPInt() { _raw = (int)(src * OneL) };
        }

        #endregion

        #region Object Overrides

        public override bool Equals (object obj)
        {
            if (obj is FPInt) {
                return ((FPInt)obj)._raw == this._raw;
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
            return (_raw >> ShiftBy) + "+" + ((float)(_raw & SubMask) / OneL);
        }

        #endregion
    }
}
