using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    public struct Entity : IEquatable<Entity>
    {
        internal readonly int Id;
        internal readonly int Index;

        internal Entity (int id, int index)
        {
            Id = id;
            Index = index;
        }

        public bool Equals (Entity other)
        {
            return Id == other.Id && Index == other.Index;
        }

        public override string ToString ()
        {
            return "Entity [" + Id + "]";
        }
    }
}
