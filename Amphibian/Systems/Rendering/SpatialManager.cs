using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Systems.Rendering
{
    public sealed class SpatialManager
    {
        private struct SpatialTag
        {
            public readonly int Id;
            public readonly Spatial Spatial;

            public SpatialTag (int id, Spatial spatial)
            {
                Id = id;
                Spatial = spatial;
            }
        }

        private int _nextId = 0;

        private List<SpatialTag> _spatials;
        private Stack<int> _free;

        public SpatialManager ()
        {
            _spatials = new List<SpatialTag>();
            _free = new Stack<int>();
        }

        public SpatialRef Add (Spatial spatial)
        {
            int index = (_free.Count > 0)
                ? _free.Pop()
                : _spatials.Count;

            SpatialRef sr = new SpatialRef(NextId(), index);

            _spatials.Add(new SpatialTag(sr.Id, spatial));

            return sr;
        }

        public void Remove (SpatialRef sref)
        {
            if (IsValid(sref)) {
                _spatials[sref.Index] = new SpatialTag();
                _free.Push(sref.Index);
            }
        }

        public bool IsValid (SpatialRef sref)
        {
            if(sref.Index<_spatials.Count)
                return sref.Id == _spatials[sref.Index].Id;
            return false;
        }

        public Spatial GetSpatial (SpatialRef sref)
        {
            SpatialTag stag = _spatials[sref.Index];
            if (stag.Id == sref.Id) {
                return stag.Spatial;
            }

            return null;
        }

        public T GetSpatial<T> (SpatialRef sref)
            where T : Spatial
        {
            return GetSpatial(sref) as T;
        }

        private int NextId ()
        {
            return _nextId++;
        }
    }
}
