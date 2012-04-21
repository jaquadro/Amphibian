using System;
using System.Collections.Generic;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class DirectionalAnimatedSpriteDefinition
    {
        private AnimatedSpriteDefinition[] _directions;
        private DirectionSet _availableDirections;

        public DirectionalAnimatedSpriteDefinition ()
        {
            _directions = new AnimatedSpriteDefinition[16];
        }

        public DirectionalAnimatedSprite CreateSprite ()
        {
            return new DirectionalAnimatedSprite(this);
        }

        public DirectionSet AvailableDiretions
        {
            get { return _availableDirections; }
        }

        public AnimatedSpriteDefinition this[Direction direction]
        {
            get { return _directions[(int)direction]; }
            set
            {
                if (value == null) {
                    _directions[(int)direction] = null;
                    _availableDirections &= ~(DirectionSet)(1 << (int)direction);
                }
                else {
                    _directions[(int)direction] = value;
                    _availableDirections |= (DirectionSet)(1 << (int)direction);
                }
            }
        }
    }
}
