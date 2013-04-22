using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlSpineAnimationMapElement : ISpineAnimationMapElement, IXmlSerializable
    {
        public IList<ISpineAnimationSetElement> Animations { get; set; }

        public XmlSpineAnimationMapElement ()
        {
            Animations = new List<ISpineAnimationSetElement>();
        }

        public XmlSpineAnimationMapElement (ISpineAnimationMapElement element)
            : this()
        {
            if (element != null) {
                foreach (ISpineAnimationSetElement animElement in element.Animations) {
                    Animations.Add(new XmlSpineAnimationSetElement(animElement));
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

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "AnimationSet":
                            XmlSpineAnimationSetElement animation = new XmlSpineAnimationSetElement();
                            animation.ReadXml(reader);
                            Animations.Add(animation);
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
            foreach (ISpineAnimationSetElement animation in Animations) {
                XmlSpineAnimationSetElement elem = animation as XmlSpineAnimationSetElement;
                if (elem == null)
                    elem = new XmlSpineAnimationSetElement(animation);

                writer.WriteStartElement("AnimationSet");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
