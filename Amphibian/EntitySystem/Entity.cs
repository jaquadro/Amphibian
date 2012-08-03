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

        public static Entity None
        {
            get { return new Entity(0, 0); }
        }

        public static bool operator == (Entity left, Entity right)
        {
            return left.Id == right.Id && left.Index == right.Index;
        }

        public static bool operator != (Entity left, Entity right)
        {
            return left.Id != right.Id || left.Index != right.Index;
        }

        public static bool operator < (Entity left, Entity right)
        {
            return left.Id < right.Id || (left.Id == right.Id && left.Index < right.Index);
        }

        public static bool operator > (Entity left, Entity right)
        {
            return left.Id > right.Id || (left.Id == right.Id && left.Index > right.Index);
        }

        public override bool Equals (object obj)
        {
            return Equals((Entity)obj);
        }

        public override string ToString ()
        {
            return "Entity [" + Id + "]";
        }

        public override int GetHashCode ()
        {
            int hash = 23;
            hash = hash * 37 + Id;
            hash = hash * 37 + Index;
            return hash;
        }
    }
}
