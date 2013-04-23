using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Amphibian.Systems.Rendering.Common.Xml;

namespace Amphibian.Systems.Rendering.Spine.Xml
{
    public class XmlSkeletonDefElement : SkeletonDefElement, IXmlSerializable
    {
        public XmlSkeletonDefElement ()
        { }

        public XmlSkeletonDefElement (SkeletonDefElement element)
        {
            if (element != null) {
                Atlas = new XmlSpineAtlasElement(element.Atlas);
                Skeleton = new XmlSpineSkeletonElement(element.Skeleton);
                AnimationMap = new XmlSpineAnimationMapElement(element.AnimationMap);
                ActivityMap = new XmlActivityMapElement(element.ActivityMap);
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
                        case "Atlas":
                            XmlSpineAtlasElement atlas = new XmlSpineAtlasElement();
                            atlas.ReadXml(reader);
                            Atlas = atlas;
                            break;

                        case "Skeleton":
                            XmlSpineSkeletonElement skeleton = new XmlSpineSkeletonElement();
                            skeleton.ReadXml(reader);
                            Skeleton = skeleton;
                            break;

                        case "AnimationMap":
                            XmlSpineAnimationMapElement animationMap = new XmlSpineAnimationMapElement();
                            animationMap.ReadXml(reader);
                            AnimationMap = animationMap;
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

        public void WriteXml (XmlWriter writer)
        {
            if (Atlas != null) {
                XmlSpineAtlasElement elem = Atlas as XmlSpineAtlasElement;
                if (elem == null)
                    elem = new XmlSpineAtlasElement(Atlas);

                writer.WriteStartElement("Atlas");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (Skeleton != null) {
                XmlSpineSkeletonElement elem = Skeleton as XmlSpineSkeletonElement;
                if (elem == null)
                    elem = new XmlSpineSkeletonElement(Skeleton);

                writer.WriteStartElement("Skeleton");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (AnimationMap != null) {
                XmlSpineAnimationMapElement elem = AnimationMap as XmlSpineAnimationMapElement;
                if (elem == null)
                    elem = new XmlSpineAnimationMapElement(AnimationMap);

                writer.WriteStartElement("AnimationMap");
                elem.WriteXml(writer);
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
