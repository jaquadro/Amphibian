using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LilyPath;
using Amphibian.EntitySystem;

namespace Amphibian.Systems.Rendering
{
    public class DrawRenderLayer : RenderLayer
    {
        private DrawRenderManager _manager;
        private DrawRenderManagerOptions _options;

        public DrawRenderLayer (EntityWorld world)
        {
            _manager = new DrawRenderManager(world);

            InitOptions();
        }

        public DrawRenderLayer (EntityWorld world, DrawBatch drawBatch)
        {
            _manager = new DrawRenderManager(world, drawBatch);

            InitOptions();
        }

        private void InitOptions ()
        {
            _options = new DrawRenderManagerOptions() {
                SamplerState = SamplerState.PointClamp,
            };
        }

        public override IRenderManager RenderManager
        {
            get { return _manager; }
        }

        public DrawRenderManagerOptions RenderOptions
        {
            get { return _options; }
        }

        public override void Begin (Matrix transformMatrix)
        {
            _manager.Begin(_options, transformMatrix);
        }

        public override void End ()
        {
            _manager.End();
        }
    }
}
