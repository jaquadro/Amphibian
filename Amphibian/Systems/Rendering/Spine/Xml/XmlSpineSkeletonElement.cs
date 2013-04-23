using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Spine.Xml
{
    public class XmlSpineSkeletonElement : ISpineSkeletonElement, IXmlSerializable
    {
        public string Source { get; set; }

        public XmlSpineSkeletonElement ()
        { }

        public XmlSpineSkeletonElement (ISpineSkeletonElement element)
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
