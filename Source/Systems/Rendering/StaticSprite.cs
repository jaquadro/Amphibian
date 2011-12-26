using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Amphibian.Geometry;

namespace Amphibian.Systems.Rendering
{
    public class StaticSprite : Sprite
    {
        private Rectangle _source;
        private int _width;
        private int _height;

        private Texture2D _texture;

        private Vector2 _origin;

        public StaticSprite ()
            : base()
        {
            _origin = Vector2.Zero;
        }

        #region Properties

        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public override float Left
        {
            get { return -_origin.X; }
        }

        public override float Right
        {
            get { return _width - _origin.X; }
        }

        public override float Top
        {
            get { return -_origin.Y; }
        }

        public override float Bottom
        {
            get { return _height - _origin.Y; }
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
            _width = (int)(source.Width * Scale);
            _height = (int)(source.Height * Scale);

            _origin = new Vector2(_texture.Width >> 1, _texture.Height >> 1);
        }

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            Vector2 p = new Vector2(position.X.Floor, position.Y.Floor);
            spriteBatch.Draw(_texture, p, _source,
                new Color(Opacity, Opacity, Opacity, Opacity), Rotation, _origin, Scale, Effects, 0);
        }
    }
}
