using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    public abstract class TagSystem : BaseSystem
    {
        private string _tag;

        public TagSystem (string tag)
        {
            _tag = tag;
        }

        public abstract void Process (Entity entity);

        protected override void ProcessInner()
        {
            Entity e = SystemManager.World.TagManager.GetEntity(_tag);
            if (EntityManager.IsValid(e)) {
                Process(e);
            }
        }
    }
}
