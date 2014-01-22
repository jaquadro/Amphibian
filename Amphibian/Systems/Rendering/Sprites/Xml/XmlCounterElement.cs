using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlCounterElement : CounterElement, IXmlSerializable
    {
        public XmlCounterElement ()
        { }

        public XmlCounterElement (CounterElement counterElement)
        {
            if (counterElement != null) {
                Instance = new XmlCounterInstance(counterElement.Instance);
                Sprites = new XmlSpriteListElement(counterElement.Sprites);
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
                            XmlCounterInstance instance = new XmlCounterInstance();
                            instance.ReadXml(reader);
                            Instance = instance;
                            break;

                        case "Sprites":
                            XmlSpriteListElement sprites = new XmlSpriteListElement();
                            sprites.ReadXml(reader);
                            Sprites = sprites;
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
                XmlCounterInstance elem = Instance as XmlCounterInstance;
                if (elem == null)
                    elem = new XmlCounterInstance(Instance);

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
        }
    }
}
