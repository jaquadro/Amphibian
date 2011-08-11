using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    // TODO: Generalize and move explicit to client code
    public class CollisionTileMapper
    {
        private Dictionary<int, Mask> _lookup;

        public CollisionTileMapper (float scalex, float scaley)
        {
            Init(scalex, scaley);
        }

        public Mask Lookup (int id, Vector2 position)
        {
            if (_lookup.ContainsKey(id) == false) {
                return null;
            }

            Mask m = _lookup[id].Clone() as Mask;
            m.Position = position;

            return m;
        }

        private void Init (float scalex, float scaley)
        {
            _lookup = new Dictionary<int, Mask>();

            // Row 1
            _lookup[1] = BuildRec(scalex, scaley, 0, 0, 1, 1);
            _lookup[2] = BuildTri(scalex, scaley, 0, 1, 1, 1, 1, 0);
            _lookup[3] = BuildTri(scalex, scaley, 0, 0, 0, 1, 1, 1);
            _lookup[4] = BuildTri(scalex, scaley, 0, 0, 1, 1, 1, 0);
            _lookup[5] = BuildTri(scalex, scaley, 0, 0, 0, 1, 1, 0);
            _lookup[6] = BuildRec(scalex, scaley, 0, 0, 1, .5f);
            _lookup[7] = BuildRec(scalex, scaley, 0, .5f, 1, 1);

            // Row2
            _lookup[8] = BuildRec(scalex, scaley, 0, 0, .5f, 1);
            _lookup[9] = BuildRec(scalex, scaley, .5f, 0, 1, 1);
            _lookup[10] = BuildTri(scalex, scaley, 0, 1, 1, 1, 1, .5f);
            _lookup[11] = BuildCom(
                BuildTri(scalex, scaley, 0, .5f, 1, .5f, 1, 0),
                BuildRec(scalex, scaley, 0, .5f, 1, 1));
            _lookup[12] = BuildCom(
                BuildTri(scalex, scaley, 0, 0, 0, .5f, 1, .5f),
                BuildRec(scalex, scaley, 0, .5f, 1, 1));
            _lookup[13] = BuildTri(scalex, scaley, 0, .5f, 0, 1, 1, 1);
            _lookup[14] = BuildTri(scalex, scaley, 0, .5f, 1, .5f, 1, 0);
            _lookup[15] = BuildTri(scalex, scaley, 0, 0, 0, .5f, 1, .5f);
        }

        private Mask BuildRec (float scalex, float scaley, float x1, float y1, float x2, float y2)
        {
            return new AABBMask(new Vector2(scalex * x1, scaley * y1), new Vector2(scalex * x2, scaley * y2));
        }

        private Mask BuildTri (float scalex, float scaley, float x1, float y1, float x2, float y2, float x3, float y3)
        {
            return new TriangleMask(new Vector2(scalex * x1, scaley * y1), 
                new Vector2(scalex * x2, scaley * y2), 
                new Vector2(scalex * x3, scaley * y3));
        }

        private Mask BuildCom (Mask m1, Mask m2)
        {
            CompositeMask com = new CompositeMask(m1);
            com.Add(m2);
            return com;
        }
    }
}
