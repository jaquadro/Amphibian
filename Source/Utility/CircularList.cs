using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Utility
{
    public class CircularList<T> : IList<T>
    {
        private T[] _list;

        private int _index;

        public CircularList (int capacity)
        {
            _list = new T[capacity];
        }

        private int TranslateIndex (int index)
        {
            index += _index;
            if (index >= _list.Length)
                index -= _list.Length;
            return index;
        }

        #region IList<T> Members

        public int IndexOf (T item)
        {
            for (int i = 0; i < _list.Length; i++) {
                if (_list[TranslateIndex(i)].Equals(item))
                    return TranslateIndex(i);
            }

            return -1;
        }

        public void Insert (int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt (int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add (T item)
        {
            throw new NotImplementedException();
        }

        public void Clear ()
        {
            throw new NotImplementedException();
        }

        public bool Contains (T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo (T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove (T item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator ()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
