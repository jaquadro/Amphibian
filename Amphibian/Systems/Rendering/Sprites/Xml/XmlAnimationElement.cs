using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System;
using System.Globalization;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public class XmlAnimationElement : IAnimationElement, IXmlSerializable
    {
        #region IAnimationElement

        public int Repeat { get; set; }
        public IList<IFrameElement> Frames { get; set; }

        #endregion

        public XmlAnimationElement ()
        {
            Frames = new List<IFrameElement>();
        }

        public XmlAnimationElement (IAnimationElement animationElement)
        {
            if (animationElement != null) {
                Repeat = animationElement.Repeat;

                foreach (IFrameElement frameElement in animationElement.Frames) {
                    Frames.Add(new XmlFrameElement(frameElement));
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

            Repeat = Convert.ToInt32(reader["Repeat"], CultureInfo.InvariantCulture);

            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();
            while (!reader.EOF) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "Frames":
                            ReadFramesXml(reader);
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

        private void ReadFramesXml (XmlReader reader)
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
                        case "Frame":
                            XmlFrameElement frame = new XmlFrameElement();
                            frame.ReadXml(reader);
                            Frames.Add(frame);
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
            writer.WriteAttributeString("Repeat", Convert.ToString(Repeat, CultureInfo.InvariantCulture));

            writer.WriteStartElement("Frames");

            foreach (IFrameElement frame in Frames) {
                XmlFrameElement elem = frame as XmlFrameElement;
                if (elem == null)
                    elem = new XmlFrameElement(frame);

                writer.WriteStartElement("Frame");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
