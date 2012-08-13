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

        private SpatialManager _manager;
        private CameraSystem _cameraSystem;

        private List<int> _layerIndexes;
        private Dictionary<int, RenderLayer> _layers;

        private Dictionary<int, UnorderedList<EntityRenderRecord>> _separator;

        public RenderSystem ()
            : base(typeof(Renderable))
        {
            _manager = new SpatialManager();
            _separator = new Dictionary<int, UnorderedList<EntityRenderRecord>>();

            _layerIndexes = new List<int>();
            _layers = new Dictionary<int, RenderLayer>();
        }

        public RenderSystem (SpriteBatch spriteBatch)
            : this()
        {
            SetLayer(0, new SpriteRenderLayer(spriteBatch));
        }

        public void SetLayer (int index, RenderLayer layer)
        {
            _layers[index] = layer;

            if (!_layerIndexes.Contains(index)) {
                _layerIndexes.Add(index);
                _layerIndexes.Sort();
            }
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

            foreach (UnorderedList<EntityRenderRecord> record in _separator.Values)
                record.Clear();
        }

        protected override void End ()
        {
            if (_cameraSystem != null) {
                Matrix matrix = _cameraSystem.GetTranslationMatrix();

                foreach (int layerIndex in _layerIndexes) {
                    RenderLayer renderLayer = _layers[layerIndex];
                    renderLayer.Begin(matrix);
                    BeginRenderLayer(layerIndex);

                    if (_separator.ContainsKey(layerIndex)) {
                        foreach (EntityRenderRecord record in _separator[layerIndex]) {
                            Spatial sp = _manager.GetSpatial(record.RenderCom.SpatialRef);
                            if (sp == null)
                                return;

                            if (sp.InBounds(_cameraSystem.Bounds))
                                sp.Render(renderLayer.RenderManager, record.Entity, record.RenderCom);
                        }
                    }

                    EndRenderLayer(layerIndex);
                    renderLayer.End();
                }
            }
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
