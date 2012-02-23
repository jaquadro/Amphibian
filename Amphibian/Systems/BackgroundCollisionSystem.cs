using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Collision;
using Amphibian.EntitySystem;
using Amphibian.Geometry;

namespace Amphibian.Systems
{
    public class BackgroundCollisionSystem : BaseSystem
    {
        private TileCollisionManager _tileMan;

        public BackgroundCollisionSystem ()
        {
        }

        public BackgroundCollisionSystem (TileCollisionManager tileMan)
            : this()
        {
            _tileMan = tileMan;
        }

        public TileCollisionManager TileCollisionManager
        {
            get { return _tileMan; }
            set { _tileMan = value; }
        }

        public bool Test (FPInt x, FPInt y)
        {
            if (_tileMan == null)
                return false;

            return _tileMan.OverlapsAny(x, y);
        }

        public bool Test (AXLine line)
        {
            if (_tileMan == null)
                return false;

            return _tileMan.OverlapsAny(line);
        }

        public bool Test (AYLine line)
        {
            if (_tileMan == null)
                return false;

            return _tileMan.OverlapsAny(line);
        }

        public bool TestEdge (FPInt x, FPInt y)
        {
            if (_tileMan == null)
                return false;

            return _tileMan.OverlapsEdgeAny(x, y);
        }

        public bool TestEdge (AXLine line)
        {
            if (_tileMan == null)
                return false;

            return _tileMan.OverlapsEdgeAny(line);
        }

        public bool TestEdge (AYLine line)
        {
            if (_tileMan == null)
                return false;

            return _tileMan.OverlapsEdgeAny(line);
        }
    }
}
