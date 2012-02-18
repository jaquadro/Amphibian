using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;

namespace Amphibian.Components
{
    public sealed class RemovalTimeout : IComponent
    {
        public RemovalTimeout (float time)
        {
            TimeRemaining = time;
        }

        public float TimeRemaining { get; set; }
    }
}
