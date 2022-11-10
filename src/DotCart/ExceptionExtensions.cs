namespace DotCart;

public static class ExceptionExtensions
{
    public static string InnerAndOuter(this Exception ex)
    {
        var result = ex.Message;
        if (ex.InnerException != null) result += $"\n -> {ex.InnerException.InnerAndOuter()}";
        return result;
    }

}