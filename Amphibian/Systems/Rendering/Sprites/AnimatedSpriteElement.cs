using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites
{
    public abstract class AnimatedSpriteElement
    {
        public IAnimatedSpriteInstance Instance { get; set; }

        public ISpriteListElement Sprites { get; set; }

        public IAnimationElement Animation { get; set; }

        public AnimatedSpriteDefinition BuildDefinition (ContentManager contentManager)
        {
            AnimatedSpriteDefinition definition = new AnimatedSpriteDefinition();

            Dictionary<String, StaticSpriteDefinition> spriteDefs = new Dictionary<string, StaticSpriteDefinition>();
            foreach (ISpriteElement sprite in Sprites.Sprites) {
                StaticSpriteDefinition spriteDef = new StaticSpriteDefinition();
                spriteDef.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
                spriteDef.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

                spriteDefs[sprite.Name] = spriteDef;
            }

            foreach (IFrameElement frame in Animation.Frames) {
                if (spriteDefs.ContainsKey(frame.Sprite))
                    definition.AddSprite(spriteDefs[frame.Sprite], frame.Duration);
            }

            if (Animation.Repeat < 0)
                definition.RepeatIndefinitely = true;
            else
                definition.RepeatLimit = Animation.Repeat;

            return definition;
        }
    }

    public interface IAnimatedSpriteInstance
    {
        ITransformElement Transform { get; set; }
    }
}
