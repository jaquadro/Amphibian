﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites.Xml
{
    [XmlType("DirectionalAnimationSet")]
    public class DirectionalAnimationSetElement
    {
        [XmlElement]
        public DirectionalAnimationSetInstance Instance { get; set; }

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
    public class DirectionalAnimationSetInstance
    {
        [XmlAttribute]
        public String InitialAnimationSet { get; set; }

        [XmlAttribute]
        public String InitialDirection { get; set; }

        [XmlElement]
        public TransformElement Transform { get; set; }
    }
}