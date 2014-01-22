using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering
{
    public class SpriteRenderLayer : RenderLayer
    {
        private SpriteRenderManager _manager;
        private SpriteRenderManagerOptions _options;

        public SpriteRenderLayer (SpriteBatch spriteBatch)
        {
            _manager = new SpriteRenderManager(spriteBatch);

            _options = new SpriteRenderManagerOptions() {
                SamplerState = SamplerState.PointClamp,
            };
        }

        public override IRenderManager RenderManager
        {
            get { return _manager; }
        }

        public SpriteRenderManagerOptions RenderOptions
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
