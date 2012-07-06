using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Common {
    public static class DictionaryExtensions {
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) {
            return (dictionary as ReadOnlyDictionary<TKey, TValue>) ?? new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
