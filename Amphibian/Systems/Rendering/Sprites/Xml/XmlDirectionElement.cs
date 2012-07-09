using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlDirectionElement : IDirectionElement, IXmlSerializable
    {
        #region IDirectionElement

        public string Value { get; set; }
        public IAnimationElement Animation { get; set; }

        #endregion

        public XmlDirectionElement ()
        { }

        public XmlDirectionElement (IDirectionElement directionElement)
        {
            if (directionElement != null) {
                Value = directionElement.Value;
                Animation = new XmlAnimationElement(directionElement.Animation);
            }
        }

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
            reader.MoveToContent();

            Value = reader["Value"];

            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "Animation":
                            XmlAnimationElement animation = new XmlAnimationElement();
                            animation.ReadXml(reader);
                            Animation = animation;
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
            writer.WriteAttributeString("Value", Value);

            if (Animation != null) {
                XmlAnimationElement elem = Animation as XmlAnimationElement;
                if (elem == null)
                    elem = new XmlAnimationElement(Animation);

                writer.WriteStartElement("Animation");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
