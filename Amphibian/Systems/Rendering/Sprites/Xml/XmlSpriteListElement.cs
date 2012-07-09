using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlSpriteListElement : ISpriteListElement, IXmlSerializable
    {
        #region ISpriteListElement

        public string Source { get; set; }
        public IList<ISpriteElement> Sprites { get; set; }

        #endregion

        public XmlSpriteListElement ()
        {
            Sprites = new List<ISpriteElement>();
        }

        public XmlSpriteListElement (ISpriteListElement spriteListElement)
        {
            if (spriteListElement != null) {
                Source = spriteListElement.Source;

                foreach (ISpriteElement spriteElement in spriteListElement.Sprites) {
                    Sprites.Add(new XmlSpriteElement(spriteElement));
                }
            }
        }

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
            reader.MoveToContent();

            Source = reader["Source"];

            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "Sprite":
                            XmlSpriteElement sprite = new XmlSpriteElement();
                            sprite.ReadXml(reader);
                            Sprites.Add(sprite);
                            break;

                        default:
                            reader.Skip();
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement) {
                    reader.ReadEndElement();
                    break;
                }
                else
                    reader.Read();
            }
        }

        public void WriteXml (XmlWriter writer)
        {
            writer.WriteAttributeString("Source", Source);

            foreach (ISpriteElement sprite in Sprites) {
                XmlSpriteElement elem = sprite as XmlSpriteElement;
                if (elem == null)
                    elem = new XmlSpriteElement(sprite);

                writer.WriteStartElement("Sprite");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
