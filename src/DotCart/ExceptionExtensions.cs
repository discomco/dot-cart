namespace DotCart;

public static class ExceptionExtensions
{
    public static Error ToError(this Exception e)
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
            res.InnerError = e.InnerException.ToError();
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