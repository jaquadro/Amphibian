using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlSpriteElement : ISpriteElement, IXmlSerializable
    {
        #region ISpriteElement

        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }

        #endregion

        public XmlSpriteElement ()
        { }

        public XmlSpriteElement (ISpriteElement spriteElement)
        {
            if (spriteElement != null) {
                Name = spriteElement.Name;
                X = spriteElement.X;
                Y = spriteElement.Y;
                Height = spriteElement.Height;
                Width = spriteElement.Width;
                OriginX = spriteElement.OriginX;
                OriginY = spriteElement.OriginY;
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
            X = Convert.ToInt32(reader["X"], CultureInfo.InvariantCulture);
            Y = Convert.ToInt32(reader["Y"], CultureInfo.InvariantCulture);
            Height = Convert.ToInt32(reader["Height"], CultureInfo.InvariantCulture);
            Width = Convert.ToInt32(reader["Width"], CultureInfo.InvariantCulture);
            OriginX = Convert.ToSingle(reader["OriginX"], CultureInfo.InvariantCulture);
            OriginY = Convert.ToSingle(reader["OriginY"], CultureInfo.InvariantCulture);

            reader.Skip();
        }

        public void WriteXml (XmlWriter writer)
        {
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("X", Convert.ToString(X, CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Y", Convert.ToString(Y, CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Height", Convert.ToString(Height, CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Width", Convert.ToString(Width, CultureInfo.InvariantCulture));
            writer.WriteAttributeString("OriginX", Convert.ToString(OriginX, CultureInfo.InvariantCulture));
            writer.WriteAttributeString("OriginY", Convert.ToString(OriginY, CultureInfo.InvariantCulture));
        }
    }
}
