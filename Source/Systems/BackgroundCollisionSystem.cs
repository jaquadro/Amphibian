using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Collision;
using Amphibian.EntitySystem;

namespace Amphibian.Systems
{
    public class BackgroundCollisionSystem : BaseSystem
    {
        private TileCollisionManager _tileMan;

        public BackgroundCollisionSystem ()
            : base(null)
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

        public bool Test (AXLine line)
        {
            if (_tileMan == null)
                return false;

            return _tileMan.OverlapsAny(line);
        }

        public bool TestEdge (AXLine line)
        {
            if (_tileMan == null)
                return false;

            return _tileMan.OverlapsEdgeAny(line);
        }
    }
}
