using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLib;

namespace Amphibian
{
    public class Tileset : Component
    {
        private Map _map;
        private List<TileLayer> _layers;

        public Tileset ()
            : base()
        {
            _layers = new List<TileLayer>();
        }

        protected override void Load ()
        {
            _map = Parent.Engine.Content.Load<Map>("testlevel");

            foreach (Layer layer in _map.Layers) {
                if (layer is TileLayer) {
                    TileLayer tileLayer = layer as TileLayer;
                    tileLayer.ScaleX = 2f;
                    tileLayer.ScaleY = 2f;

                    _layers.Add(tileLayer);
                }
            }
        }

        public override void Draw ()
        {
            base.Draw();

            foreach (TileLayer tl in _layers) {
                _map.Draw(Parent.Engine.SpriteBatch, Parent.Camera.Bounds, tl);
            }
        }
    }
}
