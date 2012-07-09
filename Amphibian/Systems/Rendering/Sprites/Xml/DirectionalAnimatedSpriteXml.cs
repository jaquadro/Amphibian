using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    [XmlType("DirectionalAnimatedSprite")]
    public class DirectionalAnimatedSpriteElement
    {
        [XmlElement]
        public DirectionalAnimatedSpriteInstance Instance { get; set; }

        [XmlElement]
        public XmlSpriteListElement Sprites { get; set; }

        [XmlArray]
        [XmlArrayItem("Direction")]
        public XmlDirectionElement[] Directions { get; set; }

        public DirectionalAnimatedSpriteDefinition BuildDefinition (ContentManager contentManager)
        {
            DirectionalAnimatedSpriteDefinition definition = new DirectionalAnimatedSpriteDefinition();

            Dictionary<String, StaticSpriteDefinition> spriteDefs = new Dictionary<string, StaticSpriteDefinition>();
            foreach (XmlSpriteElement sprite in Sprites.Sprites) {
                StaticSpriteDefinition spriteDef = new StaticSpriteDefinition();
                spriteDef.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
                spriteDef.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

                spriteDefs[sprite.Name] = spriteDef;
            }

            foreach (XmlDirectionElement direction in Directions) {
                AnimatedSpriteDefinition animDef = new AnimatedSpriteDefinition();
                foreach (XmlFrameElement frame in direction.Animation.Frames) {
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

    [XmlType("Instance")]
    public class DirectionalAnimatedSpriteInstance
    {
        [XmlAttribute]
        public String InitialDirection { get; set; }

        [XmlElement]
        public XmlTransformElement Transform { get; set; }
    }
}
