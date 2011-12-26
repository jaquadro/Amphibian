using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    public struct ComponentType
    {
        internal int Index;

        internal ComponentType (int index)
        {
            Index = index;
        }
    }
}
