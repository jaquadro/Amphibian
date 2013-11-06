using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;

namespace Amphibian
{
    public class EntityDebugger
    {
        public List<EntityDebug> DebugEntities = new List<EntityDebug>();

        public EntityDebugger(EntityFrame frame)
        {
            foreach (Entity e in frame.EntityWorld.EntityManager._active)
            {
                if (e.Id != 0)
                    DebugEntities.Add(new EntityDebug(e,frame.EntityWorld.EntityManager));
            }
        }
    }

    public class EntityDebug
    {

        String Components="";
        Dictionary<String, Object> Values = new Dictionary<string,object>();

        public EntityDebug(Entity e, EntityManager entityManager)
        {
           Utility.UnorderedList<IComponent> components = entityManager._componentsByEntity[e.Index];
           if (components == null) { Components = "NULL"; return; }
           foreach (IComponent comp in components)
           {
               Components += "<" + comp.GetType().Name + ">";
               Values.Add(comp.GetType().Name, comp);
           }
        }

    }
}
