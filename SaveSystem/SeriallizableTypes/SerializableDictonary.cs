using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictonary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    public void OnAfterDeserialize()
    {
        Clear();
        if (keys.Count != values.Count)
            Debug.LogError(keys.Count + "no same" + values.Count);
        for (int i = 0; i < keys.Count; i++)
            Add(keys[i], values[i]);
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var item in this)
        {
            keys.Add(item.Key);
            values.Add(item.Value);
        }
    }
}