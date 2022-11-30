using System.Text.Json;
using DotCart.Core;

namespace DotCart.Abstractions.Schema;

[Serializable]
public sealed class Errors : DuplicatesList<string, Error>
{
    public void Add(string key, string message)
    {
        var err = new Error { ErrorMessage = message };
        Add(key, err);
    }

    public void Add(string key, Exception e)
    {
        Add(key, e.AsError());
    }

    public new void AddRange(IEnumerable<KeyValuePair<string, Error>> range)
    {
        base.AddRange(range);
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}