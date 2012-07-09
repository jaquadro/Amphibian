using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.Globalization;
using System.Collections.Generic;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    public static class ColorExt
    {
        public static string ToRgbHex (this Color color)
        {
            return "#" + ByteToHex2(color.R) + ByteToHex2(color.G) + ByteToHex2(color.B);
        }

        public static string ToArgbHex (this Color color)
        {
            return "#" + ByteToHex2(color.A) + ByteToHex2(color.R) + ByteToHex2(color.G) + ByteToHex2(color.B);
        }

        public static Color ParseRgbHex (string hex)
        {
            if ((hex.Length != 6 && hex.Length != 7) ||
                (hex.Length == 7 && hex[0] != '#'))
                throw new ArgumentException("Invalid ARGB hex string.", "hex");

            if (hex[0] == '#')
                hex = hex.Substring(1);

            try {
                byte r = Convert.ToByte(hex.Substring(2, 2), 16);
                byte g = Convert.ToByte(hex.Substring(4, 2), 16);
                byte b = Convert.ToByte(hex.Substring(6, 2), 16);

                return new Color()
                {
                    A = 255, R = r, G = g, B = b
                };
            }
            catch (FormatException e) {
                throw new ArgumentException("Invalid ARGB hex string.", "hex");
            }
        }

        public static Color ParseArgbHex (string hex)
        {
            if ((hex.Length != 8 && hex.Length != 9) ||
                (hex.Length == 9 && hex[0] != '#'))
                throw new ArgumentException("Invalid ARGB hex string.", "hex");

            if (hex[0] == '#')
                hex = hex.Substring(1);

            try {
                byte a = Convert.ToByte(hex.Substring(0, 2), 16);
                byte r = Convert.ToByte(hex.Substring(2, 2), 16);
                byte g = Convert.ToByte(hex.Substring(4, 2), 16);
                byte b = Convert.ToByte(hex.Substring(6, 2), 16);

                return new Color()
                {
                    A = a, R = r, G = g, B = b
                };
            }
            catch (FormatException) {
                throw new ArgumentException("Invalid ARGB hex string.", "hex");
            }
        }

        private static string ByteToHex2 (byte value)
        {
            String hex = Convert.ToString(value, 16);
            if (hex.Length >= 2)
                return hex;
            else
                return "0" + hex;
        }
    }

    public interface ITransformElement
    {
        float Scale { get; set; }
        float Rotation { get; set; }
        float Opacity { get; set; }
        Color BlendColor { get; set; }
    }

    public interface ISpriteElement
    {
        string Name { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        int OriginX { get; set; }
        int OriginY { get; set; }
    }

    public interface ISpriteListElement
    {
        string Source { get; set; }
        IList<ISpriteElement> Sprites { get; set; }
    }

    

    

    

    /*[XmlType("Transform")]
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
    }*/

    /*[XmlType("Sprites")]
    public class SpritesElement
    {
        [XmlAttribute]
        public string Source { get; set; }

        [XmlElement("Sprite")]
        public SpriteElement[] Sprites { get; set; }
    }*/

    /*[XmlType("Sprite")]
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
    }*/

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
        public XmlActivityElement[] Acitivies { get; set; }
    }

    public interface IActivityElement
    {
        string Name { get; set; }
        string Animation { get; set; }
    }

    /*[XmlType("Activity")]
    public class ActivityElement
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Animation { get; set; }
    }*/
}
