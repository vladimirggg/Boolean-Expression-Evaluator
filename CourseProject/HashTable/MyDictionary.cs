using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CourseProject.HashTable{
    internal class MyDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
    {
        private readonly IEqualityComparer<TKey> _equalityComparer;
        private readonly List<KeyValuePair<TKey, TValue>>[] _items;
        private const int DefaultCapacity = 16;

        public MyDictionary() : this(DefaultCapacity, EqualityComparer<TKey>.Default) { }

        public MyDictionary(int capacity, IEqualityComparer<TKey> equalityComparer)
        {
            _equalityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
            _items = new List<KeyValuePair<TKey, TValue>>[capacity];
            for (int i = 0; i < capacity; i++)
            {
                _items[i] = [];
            }
            Keys = [];
            Values = [];
        }

        public TValue this[TKey key]
        {
            get
            {
                var hash = _equalityComparer.GetHashCode(key);
                hash = Math.Abs(hash);
                hash %= _items.Length;
                var list = _items[hash];

                if (list is null || list.Count == 0)
                    throw new KeyNotFoundException();

                foreach (var item in list)
                {
                    if (_equalityComparer.Equals(item.Key, key))
                        return item.Value;
                }

                throw new KeyNotFoundException();
            }
            set
            {
                var hash = _equalityComparer.GetHashCode(key);
                hash = Math.Abs(hash);
                hash %= _items.Length;
                var list = _items[hash];

                for (int i = 0; i < list.Count; i++)
                {
                    if (_equalityComparer.Equals(list[i].Key, key))
                    {
                        list[i] = new KeyValuePair<TKey, TValue>(key, value);
                        return;
                    }
                }

                list.Add(new KeyValuePair<TKey, TValue>(key, value));
                ((List<TKey>)Keys).Add(key);
                ((List<TValue>)Values).Add(value);
                Count++;
            }
        }

        public ICollection<TKey> Keys { get; }

        public ICollection<TValue> Values { get; }

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            this[key] = value;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            foreach (var list in _items)
            {
                list.Clear();
            }
            ((List<TKey>)Keys).Clear();
            ((List<TValue>)Values).Clear();
            Count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return TryGetValue(item.Key, out var value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return TryGetValue(key, out _);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("The array is too small.");

            foreach (var list in _items)
            {
                foreach (var item in list)
                {
                    array[arrayIndex++] = item;
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var list in _items)
            {
                foreach (var item in list)
                {
                    yield return item;
                }
            }
        }

        public bool Remove(TKey key)
        {
            var hash = _equalityComparer.GetHashCode(key);
            hash = Math.Abs(hash);
            hash %= _items.Length;
            var list = _items[hash];

            for (int i = 0; i < list.Count; i++)
            {
                if (_equalityComparer.Equals(list[i].Key, key))
                {
                    list.RemoveAt(i);
                    ((List<TKey>)Keys).Remove(key);
                    ((List<TValue>)Values).Remove(list[i].Value);
                    Count--;
                    return true;
                }
            }

            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            var hash = _equalityComparer.GetHashCode(key);
            hash = Math.Abs(hash);
            hash %= _items.Length;
            var list = _items[hash];

            foreach (var item in list)
            {
                if (_equalityComparer.Equals(item.Key, key))
                {
                    value = item.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}