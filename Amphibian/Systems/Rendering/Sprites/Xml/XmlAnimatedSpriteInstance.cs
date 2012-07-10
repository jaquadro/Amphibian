using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlAnimatedSpriteInstance : IAnimatedSpriteInstance, IXmlSerializable
    {
        #region IAnimatedSpriteInstance

        public ITransformElement Transform { get; set; }

        #endregion

        public XmlAnimatedSpriteInstance ()
        { }

        public XmlAnimatedSpriteInstance (IAnimatedSpriteInstance animatedSpriteInstance)
        {
            if (animatedSpriteInstance != null) {
                Transform = new XmlTransformElement(animatedSpriteInstance.Transform);
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
                        case "Transform":
                            XmlTransformElement transform = new XmlTransformElement();
                            transform.ReadXml(reader);
                            Transform = transform;
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
            if (Transform != null) {
                XmlTransformElement elem = Transform as XmlTransformElement;
                if (elem == null)
                    elem = new XmlTransformElement(Transform);

                writer.WriteStartElement("Transform");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
