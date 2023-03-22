using YamlDotNet.Core.Tokens;

namespace DotCart.Core;

public class DuplicatesDictionary<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
{


    public static readonly DuplicatesDictionary<string, TValue> Empty = new();

    public DuplicatesDictionary<TKey, TValue> Add(TKey key, TValue value)
    {
        var element = new KeyValuePair<TKey, TValue>(key, value);
        Add(element);
        return this;
    }

    // public bool ContainsKey(TKey key)
    // {
    //     var check = this with { };
    //     return this.Any(item => item.Key.Equals(key));
    // }
}