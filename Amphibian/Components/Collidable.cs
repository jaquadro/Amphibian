using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Collision;
using Amphibian.EntitySystem;

namespace Amphibian.Components
{
    public sealed class Collidable : IComponent, ICollidable
    {
        public Collidable (Mask mask)
        {
            CollisionMask = mask;
        }

        public Mask CollisionMask { get; set; }
        public bool BackgroundObstacle { get; set; }
    }
}
