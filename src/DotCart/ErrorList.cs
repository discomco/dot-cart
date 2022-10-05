namespace DotCart;

public class ErrorList : List<KeyValuePair<string, Error>>
{
    public void Add(string key, Error ex)
    {
        var element = new KeyValuePair<string, Error>(key, ex);
        base.Add(element);
    }

    public void Add(string key, Exception e)
    {
        Add(key, e.ToError());
    }

    public new void AddRange(IEnumerable<KeyValuePair<string, Error>> range)
    {
        base.AddRange(range);
    }
}