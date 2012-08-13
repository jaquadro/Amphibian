using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering
{
    public abstract class RenderLayer
    {
        public abstract IRenderManager RenderManager { get; }

        public abstract void Begin (Matrix transformMatrix);
        public abstract void End ();
    }
}
