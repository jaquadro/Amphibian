using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites
{
    public abstract class StaticSpriteElement
    {
        public IStaticSpriteInstance Instance { get; set; }

        public ISpriteListElement Sprites { get; set; }

        public StaticSpriteDefinition BuildDefinition (ContentManager contentManager)
        {
            StaticSpriteDefinition definition = new StaticSpriteDefinition();

            ISpriteElement sprite = Sprites.Sprites[0];
            definition.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
            definition.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

            return definition;
        }
    }

    public interface IStaticSpriteInstance
    {
        ITransformElement Transform { get; set; }
    }
}
