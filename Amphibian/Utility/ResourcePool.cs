using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Utility
{
    public class ResourcePool<T>
        where T : new()
    {
        private Stack<T> _pool;
        private int _maxReserve;

        public ResourcePool ()
        {
            _maxReserve = int.MaxValue;
            _pool = new Stack<T>();
        }

        public int MaxReserve
        {
            get { return _maxReserve; }
            set { _maxReserve = value; }
        }

        public T TakeResource ()
        {
            if (_pool.Count == 0) {
                return new T();
            }

            return _pool.Pop();
        }

        public void ReturnResource (T list)
        {
            if (_pool.Count < _maxReserve)
                _pool.Push(list);
        }
    }
}
