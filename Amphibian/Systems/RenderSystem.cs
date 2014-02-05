using System;
using System.Collections.Generic;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Amphibian.Systems.Rendering;
using Amphibian.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems
{
    public class RenderSystem : ProcessingSystem<Renderable>
    {
        private struct EntityRenderRecord
        {
            public Entity Entity;
            public Renderable RenderCom;
            public Spatial Spatial;

            public EntityRenderRecord (Entity entity, Renderable renderCom, Spatial spatial)
            {
                Entity = entity;
                RenderCom = renderCom;
                Spatial = spatial;
            }
        }

        private SpatialManager _manager;

        private List<int> _layerIndexes;

        private Dictionary<int, Dictionary<Type, RenderLayer>> _layers;
        private Dictionary<int, Dictionary<Type, UnorderedList<EntityRenderRecord>>> _separator;

        public RenderSystem ()
        {
            _manager = new SpatialManager();
            _separator = new Dictionary<int, Dictionary<Type, UnorderedList<EntityRenderRecord>>>();

            _layerIndexes = new List<int>();
            _layers = new Dictionary<int, Dictionary<Type, RenderLayer>>();

            AutoCreateLayers = true;
        }

        public RenderSystem (EntityWorld world)
            : this()
        {
            SetLayer(0, new SpriteRenderLayer(world));
        }

        public RenderSystem (EntityWorld world, SpriteBatch spriteBatch)
            : this()
        {
            SetLayer(0, new SpriteRenderLayer(world, spriteBatch));
        }

        [RequiredSystem]
        protected CameraSystem CameraSystem { get; set; }

        public bool AutoCreateLayers { get; set; }

        public void SetLayer (int index, RenderLayer layer)
        {
            Dictionary<Type, RenderLayer> layerSet;
            if (!_layers.TryGetValue(index, out layerSet))
                _layers.Add(index, layerSet = new Dictionary<Type, RenderLayer>());

            Type rmType = layer.RenderManager.GetType();
            layerSet[rmType] = layer;

            if (!_layerIndexes.Contains(index)) {
                _layerIndexes.Add(index);
                _layerIndexes.Sort();
            }
        }

        public void ClearLayer (int index)
        {
            _layers.Remove(index);
        }

        protected internal override void Initialize ()
        {
            EntityManager.RegisterComponentRemovedHandler<Renderable>((entity, renderCom) => {
                _manager.Remove(renderCom.SpatialRef);
            });
        }

        public SpatialManager SpatialManager
        {
            get { return _manager; }
        }

        protected override void Begin ()
        {
            Matrix matrix = CameraSystem != null 
                ? CameraSystem.GetTranslationMatrix()
                : Matrix.Identity;

            foreach (var recordSet in _separator.Values) {
                foreach (var record in recordSet.Values)
                    record.Clear();
            }
        }

        protected override void End ()
        {
            Matrix matrix = CameraSystem.GetTranslationMatrix();

            if (AutoCreateLayers)
                CreateMissingLayers();

            foreach (int layerIndex in _layerIndexes) {
                Dictionary<Type, RenderLayer> layerSet = _layers[layerIndex];
                Dictionary<Type, UnorderedList<EntityRenderRecord>> separatorSet;
                if (!_separator.TryGetValue(layerIndex, out separatorSet))
                    continue;

                foreach (var kv in layerSet) {
                    RenderLayer renderLayer = kv.Value;

                    UnorderedList<EntityRenderRecord> records;
                    if (!separatorSet.TryGetValue(kv.Key, out records))
                        continue;

                    renderLayer.Begin(matrix);
                    BeginRenderLayer(layerIndex);

                    foreach (EntityRenderRecord record in records) {
                        Spatial sp = _manager.GetSpatial(record.RenderCom.SpatialRef);
                        if (sp == null)
                            return;

                        if (sp.InBounds(CameraSystem.Bounds))
                            sp.Render(renderLayer.RenderManager, record.Entity, record.RenderCom);
                    }

                    EndRenderLayer(layerIndex);
                    renderLayer.End();
                }
            }
        }

        private void CreateMissingLayers ()
        {
            foreach (int layerIndex in _separator.Keys) {
                Dictionary<Type, UnorderedList<EntityRenderRecord>> separatorSet = _separator[layerIndex];
                Dictionary<Type, RenderLayer> layerSet;
                if (!_layers.TryGetValue(layerIndex, out layerSet))
                    _layers.Add(layerIndex, layerSet = new Dictionary<Type, RenderLayer>());

                foreach (var kv in separatorSet) {
                    if (kv.Value.Count == 0)
                        continue;

                    if (!layerSet.ContainsKey(kv.Key)) {
                        RenderLayer renderLayer = RenderLayerFactory.Create(kv.Key, World);
                        if (renderLayer != null)
                            layerSet[kv.Key] = renderLayer;
                    }
                }

                if (layerSet.Count > 0 && !_layerIndexes.Contains(layerIndex)) {
                    _layerIndexes.Add(layerIndex);
                    _layerIndexes.Sort();
                }
            }
        }

        protected virtual void BeginRenderLayer (int layerIndex)
        { }

        protected virtual void EndRenderLayer (int layerIndex)
        { }

        protected override void Process (Entity entity, Renderable renderCom)
        {
            if (!_manager.IsValid(renderCom.SpatialRef))
                return;

            Dictionary<Type, UnorderedList<EntityRenderRecord>> rmSet;
            if (!_separator.TryGetValue(renderCom.LayerIndex, out rmSet))
                _separator.Add(renderCom.LayerIndex, rmSet = new Dictionary<Type, UnorderedList<EntityRenderRecord>>());

            Spatial spatial = _manager.GetSpatial(renderCom.SpatialRef);

            UnorderedList<EntityRenderRecord> recordList;
            if (!rmSet.TryGetValue(spatial.RenderManagerType, out recordList))
                rmSet.Add(spatial.RenderManagerType, recordList = new UnorderedList<EntityRenderRecord>());

            recordList.Add(new EntityRenderRecord(entity, renderCom, spatial));
        }
    }
}
