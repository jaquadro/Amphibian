using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Components;
using Amphibian.Systems.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Treefrog.Runtime;

namespace Amphibian.Systems
{

    public class RenderSystem : ProcessingSystem
    {
        private SpriteBatch _spriteBatch;
        private SpatialManager _manager;

        private CameraSystem _cameraSystem;

        private Level _level;

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

            LevelIndex index = SystemManager.World.Frame.Engine.Content.Load<LevelIndex>("pcaves");
            _level = SystemManager.World.Frame.Engine.Content.Load<Level>(index.ByName("Level 1").Asset);
            _level.ScaleX = 2f;
            _level.ScaleY = 2f;
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
            if (_cameraSystem != null)
                _level.Draw(_spriteBatch, _cameraSystem.Bounds);

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
            //Position positionCom = null;

            foreach (IComponent com in EntityManager.GetComponents(entity)) {
                if (com is Renderable)
                    renderCom = com as Renderable;
                //if (com is Position)
                //    positionCom = com as Position;
            }

            if (renderCom == null)
                return;

            if (!_manager.IsValid(renderCom.SpatialRef))
                return;

            Spatial sp = _manager.GetSpatial(renderCom.SpatialRef);
            if (sp == null)
                return;

            //if (positionCom != null) {
            //    renderCom.RenderX = positionCom.X;
            //    renderCom.RenderY = positionCom.Y;
            //}

            sp.Render(_spriteBatch, renderCom);
        }

        private void SystemManager_SystemAdded (BaseSystem system)
        {
            if (system is CameraSystem) {
                _cameraSystem = system as CameraSystem;
            }
        }
    }

    public class RenderTransformSystem : ProcessingSystem
    {
        private CameraSystem _cameraSystem;

        public RenderTransformSystem ()
            : base(typeof(Renderable))
        {
        }

        protected internal override void Initialize ()
        {
            _cameraSystem = SystemManager.GetSystem(typeof(CameraSystem)) as CameraSystem;

            SystemManager.SystemAdded += SystemManager_SystemAdded;
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity e in entities) {
                Process(e);
            }
        }

        private void Process (Entity entity)
        {
            Renderable renderCom = EntityManager.GetComponent(entity, typeof(Renderable)) as Renderable;
            if (renderCom == null)
                return;

            Position positionCom = EntityManager.GetComponent(entity, typeof(Position)) as Position;
            if (positionCom != null) {
                renderCom.RenderX = positionCom.X;
                renderCom.RenderY = positionCom.Y;
            }

            ParallaxCom parallaxCom = EntityManager.GetComponent(entity, typeof(ParallaxCom)) as ParallaxCom;
            if (parallaxCom != null) {
                renderCom.RenderX += _cameraSystem.Left * parallaxCom.ScrollCoefX;
                renderCom.RenderY += _cameraSystem.Top * parallaxCom.ScrollCoefY;
            }
        }

        private void SystemManager_SystemAdded (BaseSystem system)
        {
            if (system is CameraSystem) {
                _cameraSystem = system as CameraSystem;
            }
        }
    }
}
