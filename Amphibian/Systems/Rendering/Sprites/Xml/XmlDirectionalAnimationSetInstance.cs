using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlDirectionalAnimationSetInstance : IDirectionalAnimationSetInstance, IXmlSerializable
    {
        #region IDirectionalAnimationSetInstance

        public string InitialAnimationSet { get; set; }
        public string InitialDirection { get; set; }
        public ITransformElement Transform { get; set; }

        #endregion

        public XmlDirectionalAnimationSetInstance ()
        { }

        public XmlDirectionalAnimationSetInstance (IDirectionalAnimationSetInstance spriteInstance)
        {
            if (spriteInstance != null) {
                InitialAnimationSet = spriteInstance.InitialAnimationSet;
                InitialDirection = spriteInstance.InitialDirection;
                Transform = new XmlTransformElement(spriteInstance.Transform);
            }
        }

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
            reader.MoveToContent();

            InitialAnimationSet = reader["InitialAnimationSet"];
            InitialDirection = reader["InitialDirection"];

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
            if (InitialAnimationSet != null)
                writer.WriteAttributeString("InitialAnimationSet", InitialAnimationSet);

            if (InitialDirection != null)
                writer.WriteAttributeString("InitialDirection", InitialDirection);

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
