using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Systems.Particle
{
    public struct Range
    {
        private readonly float _min;
        private readonly float _max;
        private readonly Distribution _distribution;

        public static Range Zero
        {
            get { return new Range(0); }
        }

        public Range (float value)
            : this(value, value, null)
        { }

        public Range (float min, float max)
            : this(min, max, Distribution.Uniform)
        { }

        public Range (float min, float max, Distribution distribution)
        {
            _min = min;
            _max = max;
            _distribution = distribution;

            if (!IsFixedValue && distribution == null)
                throw new ArgumentNullException("distribution", "A distribution must be supplied for ranged values.");
        }

        public float Min
        {
            get { return _min; }
        }

        public float Max
        {
            get { return _max; }
        }

        public Distribution Distribution
        {
            get { return _distribution; }
        }

        public bool IsFixedValue
        {
            get { return _min == _max; }
        }

        public bool IsRangedValue
        {
            get { return _min != _max; }
        }

        public float Sample ()
        {
            if (IsFixedValue)
                return _min;
            else
                return _min + _distribution.Sample() * (_max - _min);
        }

        public static implicit operator Range (float value)
        {
            return new Range(value);
        }
    }
}
