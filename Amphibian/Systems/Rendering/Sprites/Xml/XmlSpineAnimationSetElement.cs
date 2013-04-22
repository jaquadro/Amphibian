using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlSpineAnimationSetElement : ISpineAnimationSetElement, IXmlSerializable
    {
        #region ISpineDirectionElement

        public string Name { get; set; }
        public string DefaultAnimation { get; set; }
        public IList<ISpineDirectionElement> Directions { get; set; }

        #endregion

        public XmlSpineAnimationSetElement ()
        {
            Directions = new List<ISpineDirectionElement>();
        }

        public XmlSpineAnimationSetElement (ISpineAnimationSetElement element)
            : this()
        {
            if (element != null) {
                Name = element.Name;
                DefaultAnimation = element.DefaultAnimation;

                foreach (ISpineDirectionElement directionElement in element.Directions) {
                    Directions.Add(new XmlSpineDirectionElement(directionElement));
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
            DefaultAnimation = reader.GetAttribute("DefaultAnimation");

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "Direction":
                            XmlSpineDirectionElement direction = new XmlSpineDirectionElement();
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

            if (DefaultAnimation != null)
                writer.WriteAttributeString("DefaultAnimation", DefaultAnimation);

            foreach (ISpineDirectionElement direction in Directions) {
                XmlSpineDirectionElement elem = direction as XmlSpineDirectionElement;
                if (elem == null)
                    elem = new XmlSpineDirectionElement(direction);

                writer.WriteStartElement("Direction");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
