using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Components;
using Amphibian.Systems.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems
{

    public class RenderSystem : BaseSystem
    {
        private SpriteBatch _spriteBatch;
        private SpatialManager _manager;

        private CameraSystem _cameraSystem;

        public RenderSystem (SpriteBatch spriteBatch)
            : base(typeof(Renderable))
        {
            _manager = new SpatialManager();
            _spriteBatch = spriteBatch;
        }

        protected internal override void Initialize ()
        {
            _cameraSystem = SystemManager.GetSystem(typeof(CameraSystem)) as CameraSystem;

            SystemManager.SystemAdded += SystemManager_SystemAdded;
        }

        public SpatialManager SpatialManager
        {
            get { return _manager; }
        }

        protected override void Begin ()
        {
            Matrix matrix = _cameraSystem != null 
                ? _cameraSystem.GetTranslationMatrix()
                : Matrix.Identity;

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, matrix);
            _spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        }

        protected override void End ()
        {
            _spriteBatch.End();
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities) {
                Process(entity);
            }
        }

        private void Process (Entity entity)
        {
            Renderable renderCom = null;
            Position positionCom = null;

            foreach (IComponent com in EntityManager.GetComponents(entity)) {
                if (com is Renderable)
                    renderCom = com as Renderable;
                if (com is Position)
                    positionCom = com as Position;
            }

            if (renderCom == null)
                return;

            if (!_manager.IsValid(renderCom.SpatialRef))
                return;

            Spatial sp = _manager.GetSpatial(renderCom.SpatialRef);
            if (sp == null)
                return;

            if (positionCom != null) {
                renderCom.RenderX = positionCom.X;
                renderCom.RenderY = positionCom.Y;
            }

            sp.Render(_spriteBatch, renderCom);
        }

        private void SystemManager_SystemAdded (BaseSystem system)
        {
            if (system is CameraSystem) {
                _cameraSystem = system as CameraSystem;
            }
        }
    }
}
