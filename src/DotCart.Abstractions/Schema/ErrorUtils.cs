namespace DotCart.Abstractions.Schema;

public static class ErrorUtils
{
    public static Error AsError(this Exception e)
    {
        var res = new Error
        {
            Subject = "Unexpected Error",
            ErrorMessage = e.Message,
            LastModified = DateTime.UtcNow,
            Source = e.Source,
            Content = e.Message,
            Stack = e.StackTrace
        };
        if (e.InnerException != null)
            res.InnerError = e.InnerException.AsError();
        return res;
    }


    public static IDictionary<string, Error> AddRange(this IDictionary<string, Error> target,
        IDictionary<string, Error> source)
    {
        if (source == null) return target;
        foreach (var it in source) target.Add(it);
        return target;
    }
}