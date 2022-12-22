using Crayon;

namespace DotCart.Core;

public static class LogFormatExtensions
{
    public static string AsVerb(this string value)
    {
        return Output.Background.Blue(Output.Black($"::{value}::"));
    }

    public static string AsSubject(this string value)
    {
        return Output.Background.White(Output.Black($"::{value}::"));
    }

    public static string AsFact(this string value)
    {
        return Output.Background.Green(Output.White($"::{value}::"));
    }

    public static string AsError(this string value)
    {
        return Output.Background.Red(Output.Yellow($"::{value}::"));
    }
}

public static class AppVerbs
{
    public static readonly string Connecting = "CONNECTING".AsVerb();
    public static readonly string Starting = "STARTING".AsVerb();
    public static readonly string Stopping = "STOPPING".AsVerb();
    public static readonly string Activating = "ACTIVATING".AsVerb();
    public static readonly string Deactivating = "DEACTIVATING".AsVerb();
    public static readonly string Projecting = "PROJECTING".AsVerb();
    public static readonly string Waiting1s = "WAIT_1S".AsVerb();
    public static readonly string Running = "RUNNING".AsVerb();
    public static readonly string Subscribing = "SUBSCRIBING".AsVerb();
    public static readonly string Responding = "RESPONDING".AsVerb();
    public static readonly string Enforcing = "ENFORCING".AsVerb();
}

public static class AppErrors
{
    public static readonly string Error = "ERROR".AsError();
}

public static class AppFacts
{
    public static string Connected = "CONNECTED".AsFact();
    public static readonly string Started = "STARTED".AsFact();
    public static readonly string Stopped = "STOPPED".AsFact();
    public static string Activated = "ACTIVATED".AsFact();
    public static string Deactivated = "DEACTIVATED".AsFact();
    public static readonly string Subscribed = "SUBSCRIBED".AsFact();
    public static readonly string Received = "RECEIVED".AsFact();
    public static readonly string Responded = "RESPONDED".AsFact();
}