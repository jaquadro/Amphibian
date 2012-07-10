using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlDirectionalAnimatedSpriteElement : DirectionalAnimatedSpriteElement, IXmlSerializable
    {
        public XmlDirectionalAnimatedSpriteElement ()
        {
            Directions = new List<IDirectionElement>();
        }

        public XmlDirectionalAnimatedSpriteElement (DirectionalAnimatedSpriteElement spriteElement)
        {
            if (spriteElement != null) {
                Instance = new XmlDirectionalAnimatedSpriteInstance(spriteElement.Instance);
                Sprites = new XmlSpriteListElement(spriteElement.Sprites);

                foreach (IDirectionElement directionElement in spriteElement.Directions) {
                    Directions.Add(new XmlDirectionElement(directionElement));
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
            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "Instance":
                            XmlDirectionalAnimatedSpriteInstance instance = new XmlDirectionalAnimatedSpriteInstance();
                            instance.ReadXml(reader);
                            Instance = instance;
                            break;

                        case "Sprites":
                            XmlSpriteListElement sprites = new XmlSpriteListElement();
                            sprites.ReadXml(reader);
                            Sprites = sprites;
                            break;

                        case "Directions":
                            ReadDirectionsXml(reader);
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

        private void ReadDirectionsXml (XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "Direction":
                            XmlDirectionElement direction = new XmlDirectionElement();
                            direction.ReadXml(reader);
                            Directions.Add(direction);
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
            if (Instance != null) {
                XmlDirectionalAnimatedSpriteInstance elem = Instance as XmlDirectionalAnimatedSpriteInstance;
                if (elem == null)
                    elem = new XmlDirectionalAnimatedSpriteInstance(Instance);

                writer.WriteStartElement("Instance");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (Sprites != null) {
                XmlSpriteListElement elem = Sprites as XmlSpriteListElement;
                if (elem == null)
                    elem = new XmlSpriteListElement(Sprites);

                writer.WriteStartElement("Sprites");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (Directions != null) {
                writer.WriteStartElement("Directions");

                foreach (IDirectionElement direction in Directions) {
                    XmlDirectionElement elem = direction as XmlDirectionElement;
                    if (elem == null)
                        elem = new XmlDirectionElement(direction);

                    writer.WriteStartElement("Direction");
                    elem.WriteXml(writer);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }
    }
}
