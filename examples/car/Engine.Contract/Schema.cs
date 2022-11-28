using DotCart.Abstractions.Schema;

namespace Engine.Contract;

public static class Schema
{
    [Flags]
    public enum EngineStatus
    {
        Unknown = 0,
        Initialized = 1,
        Started = 2,
        Stopped = 4,
        Overheated = 8
    }

    public const string EngineIDPrefix = "engine";
    public static readonly IDCtorT<EngineID> IDCtor = () => new EngineID();
        

    public static EngineStatus SetFlag(this EngineStatus status, EngineStatus flag)
    {
        ((int)status).SetFlag((int)flag);
        return status;
    }

    public static EngineStatus UnsetFlag(this EngineStatus status, EngineStatus flag)
    {
        ((int)status).UnsetFlag((int)flag);
        return status;
    }

    public static bool HasFlagFast(this EngineStatus value, EngineStatus flag)
    {
        return (value & flag) != 0;
    }

    [IDPrefix(EngineIDPrefix)]
    public record EngineID : ID
    {
        public EngineID(string value = "") : base(IDPrefixAtt.Get<EngineID>(), value)
        {
        }

        public static EngineID New()
        {
            return new EngineID();
        }
    }

    public record Details(string Name = "new engine", string Description = "")
    {
        public static Details New(string name, string description = "")
        {
            return new Details(name, description);
        }
    }
}