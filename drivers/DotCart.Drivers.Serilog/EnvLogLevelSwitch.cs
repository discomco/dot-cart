using Serilog.Core;
using Serilog.Events;

namespace DotCart.Drivers.Serilog;

public class EnvLogLevelSwitch : LoggingLevelSwitch
{
    public EnvLogLevelSwitch(string environmentVariable)
    {
        var level = LogEventLevel.Information;
        MinimumLevel = level;
        if (Enum.TryParse(DotEnv.Expand(EnVars.LOG_LEVEL_MIN), true, out level)) MinimumLevel = level;
    }
}