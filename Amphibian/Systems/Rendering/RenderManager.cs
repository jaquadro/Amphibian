using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering
{
    public interface IRenderManager
    {
        void Begin ();
        void Begin (IRenderManagerOptions renderOptions);
        void Begin (IRenderManagerOptions renderOptions, Matrix transformMatrix);
        void End ();
    }

    public interface IRenderManagerOptions
    {
    }
}
