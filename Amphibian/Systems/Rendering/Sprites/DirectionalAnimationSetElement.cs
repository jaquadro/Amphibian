using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class DirectionalAnimationSetElement
    {
        public IDirectionalAnimationSetInstance Instance { get; set; }

        public ISpriteListElement Sprites { get; set; }

        public IList<IAnimationSetElement> AnimationSets { get; set; }

        public IActivityMapElement ActivityMap { get; set; }

        public DirectionalAnimationSetDefinition BuildDefinition (ContentManager contentManager)
        {
            DirectionalAnimationSetDefinition definition = new DirectionalAnimationSetDefinition();

            Dictionary<String, StaticSpriteDefinition> spriteDefs = new Dictionary<string, StaticSpriteDefinition>();
            foreach (ISpriteElement sprite in Sprites.Sprites) {
                StaticSpriteDefinition spriteDef = new StaticSpriteDefinition();
                spriteDef.Load(contentManager, Sprites.Source, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));
                spriteDef.Origin = new Vector2(sprite.OriginX, sprite.OriginY);

                spriteDefs[sprite.Name] = spriteDef;
            }

            foreach (IAnimationSetElement animSet in AnimationSets) {
                definition[animSet.Name] = new DirectionalAnimatedSpriteDefinition();

                foreach (IDirectionElement direction in animSet.Directions) {
                    AnimatedSpriteDefinition animDef = new AnimatedSpriteDefinition();
                    foreach (IFrameElement frame in direction.Animation.Frames) {
                        if (spriteDefs.ContainsKey(frame.Sprite))
                            animDef.AddSprite(spriteDefs[frame.Sprite], frame.Duration);
                    }

                    if (direction.Animation.Repeat < 0)
                        animDef.RepeatIndefinitely = true;
                    else
                        animDef.RepeatLimit = direction.Animation.Repeat;

                    definition[animSet.Name][(Direction)Enum.Parse(typeof(Direction), direction.Value, false)] = animDef;
                }
            }

            return definition;
        }

        public Dictionary<String, String> BuildActivityMap ()
        {
            Dictionary<String, String> map = new Dictionary<string, string>();

            foreach (IActivityElement activity in ActivityMap.Activities) {
                map[activity.Name] = activity.Animation;
            }

            return map;
        }
    }

    public interface IDirectionalAnimationSetInstance
    {
        string InitialAnimationSet { get; set; }
        string InitialDirection { get; set; }
        ITransformElement Transform { get; set; }
    }
}
