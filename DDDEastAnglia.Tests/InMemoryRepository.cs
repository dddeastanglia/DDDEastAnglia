using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DDDEastAnglia.Tests
{
    internal class InMemoryRepository<T>
    {
        private readonly Func<T, int> keyFunc;
        private readonly ConcurrentDictionary<int, T> store = new ConcurrentDictionary<int, T>();

        public InMemoryRepository(Func<T, int> keyFunc)
        {
            this.keyFunc = keyFunc;
        }

        public void Save(T entity)
        {
            var key = keyFunc(entity);
            if (key == 0)
            {
                key = store.Count + 1;
            }
            store.AddOrUpdate(key, e => entity, (k, e) => entity);
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