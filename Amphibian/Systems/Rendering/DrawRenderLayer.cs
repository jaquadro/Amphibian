using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LilyPath;

namespace Amphibian.Systems.Rendering
{
    public class DrawRenderLayer : RenderLayer
    {
        private DrawRenderManager _manager;
        private DrawRenderManagerOptions _options;

        public DrawRenderLayer (DrawBatch drawBatch)
        {
            _manager = new DrawRenderManager(drawBatch);

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
