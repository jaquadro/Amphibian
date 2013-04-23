using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Spine.Xml
{
    public class XmlSpineDirectionElement : ISpineDirectionElement, IXmlSerializable
    {
        #region ISpineDirectionElement

        public string Value { get; set; }
        public string Animation { get; set; }
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }

        #endregion

        public XmlSpineDirectionElement ()
        { }

        public XmlSpineDirectionElement (ISpineDirectionElement element)
        {
            if (element != null) {
                Value = element.Value;
                Animation = element.Animation;
                FlipX = element.FlipX;
                FlipY = element.FlipY;
            }
        }

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
            reader.MoveToContent();

            Value = reader["Value"];
            Animation = reader["Animation"];

            FlipX = bool.Parse(reader.GetAttribute("FlipX") ?? "false");
            FlipY = bool.Parse(reader.GetAttribute("FlipY") ?? "false");

            reader.Skip();
        }

        public void WriteXml (XmlWriter writer)
        {
            writer.WriteAttributeString("Value", Value);
            writer.WriteAttributeString("Animation", Animation);

            if (FlipX)
                writer.WriteAttributeString("FlipX", "true");
            if (FlipY)
                writer.WriteAttributeString("FlipY", "true");
        }
    }
}
