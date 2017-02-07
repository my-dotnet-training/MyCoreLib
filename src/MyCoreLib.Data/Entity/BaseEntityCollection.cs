using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyCoreLib.Data.Entity
{
    /// <summary>
    /// A collection of entities who have integer ids.
    /// </summary>
    public class BaseEntityCollection<T> : IEnumerable<T> where T : class, IIntegerIdEntity
    {
        private IReadOnlyDictionary<int, T> _idMap;
        private IReadOnlyList<T> _list;

        public BaseEntityCollection(IEnumerable<T> query, int capacity)
        {
            var idMap = new Dictionary<int, T>();
            var list = new List<T>(Math.Max(0, capacity));

            if (query != null)
            {
                foreach (T entity in query)
                {
                    idMap.Add(entity.Id, entity);
                    list.Add(entity);
                }
            }

            _idMap = new ReadOnlyDictionary<int, T>(idMap);
            _list = list.AsReadOnly();
        }

        public BaseEntityCollection(IList<T> list)
            : this(list, list == null ? 0 : list.Count)
        { }

        public T GetEntity(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id");

            T entity;
            if (_idMap.TryGetValue(id, out entity))
                return entity;
            return null;
        }

        /// <summary>
        /// Get all the cached entities.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
