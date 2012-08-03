using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Utility
{
    public struct IntPair : IEquatable<IntPair>
    {
        private readonly int _v1;
        private readonly int _v2;

        public IntPair (int value1, int value2)
        {
            _v1 = value1;
            _v2 = value2;
        }

        public int Value1
        {
            get { return _v1; }
        }

        public int Value2
        {
            get { return _v2; }
        }

        #region Comparison Operators

        public static bool operator == (IntPair v1, IntPair v2)
        {
            return (v1._v1 == v2._v1) && (v1._v2 == v2._v2);
        }

        public static bool operator != (IntPair v1, IntPair v2)
        {
            return (v1._v1 != v2._v1) || (v1._v2 != v2._v2);
        }

        #endregion

        #region Overrides

        public override bool Equals (object obj)
        {
            if (obj is IntPair) {
                return (IntPair)obj == this;
            }
            return false;
        }

        public bool Equals (IntPair other)
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
            return "(" + _v1 + ", " + _v2 + ")";
        }

        #endregion
    }
}
