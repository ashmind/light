using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Common {
    // http://stackoverflow.com/questions/678379/is-there-a-read-only-generic-dictionary-available-in-net
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
        private const string DictionaryIsReadOnlyMessage = "Dictionary is read-only";
        private readonly IDictionary<TKey, TValue> dictionary;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary) {
            this.dictionary = dictionary;
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) {
            throw new NotSupportedException(DictionaryIsReadOnlyMessage);
        }

        public bool ContainsKey(TKey key) {
            return dictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys {
            get { return dictionary.Keys; }
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key) {
            throw new NotSupportedException(DictionaryIsReadOnlyMessage);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return dictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values {
            get { return dictionary.Values; }
        }

        public TValue this[TKey key] {
            get { return dictionary[key]; }
        }

        TValue IDictionary<TKey, TValue>.this[TKey key] {
            get { return this[key]; }
            set { throw new NotSupportedException(DictionaryIsReadOnlyMessage); }
        }

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        void ICollection<KeyValuePair<TKey,TValue>>.Add(KeyValuePair<TKey, TValue> item) {
            throw new NotSupportedException(DictionaryIsReadOnlyMessage);
        }

        void ICollection<KeyValuePair<TKey,TValue>>.Clear() {
            throw new NotSupportedException(DictionaryIsReadOnlyMessage);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            dictionary.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return dictionary.Count; }
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.IsReadOnly {
            get { return true; }
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.Remove(KeyValuePair<TKey, TValue> item) {
            throw new NotSupportedException(DictionaryIsReadOnlyMessage);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return (dictionary as System.Collections.IEnumerable).GetEnumerator();
        }

        #endregion
    }
}
