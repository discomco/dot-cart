using System.Net;

namespace DotCart;

public record Error
{
    public string Subject { get; set; }
    public string Content { get; set; }

    [Obsolete("Use Content Instead")] public string Msg { get; set; }

    public string Stack { get; set; }
    public string Source { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string Server { get; set; }
    public string ProtocolVersion { get; set; }
    public string Method { get; set; }
    public DateTime LastModified { get; set; }
    public string StatusDescription { get; set; }
    public string ErrorMessage { get; set; }
    public string ResponseStatus { get; set; }
    public Error InnerError { get; set; }
}