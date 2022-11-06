namespace DotCart.Drivers.EventStoreDB;

public class AppendResult
{
    private AppendResult(long nextExpectedVersion)
    {
        NextExpectedVersion = nextExpectedVersion;
    }

    public long NextExpectedVersion { get; }

    public static AppendResult New(long nextExpectedVersion)
    {
        return new AppendResult(nextExpectedVersion);
    }
}