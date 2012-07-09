using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    [XmlType("StaticSprite")]
    public class StaticSpriteElement
    {
        [XmlElement]
        public StaticSpriteInstance Instance { get; set; }

        [XmlElement]
        public XmlSpriteListElement Sprites { get; set; }

        public StaticSpriteDefinition BuildDefinition (ContentManager contentManager)
        {
            StaticSpriteDefinition definition = new StaticSpriteDefinition();

            XmlSpriteElement sprite = Sprites.Sprites[0] as XmlSpriteElement;
            definition.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
            definition.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

            return definition;
        }
    }

    [XmlType("Instance")]
    public class StaticSpriteInstance
    {
        [XmlElement]
        public XmlTransformElement Transform { get; set; }
    }
}
