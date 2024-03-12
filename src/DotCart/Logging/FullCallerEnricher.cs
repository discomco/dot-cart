using System.Diagnostics;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace DotCart.Logging;

public class FullCallerEnricher
    : ILogEventEnricher
{
    private const string TemplateVar = "FullCaller";

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var skip = 3;
        while (true)
        {
            var stack = new StackFrame(skip);
            if (!stack.HasMethod())
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty(TemplateVar, new ScalarValue("<unknown method>")));
                return;
            }

            var method = stack.GetMethod();
            if (method.DeclaringType.Assembly != typeof(Log).Assembly)
            {
                var caller = $"{method.DeclaringType.FullName}.{method.Name}({string.Join(", ", method.GetParameters().Select(pi => pi.ParameterType.FullName))})";
                logEvent.AddPropertyIfAbsent(new LogEventProperty(TemplateVar, new ScalarValue(caller)));
            }

            skip++;
        }
    }
}

static partial class LoggerCallerEnrichmentConfiguration
{
    public static LoggerConfiguration WithFullCaller(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        return enrichmentConfiguration.With<FullCallerEnricher>();
    }
}