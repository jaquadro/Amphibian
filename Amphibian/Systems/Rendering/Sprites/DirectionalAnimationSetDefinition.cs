using System;
using System.Collections.Generic;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class DirectionalAnimationSetDefinition
    {
        private Dictionary<String, DirectionalAnimatedSpriteDefinition> _sets;

        public DirectionalAnimationSetDefinition ()
        {
            _sets = new Dictionary<string, DirectionalAnimatedSpriteDefinition>();
        }

        public DirectionalAnimationSet CreateSprite ()
        {
            return new DirectionalAnimationSet(this);
        }

        public IEnumerable<String> AvailableSets
        {
            get { return _sets.Keys; }
        }

        public DirectionalAnimatedSpriteDefinition this[String set]
        {
            get
            {
                DirectionalAnimatedSpriteDefinition defn;
                if (_sets.TryGetValue(set, out defn))
                    return defn;
                return null;
            }

            set
            {
                if (value == null)
                    _sets.Remove(set);
                else
                    _sets[set] = value;
            }
        }
    }
}
