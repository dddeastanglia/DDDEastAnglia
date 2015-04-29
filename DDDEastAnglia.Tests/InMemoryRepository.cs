using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DDDEastAnglia.Tests
{
    internal class InMemoryRepository<T> where T : class
    {
        private readonly Func<T, int> primaryKeyFinder;
        private readonly ConcurrentDictionary<int, T> store = new ConcurrentDictionary<int, T>();

        public InMemoryRepository(Func<T, int> primaryKeyFinder)
        {
            if (primaryKeyFinder == null)
            {
                throw new ArgumentNullException("primaryKeyFinder");
            }

            this.primaryKeyFinder = primaryKeyFinder;
        }

        public void Save(T entity)
        {
            var primaryKey = primaryKeyFinder(entity);
            if (primaryKey == 0)
            {
                primaryKey = store.Count + 1;
            }
            store.AddOrUpdate(primaryKey, e => entity, (k, e) => entity);
        }

        public void Delete(int key)
        {
            T value;
            store.TryRemove(key, out value);
        }

        public IEnumerable<T> GetAll()
        {
            return store.Values;
        }

        public T Get(int key)
        {
            return store.ContainsKey(key) ? store[key] : default(T);
        }
    }
}