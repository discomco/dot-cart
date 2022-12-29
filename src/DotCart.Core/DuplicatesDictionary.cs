using System.Runtime.InteropServices.JavaScript;

namespace DotCart.Core;

public class DuplicatesDictionary<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
{
    public DuplicatesDictionary<TKey,TValue> Add(TKey key, TValue value)
    {
        var element = new KeyValuePair<TKey, TValue>(key, value);
        Add(element);
        return this;
    }

    public static readonly DuplicatesDictionary<string, TValue> Empty = new();

    public bool ContainsKey(TKey key)
    {
        return this.Any(item => item.Key.Equals(key));
    }
}