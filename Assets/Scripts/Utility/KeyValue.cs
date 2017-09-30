using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility {
    [Serializable]
    public class KeyValue<TKey, TValue> {
        public TKey Key;
        public TValue Value;
    }

    public static class KeyValueUtility {
        public static void FillDictionary<TKey, TValue>(IDictionary<TKey, TValue> dict,
            IEnumerable<KeyValue<TKey, TValue>> keyValues) {
            foreach (var kv in keyValues) {
                dict.Add(kv.Key, kv.Value);
            }
        }
    }
    
    [Serializable]
    public class KeyValueInt : KeyValue<string, int> {
    }

    [Serializable]
    public class KeyValueFloat : KeyValue<string, float> {
    }
    
    [Serializable]
    public class KeyValueString : KeyValue<string, string> {
    }
    
    [Serializable]
    public class KeyValueBool : KeyValue<string, bool> {
    }
}
