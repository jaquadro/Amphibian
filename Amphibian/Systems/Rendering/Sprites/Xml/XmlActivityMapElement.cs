using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlActivityMapElement : IActivityMapElement, IXmlSerializable
    {
        #region IActivityMapElement

        public string DefaultAnimation { get; set; }
        public IList<IActivityElement> Activities { get; set; }

        #endregion

        public XmlActivityMapElement ()
        {
            Activities = new List<IActivityElement>();
        }

        public XmlActivityMapElement (IActivityMapElement activityMapElement)
        {
            if (activityMapElement != null) {
                DefaultAnimation = activityMapElement.DefaultAnimation;

                foreach (IActivityElement activityElement in activityMapElement.Activities) {
                    Activities.Add(new XmlActivityElement(activityElement));
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

            DefaultAnimation = reader["DefaultAnimation"];

            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "Activity":
                            XmlActivityElement activity = new XmlActivityElement();
                            activity.ReadXml(reader);
                            Activities.Add(activity);
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
            writer.WriteAttributeString("DefaultAnimation", DefaultAnimation);

            foreach (IActivityElement activity in Activities) {
                XmlActivityElement elem = activity as XmlActivityElement;
                if (elem == null)
                    elem = new XmlActivityElement(activity);

                writer.WriteStartElement("Activity");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
