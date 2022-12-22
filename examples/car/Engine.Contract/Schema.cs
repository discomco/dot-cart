using System.Text.Json.Serialization;
using DotCart.Abstractions.Schema;
using DotCart.Core;

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

    public const string EngineListIDValue = "EE1129A0-605E-4FFD-835C-EB91B2C06174";
    public const string EngineListIDPrefix = "engine-lst";

    public const string EngineIDPrefix = "engine";

    public static readonly IDCtorT<EngineID>
        IDCtor =
            _ => new EngineID();


    public static EngineStatus SetFlag(this EngineStatus status, EngineStatus flag)
    {
        var newStatus = ((int)status).SetFlag((int)flag);
        return (EngineStatus)newStatus;
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

    [DbName(Constants.DocRedisDbName)]
    public record Engine : IState
    {
        public static readonly StateCtorT<Engine> Ctor = () => new Engine();

        public Engine()
        {
            Details = new Details();
            Status = EngineStatus.Unknown;
        }

        [JsonConstructor]
        public Engine(string id, EngineStatus status, Details details)
        {
            Id = id;
            Status = status;
            Details = details;
            Power = 0;
        }

        private Engine(string id)
        {
            Id = id;
            Status = EngineStatus.Unknown;
            Details = Details.New("New Engine");
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }

        public EngineStatus Status { get; set; }
        public int Power { get; set; }
        public Details Details { get; set; }

        public static Engine New(string id, EngineStatus status, Details details)
        {
            return new Engine(id, status, details);
        }
    }


    public record EngineListItem(string EngineId, string Name, EngineStatus Status, int Power);

    [IDPrefix(EngineListIDPrefix)]
    public record EngineListID : ID
    {
        public EngineListID() : base(EngineListIDPrefix, EngineListIDValue)
        {
        }
    }

    public record EngineList(List<EngineListItem> Items) : IState
    {
        public EngineListID ID { get; set; } = new();
        public List<EngineListItem> Items { get; } = Items;
    }

    [IDPrefix(EngineIDPrefix)]
    public record EngineID : ID
    {
        public EngineID(string value = "") : base(IDPrefixAtt.Get<EngineID>(), value)
        {
        }

        public static EngineID New(string value = "")
        {
            if (string.IsNullOrEmpty(value))
                value = GuidUtils.LowerCaseGuid;
            return new EngineID(value.ToLower());
        }
    }

    public record Details
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public static Details New(string name, string description = "")
        {
            return new Details
            {
                Name = name,
                Description = description
            };
        }
    }
}