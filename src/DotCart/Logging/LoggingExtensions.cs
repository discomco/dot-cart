using Serilog;
using Serilog.Configuration;

namespace DotCart.Logging;

public static class LoggingExtensions
{
    public static LoggerConfiguration WithUtcTimeStamp(
        this LoggerEnrichmentConfiguration enrich)
    {
        if (enrich == null) throw new ArgumentNullException(nameof(enrich));

        return enrich.With<UtcTimestampEnricher>();
    }
}