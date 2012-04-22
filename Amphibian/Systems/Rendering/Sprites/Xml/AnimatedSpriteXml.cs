using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    [XmlType("AnimatedSprite")]
    public class AnimatedSpriteElement
    {
        [XmlElement]
        public AnimatedSpriteInstance Instance { get; set; }

        [XmlElement]
        public SpritesElement Sprites { get; set; }

        [XmlElement]
        public AnimationElement Animation { get; set; }

        public AnimatedSpriteDefinition BuildDefinition (ContentManager contentManager)
        {
            AnimatedSpriteDefinition definition = new AnimatedSpriteDefinition();

            Dictionary<String, StaticSpriteDefinition> spriteDefs = new Dictionary<string, StaticSpriteDefinition>();
            foreach (SpriteElement sprite in Sprites.Sprites) {
                StaticSpriteDefinition spriteDef = new StaticSpriteDefinition();
                spriteDef.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
                spriteDef.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

                spriteDefs[sprite.Name] = spriteDef;
            }

            foreach (FrameElement frame in Animation.Frames) {
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

    [XmlType("Instance")]
    public class AnimatedSpriteInstance
    {
        [XmlElement]
        public TransformElement Transform { get; set; }
    }
}
