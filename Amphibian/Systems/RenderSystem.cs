using System.Collections.Generic;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Amphibian.Systems.Rendering;
using Amphibian.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private SamplerState _samplerState;

        private CameraSystem _cameraSystem;

        private List<int> _layerIndexes;

        private Dictionary<int, UnorderedList<EntityRenderRecord>> _separator;

        public RenderSystem (SpriteBatch spriteBatch)
            : base(typeof(Renderable))
        {
            _manager = new SpatialManager();
            _spriteBatch = spriteBatch;
            _samplerState = SamplerState.PointClamp;
            _separator = new Dictionary<int, UnorderedList<EntityRenderRecord>>();

            _layerIndexes = new List<int>();
            _layerIndexes.Add(0);
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

        public IList<int> LayerIndexes
        {
            get { return _layerIndexes; }
        }

        public SamplerState SamplerState
        {
            get { return _samplerState; }
            set { _samplerState = value; }
        }

        protected override void Begin ()
        {
            Matrix matrix = _cameraSystem != null 
                ? _cameraSystem.GetTranslationMatrix()
                : Matrix.Identity;

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, _samplerState, null, null, null, matrix);
            _spriteBatch.GraphicsDevice.SamplerStates[0] = _samplerState;

            foreach (UnorderedList<EntityRenderRecord> record in _separator.Values)
                record.Clear();
        }

        protected override void End ()
        {
            if (_cameraSystem != null) {
                foreach (int layerIndex in _layerIndexes) {
                    BeginRenderLayer(layerIndex);

                    if (_separator.ContainsKey(layerIndex)) {
                        foreach (EntityRenderRecord record in _separator[layerIndex]) {
                            Spatial sp = _manager.GetSpatial(record.RenderCom.SpatialRef);
                            if (sp == null)
                                return;

                            if (sp.InBounds(_cameraSystem.Bounds))
                                sp.Render(_spriteBatch, record.Entity, record.RenderCom);
                        }
                    }

                    EndRenderLayer(layerIndex);
                }
            }

            _spriteBatch.End();
        }

        protected virtual void BeginRenderLayer (int layerIndex)
        { }

        protected virtual void EndRenderLayer (int layerIndex)
        { }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities) {
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

        private void SystemManager_SystemAdded (BaseSystem system)
        {
            if (system is CameraSystem) {
                _cameraSystem = system as CameraSystem;
            }
        }
    }
}
