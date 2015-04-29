using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DDDEastAnglia.Tests
{
    internal class InMemoryRepository<T> where T : class
    {
        private readonly Func<T, int> _primaryKeyFinder;
        private readonly ConcurrentDictionary<int, T> _store = new ConcurrentDictionary<int, T>();

        public InMemoryRepository(Func<T, int> primaryKeyFinder)
        {
            if (primaryKeyFinder == null)
            {
                throw new ArgumentNullException("primaryKeyFinder");
            }

            _primaryKeyFinder = primaryKeyFinder;
        }

        public void Save(T entity)
        {
            var primaryKey = _primaryKeyFinder(entity);
            if (primaryKey == 0)
            {
                primaryKey = _store.Count + 1;
            }
            _store.AddOrUpdate(primaryKey, e => entity, (k, e) => entity);
        }

        public void Delete(int key)
        {
            T value;
            _store.TryRemove(key, out value);
        }

        public IEnumerable<T> GetAll()
        {
            return _store.Values;
        }

        public T Get(int key)
        {
            return _store.ContainsKey(key) ? _store[key] : default(T);
        }
    }
}