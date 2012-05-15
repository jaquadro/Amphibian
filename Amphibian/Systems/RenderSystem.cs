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
using Amphibian.Utility;

namespace Amphibian.Systems
{

    public class RenderSystem : ProcessingSystem
    {
        private struct EntityRenderRecord
        {
            public Entity Entity;
            public Renderable RenderCom;

            public EntityRenderRecord (Entity entity, Renderable renderCom)
            {
                Entity = entity;
                RenderCom = renderCom;
            }
        }

        private SpriteBatch _spriteBatch;
        private SpatialManager _manager;

        private CameraSystem _cameraSystem;

        private Level _level;

        private Dictionary<int, UnorderedList<EntityRenderRecord>> _separator;

        public RenderSystem (SpriteBatch spriteBatch)
            : base(typeof(Renderable))
        {
            _manager = new SpatialManager();
            _spriteBatch = spriteBatch;
            _separator = new Dictionary<int, UnorderedList<EntityRenderRecord>>();
        }

        protected internal override void Initialize ()
        {
            _cameraSystem = SystemManager.GetSystem(typeof(CameraSystem)) as CameraSystem;

            SystemManager.SystemAdded += SystemManager_SystemAdded;

            LevelIndex index = SystemManager.World.Frame.Engine.Content.Load<LevelIndex>("proto3");
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

            foreach (UnorderedList<EntityRenderRecord> record in _separator.Values)
                record.Clear();
        }

        protected override void End ()
        {
            if (_cameraSystem != null) {
                foreach (Layer layer in _level.Layers) {
                    layer.Draw(_spriteBatch, _cameraSystem.Bounds);

                    if (_separator.ContainsKey(layer.Id)) {
                        foreach (EntityRenderRecord record in _separator[layer.Id]) {
                            Spatial sp = _manager.GetSpatial(record.RenderCom.SpatialRef);
                            if (sp == null)
                                return;

                            sp.Render(_spriteBatch, record.Entity, record.RenderCom);
                        }
                    }
                }
            }
                //_level.Draw(_spriteBatch, _cameraSystem.Bounds);

            _spriteBatch.End();
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities) {
                //Process(entity);
                Separate(entity);
            }
        }

        private void Separate (Entity entity)
        {
            Renderable renderCom = null;
            foreach (IComponent com in EntityManager.GetComponents(entity)) {
                if (com is Renderable) {
                    renderCom = com as Renderable;
                    break;
                }
            }

            if (renderCom == null)
                return;

            if (!_manager.IsValid(renderCom.SpatialRef))
                return;

            if (!_separator.ContainsKey(renderCom.LayerIndex))
                _separator.Add(renderCom.LayerIndex, new UnorderedList<EntityRenderRecord>());

            _separator[renderCom.LayerIndex].Add(new EntityRenderRecord(entity, renderCom));
        }

        /*private void Process (Entity entity)
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

            sp.Render(_spriteBatch, entity, renderCom);
        }*/

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
