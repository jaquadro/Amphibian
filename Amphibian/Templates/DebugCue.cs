using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Systems.Rendering;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Components;
using Microsoft.Xna.Framework;
using Amphibian.Geometry;
using Amphibian.Systems;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Templates
{
    public class DebugCueSpatial : SpriteSpatial
    {
        private float _radius;
        private float _opacity;
        //private Pen _pen;

        public DebugCueSpatial (EntityWorld world)
            : base(world)
        {
            _radius = 1f;
            _opacity = 1f;
            //_pen = Pens.White;
        }

        public override void Initialize (ContentManager contentManager)
        {
        }

        public override void Update ()
        {
            _radius += 1f;
            if (_opacity > 0)
                _opacity -= 0.05f;
        }

        public override void Render (SpriteBatch spriteBatch, Entity entity, Renderable position)
        {
            //Rectangle area = new Rectangle((int)position.RenderX - (int)_radius, (int)position.RenderY - (int)_radius, (int)_radius * 2, (int)_radius * 2);
            //Drawing.Draw2D.DrawRectangle(spriteBatch, area, _pen);
        }
    }

    public class DebugCue
    {
        public static Entity Create (Engine engine, SystemManager sysManager, PointFP location)
        {
            RenderSystem renderSys = sysManager.GetSystem(typeof(RenderSystem)) as RenderSystem;
            if (renderSys == null)
                return new Entity();

            Entity ent = sysManager.World.EntityManager.Create();

            Amphibian.Systems.Rendering.Spatial spat = new DebugCueSpatial(sysManager.World);
            spat.Initialize(engine.Content);
            Amphibian.Systems.Rendering.SpatialRef potSR = renderSys.SpatialManager.Add(spat);

            Renderable entRC = new Renderable();
            entRC.SpatialRef = potSR;
            entRC.RenderX = location.X;
            entRC.RenderY = location.Y;

            RemovalTimeout entRT = new RemovalTimeout(0.5f);

            sysManager.World.EntityManager.AddComponent(ent, entRC);
            sysManager.World.EntityManager.AddComponent(ent, entRT);

            return ent;
        }
    }
}
