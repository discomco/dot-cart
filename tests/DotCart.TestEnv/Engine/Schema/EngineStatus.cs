namespace DotCart.TestEnv.Engine.Schema;

[Flags]
public enum EngineStatus
{
    Unknown = 0,
    Initialized = 1,
    Started = 2,
    Stopped = 4,
    Overheated = 8
}