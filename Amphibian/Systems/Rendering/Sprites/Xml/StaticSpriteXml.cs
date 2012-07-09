using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Xml.Schema;
using System.Xml;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public abstract class StaticSpriteElement
    {
        public IStaticSpriteInstance Instance { get; set; }

        public ISpriteListElement Sprites { get; set; }

        public StaticSpriteDefinition BuildDefinition (ContentManager contentManager)
        {
            StaticSpriteDefinition definition = new StaticSpriteDefinition();

            ISpriteElement sprite = Sprites.Sprites[0];
            definition.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
            definition.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

            return definition;
        }
    }

    public class XmlStaticSpriteElement : StaticSpriteElement, IXmlSerializable
    {
        public XmlStaticSpriteElement ()
        { }

        public XmlStaticSpriteElement (StaticSpriteElement staticSpriteElement)
        {
            if (staticSpriteElement != null) {
                Instance = new XmlStaticSpriteInstance(staticSpriteElement.Instance);
                Sprites = new XmlSpriteListElement(staticSpriteElement.Sprites);
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
                        case "Instance":
                            XmlStaticSpriteInstance instance = new XmlStaticSpriteInstance();
                            instance.ReadXml(reader);
                            Instance = instance;
                            break;

                        case "Sprites":
                            XmlSpriteListElement sprites = new XmlSpriteListElement();
                            sprites.ReadXml(reader);
                            Sprites = sprites;
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
            if (Instance != null) {
                XmlStaticSpriteInstance elem = Instance as XmlStaticSpriteInstance;
                if (elem == null)
                    elem = new XmlStaticSpriteInstance(Instance);

                writer.WriteStartElement("Instance");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (Sprites != null) {
                XmlSpriteListElement elem = Sprites as XmlSpriteListElement;
                if (elem == null)
                    elem = new XmlSpriteListElement(Sprites);

                writer.WriteStartElement("Sprites");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }

    public interface IStaticSpriteInstance
    {
        ITransformElement Transform { get; set; }
    }

    public class XmlStaticSpriteInstance : IStaticSpriteInstance, IXmlSerializable
    {
        #region IStaticSpriteInstance

        public ITransformElement Transform { get; set; }

        #endregion

        public XmlStaticSpriteInstance ()
        { }

        public XmlStaticSpriteInstance (IStaticSpriteInstance staticSpriteInstance)
        {
            if (staticSpriteInstance != null) {
                Transform = new XmlTransformElement(staticSpriteInstance.Transform);
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
                        case "Transform":
                            XmlTransformElement transform = new XmlTransformElement();
                            transform.ReadXml(reader);
                            Transform = transform;
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
            if (Transform != null) {
                XmlTransformElement elem = Transform as XmlTransformElement;
                if (elem == null)
                    elem = new XmlTransformElement(Transform);

                writer.WriteStartElement("Transform");
                elem.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
