using System.Reflection;
using System.Runtime.CompilerServices;
using DotCart.Core;
using Serilog.Core;
using Serilog.Events;

namespace DotCart.Logging;

public class CVersionEnricher
    : ILogEventEnricher
{
    public const string PropertyName = "CVersion";
    private LogEventProperty? _cachedProperty;

    /// <summary>
    ///     Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
    }

    private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
    {
        // Don't care about thread-safety, in the worst case the field gets overwritten and one property will be GCed
        return _cachedProperty ??= CreateProperty(propertyFactory);
    }

    // Qualify as uncommon-path
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
    {
        var value = Assembly.GetCallingAssembly()?.GetVersion() ?? "unknown";
        return propertyFactory.CreateProperty(PropertyName, value);
    }
}