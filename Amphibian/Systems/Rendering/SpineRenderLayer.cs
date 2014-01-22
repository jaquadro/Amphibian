using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering
{
    public class SpineRenderLayer : RenderLayer
    {
        private SpineRenderManager _manager;
        private SpineRenderManagerOptions _options;

        public SpineRenderLayer (GraphicsDevice device)
        {
            _manager = new SpineRenderManager(device);

            _options = new SpineRenderManagerOptions();
        }

        public override IRenderManager RenderManager
        {
            get { return _manager; }
        }

        public SpineRenderManagerOptions RenderOptions
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
