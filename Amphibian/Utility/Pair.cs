using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Utility
{
    public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
    {
        private readonly T1 _v1;
        private readonly T2 _v2;

        public Pair (T1 value1, T2 value2)
        {
            _v1 = value1;
            _v2 = value2;
        }

        public T1 Value1
        {
            get { return _v1; }
        }

        public T2 Value2
        {
            get { return _v2; }
        }

        #region Comparison Operators

        public static bool operator == (Pair<T1, T2> v1, Pair<T1, T2> v2)
        {
            return v1._v1.Equals(v2._v1) && v1._v2.Equals(v2._v2);
        }

        public static bool operator != (Pair<T1, T2> v1, Pair<T1, T2> v2)
        {
            return !v1._v1.Equals(v2._v1) || !v1._v2.Equals(v2._v2);
        }

        #endregion

        #region Overrides

        public override bool Equals (object obj)
        {
            if (obj is Pair<T1, T2>) {
                return (Pair<T1, T2>)obj == this;
            }
            return false;
        }

        public bool Equals (Pair<T1, T2> other)
        {
            return other == this;
        }

        public override int GetHashCode ()
        {
            int hash = _v1.GetHashCode() * 37;
            return hash ^ _v2.GetHashCode();
        }

        public override string ToString ()
        {
            return "{" + _v1 + ", " + _v2 + "}";
        }

        #endregion
    }
}
