using System;
using System.Collections.Generic;
using Amphibian.Systems.Rendering.Common;
using Amphibian.Systems.Rendering.Sprites;

namespace Amphibian.Systems.Rendering.Spine
{
    public class SkeletonDefElement
    {
        public ISpineAtlasElement Atlas { get; set; }

        public ISpineSkeletonElement Skeleton { get; set; }

        public ISpineAnimationMapElement AnimationMap { get; set; }

        public IActivityMapElement ActivityMap { get; set; }

        public Dictionary<string, string> BuildDefaultAnimationMap ()
        {
            var map = new Dictionary<string, string>();

            foreach (var animation in AnimationMap.Animations) {
                if (!string.IsNullOrEmpty(animation.DefaultAnimation))
                    map[animation.Name] = animation.DefaultAnimation;
            }

            return map;
        }

        public Dictionary<string, Dictionary<Direction, ISpineDirectionElement>> BuildDirectedAnimationMap ()
        {
            var map = new Dictionary<string, Dictionary<Direction, ISpineDirectionElement>>();

            foreach (var animation in AnimationMap.Animations) {
                if (animation.Directions.Count == 0)
                    continue;

                var submap = new Dictionary<Direction, ISpineDirectionElement>();
                foreach (var direction in animation.Directions) {
                    try {
                        Direction dir = (Direction)Enum.Parse(typeof(Direction), direction.Value, true);
                        submap[dir] = direction;
                    }
                    catch {
                        continue;
                    }
                }

                map[animation.Name] = submap;
            }

            return map;
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
}
