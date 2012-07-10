using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlDirectionalAnimationSetElement : DirectionalAnimationSetElement, IXmlSerializable
    {
        public XmlDirectionalAnimationSetElement ()
        {
            AnimationSets = new List<IAnimationSetElement>();
        }

        public XmlDirectionalAnimationSetElement (DirectionalAnimationSetElement spriteElement)
        {
            if (spriteElement != null) {
                Instance = new XmlDirectionalAnimationSetInstance(spriteElement.Instance);
                Sprites = new XmlSpriteListElement(spriteElement.Sprites);

                foreach (IAnimationSetElement animationElement in spriteElement.AnimationSets) {
                    AnimationSets.Add(new XmlAnimationSetElement(animationElement));
                }

                ActivityMap = new XmlActivityMapElement(spriteElement.ActivityMap);
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
                            XmlDirectionalAnimationSetInstance instance = new XmlDirectionalAnimationSetInstance();
                            instance.ReadXml(reader);
                            Instance = instance;
                            break;

                        case "Sprites":
                            XmlSpriteListElement sprites = new XmlSpriteListElement();
                            sprites.ReadXml(reader);
                            Sprites = sprites;
                            break;

                        case "AnimationSets":
                            ReadAnimationSetsXml(reader);
                            break;

                        case "ActivityMap":
                            XmlActivityMapElement activityMap = new XmlActivityMapElement();
                            activityMap.ReadXml(reader);
                            ActivityMap = activityMap;
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

        private void ReadAnimationSetsXml (XmlReader reader)
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
                        case "AnimationSet":
                            XmlAnimationSetElement animationSet = new XmlAnimationSetElement();
                            animationSet.ReadXml(reader);
                            AnimationSets.Add(animationSet);
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
                XmlDirectionalAnimationSetInstance elem = Instance as XmlDirectionalAnimationSetInstance;
                if (elem == null)
                    elem = new XmlDirectionalAnimationSetInstance(Instance);

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

            if (AnimationSets != null) {
                writer.WriteStartElement("AnimationSets");

                foreach (IAnimationSetElement animationSet in AnimationSets) {
                    XmlAnimationSetElement elem = animationSet as XmlAnimationSetElement;
                    if (elem == null)
                        elem = new XmlAnimationSetElement(animationSet);

                    writer.WriteStartElement("AnimationSet");
                    elem.WriteXml(writer);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            if (ActivityMap != null) {
                XmlActivityMapElement elem = ActivityMap as XmlActivityMapElement;
                if (elem == null)
                    elem = new XmlActivityMapElement(ActivityMap);

                writer.WriteStartElement("ActivityMap");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
