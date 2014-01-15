using System;
using Amphibian.Components;
using Amphibian.EntitySystem;
using LilyPath;

namespace Amphibian.Systems.Rendering
{
    public class DrawCommandSpatial : DrawSpatial
    {
        public DrawCommandSpatial ()
        { }

        public DrawCommandSpatial (Action<DrawBatch, Renderable> command)
        {
            Command = command;
        }

        public Action<DrawBatch, Renderable> Command { get; set; }

        public override void Render (DrawBatch drawBatch, EntityWorld world, Entity entity, Renderable position)
        {
            if (Command != null)
                Command(drawBatch, position);
        }
    }
}
