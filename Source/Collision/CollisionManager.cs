using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Drawing;
using Amphibian.Geometry;

namespace Amphibian.Collision
{
    public interface ICollidable
    {
        Mask CollisionMask { get; }
    }

    public interface ICollisionManager
    {
        void AddObject (ICollidable collidable);
        void RemoveObject (ICollidable collidable);

        bool OverlapsAny (Mask mask);
    }

    class CollisionManager
    {

    }

    public class TileCollisionManager : ICollisionManager
    {
        private Mask[,] _grid;
        private CollisionTileMapper _trans;

        private int _width;
        private int _height;
        private int _tileWidth;
        private int _tileHeight;

        public TileCollisionManager (int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
        {
            _width = tilesWide;
            _height = tilesHigh;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;

            _grid = new Mask[tilesWide, tilesHigh];
            _trans = new CollisionTileMapper((FPInt)tileWidth, (FPInt)tileHeight);
        }

        public void AddObject (int id, int x, int y)
        {
            PointFP position = new PointFP(x * _tileWidth, y * _tileHeight);

            Mask mask = _trans.Lookup(id, position);
            if (mask == null) {
                return;
            }

            _grid[x, y] = mask;
        }

        public void AddObject (ICollidable collidable)
        {
            RectangleFP rect = collidable.CollisionMask.Bounds;

            if (rect.Width > _tileWidth || rect.Height > _tileHeight) {
                throw new Exception("Collidable incompatible with this Collision Manager");
            }

            int x = (int)(rect.Left / _tileWidth);
            int y = (int)(rect.Top / _tileHeight);

            if (rect.Left != _tileWidth * x || rect.Top != _tileHeight * y) {
                throw new Exception("Collidable incompatible with this Collision Manager");
            }

            /*if (_grid[x, y] == null) {
                _grid[x, y] = new List<ICollidable>();
            }

            _grid[x, y].Add(collidable);*/
        }

        public void RemoveObject (ICollidable collidable)
        {

        }

        public bool OverlapsAny (Mask mask)
        {
            RectangleFP rect = mask.Bounds;

            int minXId = (int)(rect.Left / _tileWidth);
            int maxXId = (int)(rect.Right / _tileWidth);
            int minYId = (int)(rect.Top / _tileHeight);
            int maxYId = (int)(rect.Bottom / _tileHeight);

            for (int x = minXId; x <= maxXId; x++) {
                if (x >= _width)
                    continue;
                for (int y = minYId; y <= maxYId; y++) {
                    if (y >= _height)
                        continue;

                    if (_grid[x, y] == null) {
                        continue;
                    }

                    if (_grid[x, y].TestOverlap(mask)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Draw (SpriteBatch spriteBatch, Rectangle drawArea)
        {
            int minX = Math.Max((int)Math.Floor((float)drawArea.Left / _tileWidth), 0);
            int maxX = Math.Min((int)Math.Ceiling((float)drawArea.Right / _tileWidth), _width);

            int minY = Math.Max((int)Math.Floor((float)drawArea.Top / _tileHeight), 0);
            int maxY = Math.Min((int)Math.Ceiling((float)drawArea.Bottom / _tileHeight), _height);

            for (int x = minX; x < maxX; x++) {
                for (int y = minY; y < maxY; y++) {
                    if (_grid[x, y] != null) {
                        _grid[x, y].Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
