using System.Text.Json;

namespace DotCart.Abstractions.Contract;

[Serializable]
public sealed class Errors : List<(string, Error)>
{
    public void Add(string key, string message)
    {
        var err = new Error { ErrorMessage = message };
        Add((key, err));
    }

    public void Add(string key, Exception e)
    {
        Add((key, e.AsError()));
    }

    public void AddRange(IEnumerable<KeyValuePair<string, Error>> range)
    {
        base.AddRange(range.Select(pair => (pair.Key, pair.Value)));
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}