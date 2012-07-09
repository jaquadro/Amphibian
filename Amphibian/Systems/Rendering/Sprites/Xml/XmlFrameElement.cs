using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlFrameElement : IFrameElement, IXmlSerializable
    {
        #region IFrameElement

        public string Sprite { get; set; }
        public float Duration { get; set; }

        #endregion

        public XmlFrameElement ()
        { }

        public XmlFrameElement (IFrameElement frameElement)
        {
            if (frameElement != null) {
                Sprite = frameElement.Sprite;
                Duration = frameElement.Duration;
            }
        }

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
            reader.MoveToContent();

            Sprite = reader["Sprite"];
            Duration = Convert.ToSingle(reader["Duration"], CultureInfo.InvariantCulture);

            reader.Skip();
        }

        public void WriteXml (XmlWriter writer)
        {
            writer.WriteAttributeString("Sprite", Sprite);
            writer.WriteAttributeString("Duration", Convert.ToString(Duration, CultureInfo.InvariantCulture));
        }
    }
}
