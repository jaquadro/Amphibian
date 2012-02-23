using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Drawing;
using Amphibian.Geometry;
using Amphibian.Collision.BroadPhase;
using Amphibian.Utility;

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

    

    public class CollisionManager
    {
        private SelectiveSweep _sweep;

        // XXX: Implement list pool
        private ResourcePool<List<ICollidable>> _listPool;
        private Dictionary<ICollidable, List<ICollidable>> _candidatePairs;
        private List<ICollidable> _empty;

        public CollisionManager ()
        {
            _listPool = new ResourcePool<List<ICollidable>>();
            _candidatePairs = new Dictionary<ICollidable, List<ICollidable>>();
            _empty = new List<ICollidable>();

            _sweep = new SelectiveSweep();
            _sweep.Collision += CollisionHandler;
        }

        public void Update ()
        {
            Reset();
            _sweep.Detect();
        }

        public void Reset ()
        {
            foreach (List<ICollidable> list in _candidatePairs.Values) {
                list.Clear();
                _listPool.ReturnResource(list);
            }

            _candidatePairs.Clear();
        }

        public void AddObject (ICollidable obj)
        {
            _sweep.AddCollidable(obj);
        }

        public void RemoveObject (ICollidable obj)
        {
            _sweep.RemoveCollidable(obj);
        }

        public List<ICollidable> CandidatePairs (ICollidable obj)
        {
            List<ICollidable> list;
            if (_candidatePairs.TryGetValue(obj, out list))
                return list;

            return _empty;
        }

        private void CollisionHandler (ICollidable first, ICollidable second)
        {
            AddObjectToList(first, second);
            AddObjectToList(second, first);
        }

        private void AddObjectToList (ICollidable first, ICollidable second)
        {
            List<ICollidable> list;
            if (!_candidatePairs.TryGetValue(first, out list)) {
                list = _listPool.TakeResource();
                _candidatePairs.Add(first, list);
            }

            list.Add(second);
        }
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
            SharedPointFP position = new SharedPointFP(x * _tileWidth, y * _tileHeight);

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
                if (x >= _width || x < 0)
                    continue;
                for (int y = minYId; y <= maxYId; y++) {
                    if (y >= _height || y < 0)
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

        public bool OverlapsEdgeAny (Mask mask)
        {
            RectangleFP rect = mask.Bounds;

            int minXId = (int)(rect.Left / _tileWidth);
            int maxXId = (int)(rect.Right / _tileWidth);
            int minYId = (int)(rect.Top / _tileHeight);
            int maxYId = (int)(rect.Bottom / _tileHeight);

            for (int x = minXId; x <= maxXId; x++) {
                if (x >= _width || x < 0)
                    continue;
                for (int y = minYId; y <= maxYId; y++) {
                    if (y >= _height || y < 0)
                        continue;

                    if (_grid[x, y] == null) {
                        continue;
                    }

                    if (_grid[x, y].TestOverlapEdge(mask)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool OverlapsAny (FPInt x, FPInt y)
        {
            int txId = (int)(x.Floor / _tileWidth);
            int tyId = (int)(y.Floor / _tileHeight);

            if (txId < 0 || txId >= _width || tyId < 0 || tyId >= _height) {
                return false;
            }

            if (_grid[txId, tyId] == null) {
                return false;
            }

            return _grid[txId, tyId].TestOverlap(x, y);
        }

        public bool OverlapsEdgeAny (FPInt x, FPInt y)
        {
            int txId = (int)(x.Floor / _tileWidth);
            int tyId = (int)(y.Floor / _tileHeight);

            if (txId < 0 || txId >= _width || tyId < 0 || tyId >= _height) {
                return false;
            }

            if (_grid[txId, tyId] == null) {
                return false;
            }

            return _grid[txId, tyId].TestOverlapEdge(x, y);
        }

        public bool OverlapsAny (AXLine line)
        {
            int minX = Math.Max(0, (int)(line.Left / _tileWidth));
            int maxX = Math.Min(_width - 1, (int)(line.Right / _tileWidth));
            int y = Math.Max(0, Math.Min(_height - 1, (int)(line.Y.Floor / _tileHeight)));

            for (int x = minX; x <= maxX; x++) {
                if (_grid[x, y] != null && _grid[x, y].TestOverlap(line))
                    return true;
            };

            return false;
        }

        public bool OverlapsEdgeAny (AXLine line)
        {
            int minX = Math.Max(0, (int)(line.Left / _tileWidth));
            int maxX = Math.Min(_width - 1, (int)(line.Right / _tileWidth));
            int y = Math.Max(0, Math.Min(_height - 1, (int)(line.Y.Floor / _tileHeight)));

            for (int x = minX; x <= maxX; x++) {
                if (_grid[x, y] != null && _grid[x, y].TestOverlapEdge(line))
                    return true;
            };

            return false;
        }

        public bool OverlapsAny (AYLine line)
        {
            int x = Math.Max(0, Math.Min(_width - 1, (int)(line.X.Floor / _tileWidth)));
            int minY = Math.Max(0, (int)(line.Top / _tileHeight));
            int maxY = Math.Min(_height - 1, (int)(line.Bottom / _tileHeight));
            

            for (int y = minY; y <= maxY; y++) {
                if (_grid[x, y] != null && _grid[x, y].TestOverlap(line))
                    return true;
            };

            return false;
        }

        public bool OverlapsEdgeAny (AYLine line)
        {
            int x = Math.Max(0, Math.Min(_width - 1, (int)(line.X.Floor / _tileWidth)));
            int minY = Math.Max(0, (int)(line.Top / _tileHeight));
            int maxY = Math.Min(_height - 1, (int)(line.Bottom / _tileHeight));


            for (int y = minY; y <= maxY; y++) {
                if (_grid[x, y] != null && _grid[x, y].TestOverlapEdge(line))
                    return true;
            };

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
