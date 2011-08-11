using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Amphibian.Geometry;

namespace Amphibian.Collision
{
    // TODO: Generalize and move explicit to client code
    public class CollisionTileMapper
    {
        private Dictionary<int, Mask> _lookup;

        public CollisionTileMapper (FPInt scalex, FPInt scaley)
        {
            Init(scalex, scaley);
        }

        public Mask Lookup (int id, PointFP position)
        {
            if (_lookup.ContainsKey(id) == false) {
                return null;
            }

            Mask m = _lookup[id].Clone() as Mask;
            m.Position = position;

            return m;
        }

        private void Init (FPInt scalex, FPInt scaley)
        {
            _lookup = new Dictionary<int, Mask>();

            FPInt h = (FPInt)0.5f;

            // Row 1
            _lookup[1] = BuildRec(scalex, scaley, 0, 0, 1, 1);
            _lookup[2] = BuildTri(scalex, scaley, 0, 1, 1, 1, 1, 0);
            _lookup[3] = BuildTri(scalex, scaley, 0, 0, 0, 1, 1, 1);
            _lookup[4] = BuildTri(scalex, scaley, 0, 0, 1, 1, 1, 0);
            _lookup[5] = BuildTri(scalex, scaley, 0, 0, 0, 1, 1, 0);
            _lookup[6] = BuildRec(scalex, scaley, 0, 0, 1, h);
            _lookup[7] = BuildRec(scalex, scaley, 0, h, 1, 1);

            // Row2
            _lookup[8] = BuildRec(scalex, scaley, 0, 0, h, 1);
            _lookup[9] = BuildRec(scalex, scaley, h, 0, 1, 1);
            _lookup[10] = BuildTri(scalex, scaley, 0, 1, 1, 1, 1, h);
            _lookup[11] = BuildCom(
                BuildTri(scalex, scaley, 0, h, 1, h, 1, 0),
                BuildRec(scalex, scaley, 0, h, 1, 1));
            _lookup[12] = BuildCom(
                BuildTri(scalex, scaley, 0, 0, 0, h, 1, h),
                BuildRec(scalex, scaley, 0, h, 1, 1));
            _lookup[13] = BuildTri(scalex, scaley, 0, h, 0, 1, 1, 1);
            _lookup[14] = BuildTri(scalex, scaley, 0, h, 1, h, 1, 0);
            _lookup[15] = BuildTri(scalex, scaley, 0, 0, 0, h, 1, h);
        }

        private Mask BuildRec (FPInt scalex, FPInt scaley, FPInt x1, FPInt y1, FPInt x2, FPInt y2)
        {
            return new AABBMask(new PointFP(scalex * x1, scaley * y1), new PointFP(scalex * x2, scaley * y2));
        }

        private Mask BuildTri (FPInt scalex, FPInt scaley, FPInt x1, FPInt y1, FPInt x2, FPInt y2, FPInt x3, FPInt y3)
        {
            return new TriangleMask(new PointFP(scalex * x1, scaley * y1), 
                new PointFP(scalex * x2, scaley * y2), 
                new PointFP(scalex * x3, scaley * y3));
        }

        private Mask BuildCom (Mask m1, Mask m2)
        {
            CompositeMask com = new CompositeMask(m1);
            com.Add(m2);
            return com;
        }
    }
}
