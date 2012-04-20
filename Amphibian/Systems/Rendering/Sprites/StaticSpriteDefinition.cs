using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class StaticSpriteDefinition
    {
        private Rectangle _source;
        private int _width;
        private int _height;

        private Texture2D _texture;
        private Vector2 _origin;

        public StaticSpriteDefinition ()
        {
            _origin = Vector2.Zero;
        }

        public StaticSprite CreateSprite ()
        {
            return new StaticSprite(this);
        }

        #region Properties

        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public float Left
        {
            get { return -_origin.X; }
        }

        public float Right
        {
            get { return _width - _origin.X; }
        }

        public float Top
        {
            get { return -_origin.Y; }
        }

        public float Bottom
        {
            get { return _height - _origin.Y; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int Width
        {
            get { return _width; }
        }


        private bool Initialized
        {
            get { return _texture != null; }
        }

        #endregion

        public void Load (ContentManager contentManager, string assetName)
        {
            _texture = contentManager.Load<Texture2D>(assetName);
            LoadContent(_texture, new Rectangle(0, 0, _texture.Width, _texture.Height));
        }

        public void Load (ContentManager contentManager, string assetName, Rectangle source)
        {
            _texture = contentManager.Load<Texture2D>(assetName);
            LoadContent(_texture, source);
        }

        private void LoadContent (Texture2D tex, Rectangle source)
        {
            _source = source;
            _width = source.Width;
            _height = source.Height;

            _origin = new Vector2(_texture.Width >> 1, _texture.Height >> 1);
        }

        public void Draw (SpriteBatch spriteBatch, PointFP position, SpriteInfo spriteData)
        {
            Vector2 p = new Vector2(position.X.Floor, position.Y.Floor);
            spriteBatch.Draw(_texture, p, _source,
                MakeBlendColor(spriteData.Opacity), spriteData.Rotation, Origin, spriteData.Scale, spriteData.Effects, 0);
        }

        private Color MakeBlendColor (float opacity)
        {
            return new Color(opacity, opacity, opacity, opacity);
        }
    }
}
