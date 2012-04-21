using System;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class DirectionalAnimatedSprite : Sprite
    {
        private DirectionalAnimatedSpriteDefinition _definition;
        private AnimatedSprite[] _anims;
        private Direction _currentDirection;

        public DirectionalAnimatedSprite (DirectionalAnimatedSpriteDefinition definition, SpriteInfo spriteInfo)
            : base(spriteInfo)
        {
            _definition = definition;

            _anims = new AnimatedSprite[16];
            for (int i = 0; i < 16; i++)
                if (_definition[(Direction)i] != null)
                    _anims[i] = new AnimatedSprite(_definition[(Direction)i], SpriteInfo);
        }

        public DirectionalAnimatedSprite (DirectionalAnimatedSpriteDefinition definition)
            : this(definition, new SpriteInfo())
        {
        }

        public override float Left
        {
            get { return _anims[(int)_currentDirection].Left; }
        }

        public override float Right
        {
            get { return _anims[(int)_currentDirection].Right; }
        }

        public override float Top
        {
            get { return _anims[(int)_currentDirection].Top; }
        }

        public override float Bottom
        {
            get { return _anims[(int)_currentDirection].Bottom; }
        }

        public AnimatedSprite this[Direction direction]
        {
            get { return _anims[(int)direction]; }
        }

        public AnimatedSprite CurrentSequence
        {
            get { return _anims[(int)_currentDirection]; }
        }

        public Direction CurrentDirection
        {
            get { return _currentDirection; }
            set { _currentDirection = value; }
        }

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            _anims[(int)_currentDirection].Draw(spriteBatch, position);
        }

        public override void Update (AmphibianGameTime gameTime)
        {
            _anims[(int)_currentDirection].Update(gameTime);
        }
    }

    
}
