namespace DotCart.Core;

public class DuplicatesList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
{
    public void Add(TKey key, TValue value)
    {
        var element = new KeyValuePair<TKey, TValue>(key, value);
        Add(element);
    }
}