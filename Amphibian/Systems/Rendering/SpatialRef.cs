using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Systems.Rendering
{
    public struct SpatialRef
    {
        internal readonly int Id;
        internal readonly int Index;

        internal SpatialRef (int id, int index)
        {
            Id = id;
            Index = index;
        }

        public override string ToString ()
        {
            return "SpatialRef[" + Id + "]";
        }
    }
}
