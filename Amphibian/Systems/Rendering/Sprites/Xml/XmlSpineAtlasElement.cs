using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlSpineAtlasElement : ISpineAtlasElement, IXmlSerializable
    {
        public string Source { get; set; }

        public XmlSpineAtlasElement ()
        { }

        public XmlSpineAtlasElement (ISpineAtlasElement element)
        {
            if (element != null) {
                Source = element.Source;
            }
        }

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
            reader.MoveToContent();

            Source = reader["Source"];

            reader.Skip();
        }

        public void WriteXml (XmlWriter writer)
        {
            writer.WriteAttributeString("Source", Source);
        }
    }
}
