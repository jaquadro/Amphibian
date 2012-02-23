using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;

namespace Amphibian.Components
{
    public sealed class Animation : IComponent
    {
        public int Index;
        public int LoopCount;
    }
}
