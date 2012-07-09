using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Amphibian.Xna;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlTransformElement : ITransformElement, IXmlSerializable
    {
        private const float DefaultScale = 1f;
        private const float DefaultRotation = 0f;
        private const float DefaultOpacity = 1f;
        private static Color DefaultBlendColor = Color.White;

        #region ITransformElement

        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float Opacity { get; set; }
        public Color BlendColor { get; set; }

        #endregion

        public XmlTransformElement ()
        {
            Scale = DefaultScale;
            Rotation = DefaultRotation;
            Opacity = DefaultOpacity;
            BlendColor = DefaultBlendColor;
        }

        public XmlTransformElement (ITransformElement transformElement)
        {
            if (transformElement != null) {
                Scale = transformElement.Scale;
                Rotation = transformElement.Rotation;
                Opacity = transformElement.Opacity;
                BlendColor = transformElement.BlendColor;
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
                        case "Scale":
                            Scale = reader.ReadElementContentAsFloat();
                            break;

                        case "Rotation":
                            Rotation = reader.ReadElementContentAsFloat();
                            break;

                        case "Opacity":
                            Opacity = reader.ReadElementContentAsFloat();
                            break;

                        case "BlendColor":
                            BlendColor = ColorExt.ParseArgbHex(reader.ReadElementContentAsString());
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
            if (Scale != DefaultScale)
                writer.WriteElementString("Scale", Convert.ToString(Scale, CultureInfo.InvariantCulture));

            if (Rotation != DefaultRotation)
                writer.WriteElementString("Rotation", Convert.ToString(Rotation, CultureInfo.InvariantCulture));

            if (Opacity != DefaultOpacity)
                writer.WriteElementString("Opacity", Convert.ToString(Opacity, CultureInfo.InvariantCulture));

            if (BlendColor != DefaultBlendColor)
                writer.WriteElementString("BlendColor", BlendColor.ToArgbHex());
        }
    }
}
