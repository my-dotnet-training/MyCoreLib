
namespace MyCoreLib.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public class CircularList<T> : IEnumerable<T>, ICollection
    {
        #region Private

        private int _size;
        private int _version;
        private bool _full;
        private int _pointer;
        private T[] _array;
        private object _syncRoot;

        #endregion Private

        #region Properties
        /// <summary>
        /// Return the total number of items stored in the list.
        /// </summary>
        public int Count
        {
            get { return _size; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }

                return _syncRoot;
            }
        }

#if DEBUG
        // Only for unit tests in debug builds.
        internal T[] InternalArray
        {
            get { return _array; }
        }

        internal int Version
        {
            get { return _version; }
        }

        internal bool Full
        {
            get { return _full; }
        }

        internal int Pointer
        {
            get { return _pointer; }
        }
#endif
        #endregion Properties

        #region Constructor

        public CircularList(int maxSlots)
        {
            if (maxSlots <= 0)
            {
                throw new ArgumentOutOfRangeException("maxSlots");
            }

            _array = new T[maxSlots];
            _pointer = 0;
            _full = false;
            _size = 0;
        }

        #endregion Constructor

        #region Methods
        /// <summary>
        /// Add an object to the end of the list.
        /// </summary>
        public void Append(T item)
        {
            _array[_pointer++] = item;
            _pointer %= _array.Length;
            if (!_full)
            {
                _full = (_pointer == 0);
                _size++;
            }

            _version++;
        }

        /// <summary>
        /// Returns the object at the end of the list.
        /// </summary>
        public T GetLastItem()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("An empty list is not allowed.");
            }
            int lastIndex = (_pointer - 1 + _array.Length) % _array.Length;
            return _array[lastIndex];
        }

        /// <summary>
        /// Copy the elements to a new array.
        /// </summary>
        public T[] ToArray()
        {
            T[] destArray = new T[_size];
            if (_size != 0)
            {
                if (_full)
                {
                    Array.Copy(_array, _pointer, destArray, 0, _array.Length - _pointer);
                    Array.Copy(_array, 0, destArray, _array.Length - _pointer, _pointer);
                }
                else
                {
                    Array.Copy(_array, 0, destArray, 0, _size);
                }
            }

            return destArray;
        }

        /// <summary>
        /// Returns an enumerator to iterate through the list.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            if (_full)
            {
                for (int i = _pointer; i < _array.Length; i++)
                {
                    yield return _array[i];
                }
                for (int i = 0; i < _pointer; i++)
                {
                    yield return _array[i];
                }
            }
            else
            {
                for (int i = 0; i < _pointer; i++)
                {
                    yield return _array[i];
                }
            }
        }

        /// <summary>
        /// Returns an enumerator to iterate through the list.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // We do not need this method
        [Obsolete("This method isn't implemented", true)]
        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        #endregion Methods
    }
}
