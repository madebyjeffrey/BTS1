using System.Collections;

namespace BTS1;

public class MultiValueDictionary<TKey, TValue> : IEnumerable where TKey : notnull
{
    private readonly Dictionary<TKey, IEnumerable<TValue>> _data = new Dictionary<TKey, IEnumerable<TValue>>();

    public IEnumerable<TValue> Values => _data.Values.SelectMany(x => x);

    public IEnumerable<TKey> Keys => _data.Keys;

    public int Count => _data.Count;

    public IEnumerable<TValue> this[TKey key]
    {
        get => _data[key];
        set => AddOrUpdate(key, value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void AddOrUpdate(TKey key, IEnumerable<TValue> values)
    {
        if (ContainsKey(key)) // if key exists
        {
            (_data[key] as List<TValue>)?.AddRange(values);                
        }
        else
        {
            // add the new key with its value.
            _data.Add(key, values);
        }
    }

    public void AddOrUpdate(TKey key, TValue value)
    {
        if (ContainsKey(key)) // if key exists
        {
            //check value and add it if not exists
            if (!_data[key].Contains(value)) { (_data[key] as List<TValue>)?.Add(value); }
        }
        else
        {
            // add the new key with its value.
            _data.Add(key, new List<TValue>() { value });
        }
    }

    public void Clear(TKey key)
    {
        if (ContainsKey(key))
        {
            (_data[key] as List<TValue>)?.Clear();
        }
    }

    public bool TryGetValue(TKey key, out IEnumerable<TValue> values) => _data.TryGetValue(key, out values!);

    // ReSharper disable once MemberCanBePrivate.Global
    public bool ContainsKey(TKey key) => _data.ContainsKey(key);

    public bool RemoveKey(TKey key) => _data.Remove(key);

    public bool RemoveValue(TKey key, TValue value) => ContainsKey(key) && (_data[key] as List<TValue>)?.Remove(value) == true;

    public void Clean()
    {
        foreach (var item in _data.Where(x => !x.Value.Any()))
        {
            _data.Remove(item.Key);
        }
    }
    // enabling foreach loop
    public IEnumerator GetEnumerator() => _data.GetEnumerator();
}