using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Stats : CustomBehaviour {
    [SerializeField] private KeyValueInt[] Integers;
    [SerializeField] private KeyValueFloat[] Floats;
    [SerializeField] private KeyValueBool[] Bools;
    [SerializeField] private KeyValueString[] Strings;

    private readonly IDictionary<string, int> integers = new Dictionary<string, int>();
    private readonly IDictionary<string, float> floats = new Dictionary<string, float>();
    private readonly IDictionary<string, bool> bools = new Dictionary<string, bool>();
    private readonly IDictionary<string, string> strings = new Dictionary<string, string>();

    private void Awake() {
        KeyValueUtility.FillDictionary(integers, Integers);
        KeyValueUtility.FillDictionary(floats, Floats);
        KeyValueUtility.FillDictionary(bools, Bools);
        KeyValueUtility.FillDictionary(strings, Strings);
    }

    public int GetInt(string key, int defaultValue) {
        return GetValue(key, integers, defaultValue);
    }
    
    public float GetFloat(string key, float defaultValue) {
        return GetValue(key, floats, defaultValue);
    }
    
    public bool GetBool(string key, bool defaultValue) {
        return GetValue(key, bools, defaultValue);
    }
    
    public string GetString(string key, string defaultValue) {
        return GetValue(key, strings, defaultValue);
    }

    private static T GetValue<T>(string key, IDictionary<string, T> dict, T defaultValue) {
        T value;
        if (!dict.TryGetValue(key, out value)) {
            value = defaultValue;
        }
        return value;
    } 
}
