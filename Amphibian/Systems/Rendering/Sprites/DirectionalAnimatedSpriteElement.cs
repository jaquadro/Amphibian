using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class DirectionalAnimatedSpriteElement
    {
        public IDirectionalAnimatedSpriteInstance Instance { get; set; }

        public ISpriteListElement Sprites { get; set; }

        public IList<IDirectionElement> Directions { get; set; }

        public DirectionalAnimatedSpriteDefinition BuildDefinition (ContentManager contentManager)
        {
            DirectionalAnimatedSpriteDefinition definition = new DirectionalAnimatedSpriteDefinition();

            Dictionary<String, StaticSpriteDefinition> spriteDefs = new Dictionary<string, StaticSpriteDefinition>();
            foreach (ISpriteElement sprite in Sprites.Sprites) {
                StaticSpriteDefinition spriteDef = new StaticSpriteDefinition();
                spriteDef.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
                spriteDef.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

                spriteDefs[sprite.Name] = spriteDef;
            }

            foreach (IDirectionElement direction in Directions) {
                AnimatedSpriteDefinition animDef = new AnimatedSpriteDefinition();
                foreach (IFrameElement frame in direction.Animation.Frames) {
                    if (spriteDefs.ContainsKey(frame.Sprite))
                        animDef.AddSprite(spriteDefs[frame.Sprite], frame.Duration);
                }

                if (direction.Animation.Repeat < 0)
                    animDef.RepeatIndefinitely = true;
                else
                    animDef.RepeatLimit = direction.Animation.Repeat;

                definition[(Direction)Enum.Parse(typeof(Direction), direction.Value)] = animDef;
            }

            return definition;
        }
    }

    public interface IDirectionalAnimatedSpriteInstance
    {
        string InitialDirection { get; set; }
        ITransformElement Transform { get; set; }
    }
}
