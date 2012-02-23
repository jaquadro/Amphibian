using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Collision;
using Amphibian.EntitySystem;

namespace Amphibian.Components
{
    public sealed class Collidable : IComponent
    {
        public Collidable (Mask mask)
        {
            Mask = mask;
        }

        public Mask Mask { get; set; }
        public bool BackgroundObstacle { get; set; }
    }
}
