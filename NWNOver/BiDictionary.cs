using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver
{
    public class BiDictionary<T1, T2>
    {
        private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public BiDictionary()
        {
            this.Forward = new Indexer<T1, T2>(_forward);
            this.Reverse = new Indexer<T2, T1>(_reverse);
        }

        public BiDictionary(Dictionary<T1, T2> dictionary) : this()
        {
            if (HasDuplicates(dictionary))
                throw new ArgumentException("Dictionary contains duplicate values.");
            foreach (var item in dictionary)
            {
                _forward.Add(item.Key, item.Value);
                _reverse.Add(item.Value, item.Key);
            }
        }

        public BiDictionary(Dictionary<T2, T1> dictionary) : this()
        {
            if (HasDuplicates(dictionary))
                throw new ArgumentException("Dictionary contains duplicate values.");
            foreach (var item in dictionary.ToDictionary(pair => pair.Value, pair => pair.Key))
            {
                _forward.Add(item.Key, item.Value);
                _reverse.Add(item.Value, item.Key);
            }
        }

        static bool HasDuplicates<T3, T4>(Dictionary<T3, T4> dictionary)
        {
            return dictionary.GroupBy(pair => pair.Value).Any(group => group.Count() > 1);
        }

        public class Indexer<T3, T4>
        {
            private Dictionary<T3, T4> _dictionary;
            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }
            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }
        }

        public int Count
        {
            get
            {
                return _forward.Count;
            }
        }

        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public void RemoveForward(T1 key)
        {
            Remove(key, Forward[key]);
        }

        public void RemoveReverse(T2 key)
        {
            Remove(Reverse[key], key);
        }

        private void Remove(T1 t1, T2 t2)
        {
            _forward.Remove(t1);
            _reverse.Remove(t2);
        }

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }

        public Dictionary<T1, T2> ToDictionaryForward()
        {
            return new Dictionary<T1, T2>(_forward);
        }

        public Dictionary<T2, T1> ToDictionaryReverse()
        {
            return new Dictionary<T2, T1>(_reverse);
        }
    }
}
