using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System;
using System.Globalization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlAnimationSetElement : IAnimationSetElement, IXmlSerializable
    {
        #region IAnimationSetElement

        public string Name { get; set; }
        public IList<IDirectionElement> Directions { get; set; }

        #endregion

        public XmlAnimationSetElement ()
        {
            Directions = new List<IDirectionElement>();
        }

        public XmlAnimationSetElement (IAnimationSetElement animationSetElement)
        {
            if (animationSetElement != null) {
                Name = animationSetElement.Name;

                foreach (IDirectionElement directionElement in animationSetElement.Directions) {
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

            Name = reader["Name"];

            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
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
            writer.WriteAttributeString("Name", Name);

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
