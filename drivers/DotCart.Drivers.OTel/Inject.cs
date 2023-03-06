using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace DotCart.Drivers.OTel;

public static class Inject
{
    public static IServiceCollection AddJaeger(this IServiceCollection services)
    {
        var b = services.AddOpenTelemetry()
            .WithTracing(c => c.AddJaegerExporter(o =>
            {
                o.Protocol = JaegerExportProtocol.HttpBinaryThrift;
                o.HttpClientFactory = () =>
                {
                    var http = new HttpClient();
                    http.DefaultRequestHeaders.Add("X-DotCart", "value");
                    return http;
                };
            }));
        return services;
    }
}