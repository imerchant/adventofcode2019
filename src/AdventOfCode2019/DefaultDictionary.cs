using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class DefaultDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _defaultFactory;
        private readonly Dictionary<TKey, TValue> _backingDictionary;

        public TValue this[TKey key]
        {
            get
            {
                TryGetValue(key, out TValue value);
                return value;
            }
            set => _backingDictionary[key] = value;
        }

        public ICollection<TKey> Keys => _backingDictionary.Keys;

        public ICollection<TValue> Values => _backingDictionary.Values;

        public int Count => _backingDictionary.Count;
        public bool IsReadOnly => ((IDictionary<TKey, TValue>)_backingDictionary).IsReadOnly;

        public DefaultDictionary(Func<TValue> defaultFactory) : this(0, _ => defaultFactory())
        {
        }

        public DefaultDictionary(int capacity, Func<TValue> defaultFactory) : this(capacity, _ => defaultFactory())
        {
        }

        public DefaultDictionary(Func<TKey, TValue> defaultFactory) : this(0, defaultFactory)
        {
        }

        public DefaultDictionary(int capacity, Func<TKey, TValue> defaultFactory)
        {
            if (defaultFactory == null)
                throw new ArgumentNullException(nameof(defaultFactory));

            _defaultFactory = defaultFactory;
            _backingDictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public void Add(TKey key, TValue value)
        {
            _backingDictionary.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)_backingDictionary).Add(item);
        }

        public void Clear()
        {
            _backingDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_backingDictionary).Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _backingDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_backingDictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _backingDictionary.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return _backingDictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_backingDictionary).Remove(item);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var result = _backingDictionary.TryGetValue(key, out value);
            if (!result)
            {
                _backingDictionary[key] = value = _defaultFactory(key);
            }
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}