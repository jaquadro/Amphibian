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
        //private TileCollisionManager _tileMan;

        private List<TileCollisionManager> _managers;

        public BackgroundCollisionSystem ()
        {
            _managers = new List<TileCollisionManager>();
        }

        public BackgroundCollisionSystem (TileCollisionManager tileMan)
            : this()
        {
            //_tileMan = tileMan;
            _managers.Add(tileMan);
        }

        public void AddCollisionManager (TileCollisionManager manager)
        {
            _managers.Add(manager);
        }

        public void RemoveCollisionManager (TileCollisionManager manager)
        {
            _managers.Remove(manager);
        }

        public void ClearCollisionManagers ()
        {
            _managers.Clear();
        }

        /*public TileCollisionManager TileCollisionManager
        {
            get { return _tileMan; }
            set { _tileMan = value; }
        }*/

        public bool Test (FPInt x, FPInt y)
        {
            foreach (TileCollisionManager manager in _managers) {
                if (manager.OverlapsAny(x, y))
                    return true;
            }

            return false;
        }

        public bool Test (AXLine line)
        {
            foreach (TileCollisionManager manager in _managers) {
                if (manager.OverlapsAny(line))
                    return true;
            }

            return false;
        }

        public bool Test (AYLine line)
        {
            foreach (TileCollisionManager manager in _managers) {
                if (manager.OverlapsAny(line))
                    return true;
            }

            return false;
        }

        public bool TestEdge (FPInt x, FPInt y)
        {
            foreach (TileCollisionManager manager in _managers) {
                if (manager.OverlapsEdgeAny(x, y))
                    return true;
            }

            return false;
        }

        public bool TestEdge (AXLine line)
        {
            foreach (TileCollisionManager manager in _managers) {
                if (manager.OverlapsEdgeAny(line))
                    return true;
            }

            return false;
        }

        public bool TestEdge (AYLine line)
        {
            foreach (TileCollisionManager manager in _managers) {
                if (manager.OverlapsEdgeAny(line))
                    return true;
            }

            return false;
        }
    }
}
