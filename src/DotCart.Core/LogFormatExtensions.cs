using Crayon;

namespace DotCart.Core;

public static class LogFormatExtensions
{
    public static string AsVerb(this string value)
        => Output.Background.Blue(Output.Black($"::{value}::"));

    public static string AsSubject(this string value)
        => Output.Background.White(Output.Black($"::{value}::"));

    public static string AsFact(this string value)
        => Output.Background.Green(Output.White($"::{value}::"));

    public static string AsError(this string value)
        => Output.Background.Red(Output.Yellow($"::{value}::"));
}

public static class AppVerbs
{
    public static string Connecting = "CONNECTING".AsVerb();
    public static string Starting = "STARTING".AsVerb();
    public static string Stopping = "STOPPING".AsVerb();
    public static string Activating = "ACTIVATING".AsVerb();
    public static string Deactivating = "DEACTIVATING".AsVerb();
}

public static class AppFacts
{
    public static string Connected = "CONNECTED".AsFact();
    public static string Started = "STARTED".AsFact();
    public static string Stopped = "STOPPED".AsFact();
    public static string Activated = "ACTIVATED".AsFact();
    public static string Deactivated = "DEACTIVATED".AsFact();
}