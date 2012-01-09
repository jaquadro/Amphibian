using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Utility
{
    public class UnorderedList<T> : ICollection<T>
    {
        private const int _maxExpansionIncrement = 512;

        private T[] _items;
        private int _index;
        private IEqualityComparer<T> _comparer;

        public UnorderedList ()
            : this(4)
        {
        }

        public UnorderedList (int initialCapacity)
            : this(initialCapacity, EqualityComparer<T>.Default)
        {
        }

        public UnorderedList (IEqualityComparer<T> comparer)
            : this(4, comparer)
        {
        }

        public UnorderedList (int initialCapacity, IEqualityComparer<T> comparer)
        {
            _index = 0;
            _items = new T[initialCapacity];
            _comparer = comparer;
        }

        public int Capacity
        {
            get { return _items.Length; }
        }

        public T this[int index]
        {
            get
            {
                //if (index < 0 || index >= _index)
                //    throw new IndexOutOfRangeException();
                return _items[index];
            }
        }

        public int IndexOf (T item)
        {
            for (int i = 0; i < _index; i++)
                if (_comparer.Equals(_items[i], item))
                    return i;
            return -1;
        }

        public void Add (T item, out int index)
        {
            Add(item);
            index = _index - 1;
        }

        public void Set (int index, T value)
        {
            if (index < 0)
                throw new IndexOutOfRangeException();
            if (index >= _items.Length) {
                ExpandMin(index + 1);
                _index = index + 1;
            }
            else if (index >= _index)
                _index = index + 1;

            _items[index] = value;
        }

        public void RemoveAt (int index)
        {
            if (index < 0 || index >= _index)
                throw new ArgumentOutOfRangeException("index");

            _items[index] = _items[_index--];
            _items[_index] = default(T);
        }

        public void RemoveAll (T item)
        {
            for (int i = 0; i < _index; i++) {
                if (_comparer.Equals(_items[i], item)) {
                    _items[i] = _items[_index--];
                    _items[_index] = default(T);
                }
            }
        }

        public int NextIndex ()
        {
            return _index;
        }

        private void Expand ()
        {
            if (_items.Length < _maxExpansionIncrement) {
                Expand(_items.Length * 2);
            }
            else {
                Expand(_items.Length + _maxExpansionIncrement);
            }
        }

        private void ExpandMin (int minSize)
        {
            if (minSize >= _maxExpansionIncrement) {
                Expand(minSize + _maxExpansionIncrement - (minSize % _maxExpansionIncrement));
            }
            else {
                int sz = _items.Length;
                while (sz < minSize)
                    sz <<= 1;
                Expand(sz);
            }
        }

        private void Expand (int newSize)
        {
            if (newSize < _items.Length) {
                throw new Exception();
            }

            T[] items = new T[newSize];
            Array.Copy(_items, items, _items.Length);
            _items = items;
        }

        #region ICollection<T> Members

        public void Add (T item)
        {
            if (_index == _items.Length)
                Expand();
            _items[_index++] = item;
        }

        public void Clear ()
        {
            for (int i = 0; i < _index; i++)
                _items[i] = default(T);
            _index = 0;
        }

        public bool Contains (T item)
        {
            for (int i = 0; i < _index; i++)
                if (_comparer.Equals(_items[i], item))
                    return true;
            return false;
        }

        public void CopyTo (T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, _items.Length);
        }

        public int Count
        {
            get { return _index; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove (T item)
        {
            for (int i = 0; i < _index; i++) {
                if (_comparer.Equals(_items[i], item)) {
                    _items[i] = _items[_index--];
                    _items[_index] = default(T);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator ()
        {
            for (int i = 0; i < _index; i++)
                yield return _items[i];
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
