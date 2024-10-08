using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}


[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<KeyValuePair<TKey, TValue>> items = new List<KeyValuePair<TKey, TValue>>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public void OnBeforeSerialize()
    {
        items.Clear();
        foreach (var kvp in dictionary)
        {
            items.Add(new KeyValuePair<TKey, TValue> { Key = kvp.Key, Value = kvp.Value });
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        foreach (var kvp in items)
        {
            dictionary[kvp.Key] = kvp.Value;
        }
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return dictionary;
    }
}
