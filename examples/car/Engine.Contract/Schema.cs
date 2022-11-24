using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;

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

    public static NewID<EngineID> IDCtor => () => new EngineID();

    [IDPrefix(EngineIDPrefix)]
    public record EngineID : ID
    {
        public EngineID(string value = "") : base(EngineIDPrefix, value)
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