using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites
{
    public abstract class CounterElement
    {
        public ICounterInstance Instance { get; set; }

        public ISpriteListElement Sprites { get; set; }

        public CounterDefinition BuildDefinition (ContentManager contentManager)
        {
            CounterDefinition definition = new CounterDefinition();

            foreach (ISpriteElement sprite in Sprites.Sprites) {
                if (!CounterDefinition.DigitIndex.ContainsKey(sprite.Name))
                    continue;

                StaticSpriteDefinition spriteDef = new StaticSpriteDefinition();
                spriteDef.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
                spriteDef.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

                definition.DigitFrames[CounterDefinition.DigitIndex[sprite.Name]] = spriteDef;
            }

            return definition;
        }
    }

    public interface ICounterInstance
    {
        ITransformElement Transform { get; set; }
    }
}
