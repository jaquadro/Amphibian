using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlAnimatedSpriteElement : AnimatedSpriteElement, IXmlSerializable
    {
        public XmlAnimatedSpriteElement ()
        { }

        public XmlAnimatedSpriteElement (AnimatedSpriteElement animatedSpriteElement)
        {
            if (animatedSpriteElement != null) {
                Instance = new XmlAnimatedSpriteInstance(animatedSpriteElement.Instance);
                Sprites = new XmlSpriteListElement(animatedSpriteElement.Sprites);
                Animation = new XmlAnimationElement(animatedSpriteElement.Animation);
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
                            XmlAnimatedSpriteInstance instance = new XmlAnimatedSpriteInstance();
                            instance.ReadXml(reader);
                            Instance = instance;
                            break;

                        case "Sprites":
                            XmlSpriteListElement sprites = new XmlSpriteListElement();
                            sprites.ReadXml(reader);
                            Sprites = sprites;
                            break;

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
            if (Instance != null) {
                XmlAnimatedSpriteInstance elem = Instance as XmlAnimatedSpriteInstance;
                if (elem == null)
                    elem = new XmlAnimatedSpriteInstance(Instance);

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
