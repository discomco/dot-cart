namespace DotCart.Abstractions.Drivers;

public record AppendResult
{
    private AppendResult(ulong nextExpectedVersion)
    {
        NextExpectedVersion = nextExpectedVersion;
    }

    public ulong NextExpectedVersion { get; }

    public static AppendResult New(ulong nextExpectedVersion)
    {
        return new AppendResult(nextExpectedVersion);
    }
}