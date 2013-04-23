using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Common.Xml
{
    public class XmlActivityElement : IActivityElement, IXmlSerializable
    {
        #region IActivityElement

        public string Name { get; set; }
        public string Animation { get; set; }

        #endregion

        public XmlActivityElement ()
        { }

        public XmlActivityElement (IActivityElement activityElement)
        {
            if (activityElement != null) {
                Name = activityElement.Name;
                Animation = activityElement.Animation;
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
            Animation = reader["Animation"];

            reader.Skip();
        }

        public void WriteXml (XmlWriter writer)
        {
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("Animation", Animation);
        }
    }
}
