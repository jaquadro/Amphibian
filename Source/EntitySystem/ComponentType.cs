using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    public struct ComponentType : IEquatable<ComponentType>
    {
        internal int Index;

        internal ComponentType (int index)
        {
            Index = index;
        }

        public bool Equals(ComponentType other)
        {
            return Index == other.Index;
        }
    }
}
