using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TiledLib;
using Treefrog.Runtime;
using Amphibian.Systems;

namespace Amphibian
{
    public class Tileset : Component
    {
        //private Map _map;
        private Level _level;
        private List<TileLayer> _layers;

        public Tileset ()
            : base()
        {
            _layers = new List<TileLayer>();
        }

        protected override void Load ()
        {
            //_map = Parent.Engine.Content.Load<Map>("purple_caves_lev");
            LevelIndex index = Parent.Engine.Content.Load<LevelIndex>("pcaves");
            _level = Parent.Engine.Content.Load<Level>(index.ByName("Level 1").Asset);
            _level.ScaleX = 2f;
            _level.ScaleY = 2f;

            //foreach (Layer layer in _map.Layers) {
            foreach (Layer layer in _level.Layers) {
                if (layer is TileLayer) {
                    TileLayer tileLayer = layer as TileLayer;
                    //tileLayer.ScaleX = 2f;
                    //tileLayer.ScaleY = 2f;

                    _layers.Add(tileLayer);
                }
            }
        }

        public override void Draw ()
        {
            base.Draw();

            //foreach (TileLayer tl in _layers) {
                //_map.Draw(Parent.Engine.SpriteBatch, Parent.Camera.Bounds, tl);
            //}

            CameraSystem camera = Parent.EntityWorld.SystemManager.GetSystem(typeof(CameraSystem)) as CameraSystem;
            if (camera == null)
                return;

            _level.Draw(Parent.Engine.SpriteBatch, camera.Bounds);
        }
    }
}
