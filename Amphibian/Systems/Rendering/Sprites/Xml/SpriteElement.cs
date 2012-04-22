using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    [XmlType("DirAnimationSet")]
    public class DirAnimationSetElement
    {
        [XmlElement]
        public InstanceElement Instance { get; set; }

        [XmlElement]
        public SpritesElement Sprites { get; set; }

        [XmlArray]
        [XmlArrayItem("AnimationSet")]
        public AnimationSetElement[] AnimationSets { get; set; }

        [XmlElement]
        public ActivityMapElement ActivityMap { get; set; }

        public DirectionalAnimationSetDefinition BuildDefinition (ContentManager contentManager)
        {
            DirectionalAnimationSetDefinition definition = new DirectionalAnimationSetDefinition();

            Dictionary<String, StaticSpriteDefinition> spriteDefs = new Dictionary<string, StaticSpriteDefinition>();
            foreach (SpriteElement sprite in Sprites.Sprites) {
                StaticSpriteDefinition spriteDef = new StaticSpriteDefinition();
                spriteDef.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
                spriteDef.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

                spriteDefs[sprite.Name] = spriteDef;
            }

            foreach (AnimationSetElement animSet in AnimationSets) {
                definition[animSet.Name] = new DirectionalAnimatedSpriteDefinition();

                foreach (DirectionElement direction in animSet.Directions) {
                    AnimatedSpriteDefinition animDef = new AnimatedSpriteDefinition();
                    foreach (FrameElement frame in direction.Animation.Frames) {
                        if (spriteDefs.ContainsKey(frame.Sprite))
                            animDef.AddSprite(spriteDefs[frame.Sprite], frame.Duration);
                    }

                    if (direction.Animation.Repeat < 0)
                        animDef.RepeatIndefinitely = true;
                    else
                        animDef.RepeatLimit = direction.Animation.Repeat;

                    definition[animSet.Name][(Direction)Enum.Parse(typeof(Direction), direction.Value)] = animDef;
                }
            }

            return definition;
        }

        public Dictionary<String, String> BuildActivityMap ()
        {
            Dictionary<String, String> map = new Dictionary<string, string>();

            foreach (ActivityElement activity in ActivityMap.Acitivies) {
                map[activity.Name] = activity.Animation;
            }

            return map;
        }
    }

    [XmlType("Instance")]
    public class InstanceElement
    {
        [XmlAttribute]
        public String InitialAnimationSet { get; set; }

        [XmlAttribute]
        public String InitialDirection { get; set; }

        [XmlElement]
        public TransformElement Transform { get; set; }
    }

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
