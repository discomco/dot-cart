using Crayon;

namespace DotCart.Core;

public static class LogFormatExtensions
{
    public static string AsVerb(this string value)
    {
        return Output.Background.Blue(Output.Black($"::{value}::"));
    }

    public static string AsDo(this string value)
    {
        return Output.Background.Rgb(64, 64, 128, Output.Black($"\t::{value}::"));
    }

    public static string AsRunning(this string value)
    {
        return Output.Background.Rgb(16, 16, 128, Output.Black($"::{value}::"));
    }

    public static string AsEnforcing(this string value)
    {
        return Output.Background.Rgb(8, 16, 136, Output.Black($"::{value}::"));
    }

    public static string AsEnforced(this string value)
    {
        return Output.Background.Rgb(136, 16, 8, Output.Black($"::{value}::"));
    }


    public static string AsRan(this string value)
    {
        return Output.Background.Rgb(128, 16, 16, Output.Black($"::{value}::"));
    }

    public static string AsDone(this string value)
    {
        return Output.Background.Rgb(128, 32, 32, Output.Black($"\t::{value}::"));
    }


    public static string AsSubject(this string value)
    {
        return Output.Background.White(Output.Black($"::{value}::"));
    }

    public static string AsFact(this string value)
    {
        return Output.Background.Rgb(22, 120, 120, Output.Yellow($"::{value}::"));
    }

    public static string AsError(this string value)
    {
        return Output.Background.Red(Output.Yellow($"::{value}::"));
    }
}

public static class AppVerbs
{
    public static readonly string Executing = "EXECUTING".AsVerb();
    public static readonly string Connecting = "CONNECTING".AsVerb();
    public static readonly string Starting = "STARTING".AsVerb();
    public static readonly string Stopping = "STOPPING".AsVerb();
    public static readonly string Activating = "ACTIVATING".AsVerb();
    public static readonly string Deactivating = "DEACTIVATING".AsVerb();
    public static readonly string Projecting = "PROJECTING".AsVerb();
    public static readonly string Waiting1s = "WAIT_1S".AsVerb();

    public static readonly string Subscribing = "SUBSCRIBING".AsVerb();
    public static readonly string Responding = "RESPONDING".AsVerb();
    public static readonly string Enforcing = "ENFORCING".AsEnforcing();
    public static readonly string Emitting = "EMITTING".AsVerb();
    public static readonly string Running = "RUNNING".AsRunning();
    public static readonly string Do = "DO".AsDo();

    public static string Looping(string activity, int counter)
    {
        return $"LOOPING({activity}, {counter})".AsVerb();
    }
}

public static class AppErrors
{
    public static string Error(string error)
    {
        return "ERROR".AsError() + $"~> ({error})";
    }
}

public static class AppFacts
{
    public static readonly string Connected = "CONNECTED".AsFact();
    public static readonly string Started = "STARTED".AsFact();
    public static readonly string Stopped = "STOPPED".AsFact();
    public static readonly string Activated = "ACTIVATED".AsFact();
    public static readonly string Deactivated = "DEACTIVATED".AsFact();
    public static readonly string Subscribed = "SUBSCRIBED".AsFact();
    public static readonly string Received = "RECEIVED".AsFact();
    public static readonly string Responded = "RESPONDED".AsFact();
    public static readonly string Cancelled = "CANCELLED".AsFact();
    public static readonly string Ran = "RAN".AsRan();
    public static readonly string Executed = "EXECUTED".AsFact();
    public static readonly string Done = "DONE".AsDone();
    public static readonly string Enforced = "ENFORCED".AsEnforced();
}