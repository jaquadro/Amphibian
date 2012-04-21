using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class DirectionalAnimationSet : Sprite
    {
        private DirectionalAnimationSetDefinition _definition;
        private Dictionary<String, AnimatedSprite[]> _anims;
        private Direction _currentDirection;
        private String _currentSet;

        public DirectionalAnimationSet (DirectionalAnimationSetDefinition definition)
        {
            _definition = definition;

            _anims = new Dictionary<String, AnimatedSprite[]>();
            foreach (String key in _definition.AvailableSets) {
                _anims[key] = new AnimatedSprite[16];
                for (int i = 0; i < 16; i++) {
                    AnimatedSpriteDefinition seqDefinition = _definition[key][(Direction)i];
                    if (seqDefinition != null)
                        _anims[key][i] = new AnimatedSprite(seqDefinition, SpriteInfo);
                }
            }
        }

        public override float Left
        {
            get { return _anims[_currentSet][(int)_currentDirection].Left; }
        }

        public override float Right
        {
            get { return _anims[_currentSet][(int)_currentDirection].Right; }
        }

        public override float Top
        {
            get { return _anims[_currentSet][(int)_currentDirection].Top; }
        }

        public override float Bottom
        {
            get { return _anims[_currentSet][(int)_currentDirection].Bottom; }
        }

        public AnimatedSprite this[String set, Direction direction]
        {
            get { return _anims[set][(int)direction]; }
        }

        public AnimatedSprite CurrentSequence
        {
            get { return _anims[_currentSet][(int)_currentDirection]; }
        }

        public Direction CurrentDirection
        {
            get { return _currentDirection; }
            set { _currentDirection = value; }
        }

        public String CurrentAnimationSet
        {
            get { return _currentSet; }
            set { _currentSet = value; }
        }

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            _anims[_currentSet][(int)_currentDirection].Draw(spriteBatch, position);
        }

        public override void Update (AmphibianGameTime gameTime)
        {
            _anims[_currentSet][(int)_currentDirection].Update(gameTime);
        }
    }
}
