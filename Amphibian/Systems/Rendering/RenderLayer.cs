using System;
using System.Collections.Generic;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering
{
    public abstract class RenderLayer
    {
        public abstract IRenderManager RenderManager { get; }

        public abstract void Begin (Matrix transformMatrix);
        public abstract void End ();
    }

    public static class RenderLayerFactory
    {
        private static Dictionary<Type, Func<EntityWorld, RenderLayer>> _registry = new Dictionary<Type, Func<EntityWorld, RenderLayer>>();

        public static void Register (Type type, Func<EntityWorld, RenderLayer> createFunc)
        {
            _registry[type] = createFunc;
        }

        public static void Register<T> (Func<EntityWorld, RenderLayer> createFunc)
            where T : class, IRenderManager
        {
            Register(typeof(T), createFunc);
        }

        public static RenderLayer Create (Type type, EntityWorld world)
        {
            Func<EntityWorld, RenderLayer> func;
            if (_registry.TryGetValue(type, out func))
                return func(world);

            return null;
        }

        public static T Create<T> (EntityWorld world)
            where T : class, IRenderManager
        {
            return Create(typeof(T), world) as T;
        }

        static RenderLayerFactory ()
        {
            Register<SpriteRenderManager>(w => new SpriteRenderLayer(w));
            Register<DrawRenderManager>(w => new DrawRenderLayer(w));
            Register<SpineRenderManager>(w => new SpineRenderLayer(w));
        }
    }
}
