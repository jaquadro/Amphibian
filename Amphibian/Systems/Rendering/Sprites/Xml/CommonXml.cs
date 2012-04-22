using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    [XmlType("Transform")]
    public class TransformElement
    {
        [XmlElement]
        public float Scale = 1f;

        [XmlElement]
        public float Rotation = 0f;

        [XmlElement]
        public float Opacity = 1f;

        [XmlElement]
        public Color BlendColor = Color.White;
    }

    [XmlType("Sprites")]
    public class SpritesElement
    {
        [XmlAttribute]
        public string Source { get; set; }

        [XmlElement("Sprite")]
        public SpriteElement[] Sprites { get; set; }
    }

    [XmlType("Sprite")]
    public class SpriteElement
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int X { get; set; }

        [XmlAttribute]
        public int Y { get; set; }

        [XmlAttribute]
        public int Height { get; set; }

        [XmlAttribute]
        public int Width { get; set; }

        [XmlAttribute]
        public int OriginX { get; set; }

        [XmlAttribute]
        public int OriginY { get; set; }
    }

    [XmlType("AnimationSet")]
    public class AnimationSetElement
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray]
        [XmlArrayItem("Direction")]
        public DirectionElement[] Directions { get; set; }
    }

    [XmlType("Direction")]
    public class DirectionElement
    {
        [XmlAttribute]
        public string Value { get; set; }

        [XmlElement]
        public AnimationElement Animation { get; set; }
    }

    [XmlType("Animation")]
    public class AnimationElement
    {
        [XmlAttribute]
        public int Repeat { get; set; }

        [XmlArray]
        [XmlArrayItem("Frame")]
        public FrameElement[] Frames { get; set; }
    }

    [XmlType("Frame")]
    public class FrameElement
    {
        [XmlAttribute]
        public string Sprite { get; set; }

        [XmlAttribute]
        public float Duration { get; set; }
    }

    [XmlType("ActivityMap")]
    public class ActivityMapElement
    {
        [XmlAttribute]
        public String DefaultAnimation { get; set; }

        [XmlElement("Activity")]
        public ActivityElement[] Acitivies { get; set; }
    }

    [XmlType("Activity")]
    public class ActivityElement
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Animation { get; set; }
    }
}
