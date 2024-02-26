using Serilog.Core;
using Serilog.Events;

namespace DotCart.Core.Logging;

public class UtcTimestampEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory lepf)
    {
        logEvent.AddPropertyIfAbsent(
            lepf.CreateProperty("UtcOffset", logEvent.Timestamp.Offset));
        logEvent.AddPropertyIfAbsent(
            lepf.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
    }
}