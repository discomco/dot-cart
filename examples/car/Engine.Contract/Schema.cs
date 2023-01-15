using System.Collections.Immutable;
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


    public static readonly ValueObjectCtorT<EngineListItem>
        EngineListItemCtor =
            () => new EngineListItem();

    public static readonly ValueObjectCtorT<Details>
        DetailsCtor =
            () => new Details();

    public static readonly IDCtorT<EngineID>
        RootIDCtor =
            _ => new EngineID();

    public static readonly IDCtorT<EngineListID>
        ListIDCtor =
            _ => new EngineListID();

    public static readonly StateCtorT<Engine>
        RootCtor =
            () => new Engine();

    public static readonly StateCtorT<EngineList>
        ListCtor =
            EngineList.New;


    private static readonly object _setFlagMutex = new();


    public static EngineStatus SetFlag(this EngineStatus status, EngineStatus flag)
    {
        lock (_setFlagMutex)
        {
            var newStatus = ((int)status).SetFlag((int)flag);
            return (EngineStatus)newStatus;
        }
    }

    public static EngineStatus UnsetFlag(this EngineStatus status, EngineStatus flag)
    {
        lock (_setFlagMutex)
        {
            var newStatus = ((int)status).UnsetFlag((int)flag);
            return (EngineStatus)newStatus;
        }
    }

    public static bool HasFlagFast(this EngineStatus value, EngineStatus flag)
    {
        return (value & flag) != 0;
    }

    public record Rpm(int Value) : IValueObject
    {
        public int Value { get; } = Value;

        public static Rpm? New(int value)
        {
            return new Rpm(value);
        }
    }

    [DbName(DbConstants.RedisDocDbName)]
    public record Engine : IState
    {
        public Engine()
        {
            Details = new Details();
            Status = EngineStatus.Unknown;
            Rpm = new Rpm(0);
        }

        [JsonConstructor]
        public Engine(string id, EngineStatus status, Details details, Rpm rpm)
        {
            Id = id;
            Status = status;
            Details = details;
            Rpm = rpm;
        }

        private Engine(string id)
        {
            Id = id;
            Status = EngineStatus.Unknown;
            Details = Details.New("New Engine");
            Rpm = Rpm.New(0);
        }

        public EngineStatus Status { get; set; }

        public Rpm Rpm { get; set; }
        public Details Details { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }

        public string Prev { get; set; }

        public static Engine New(string id, EngineStatus status, Details details, Rpm rpm)
        {
            return new Engine(id, status, details, rpm);
        }
    }


    public record EngineListItem : IValueObject, IEntityT<EngineID>, IState
    {
        public string Name { get; set; }
        public EngineStatus Status { get; set; }
        public int Power { get; set; }
        public string Id { get; set; }
        public string Prev { get; set; }

        public static EngineListItem New(string aggId, string name, EngineStatus status, int power)
        {
            return new EngineListItem
            {
                Id = aggId,
                Name = name,
                Status = status,
                Power = power
            };
        }
    }


    [IDPrefix(IDConstants.EngineListIDPrefix)]
    public record EngineListID() : IDB(IDConstants.EngineListIDPrefix, IDConstants.EngineListIDValue)
    {
        public static EngineListID New()
        {
            return new EngineListID();
        }
    }

    [DbName(DbConstants.RedisListDbName)]
    public record EngineList(ImmutableDictionary<string, EngineListItem> Items) : IListState
    {
        public ImmutableDictionary<string, EngineListItem> Items { get; set; } = Items;

        public string Id { get; }
        public string Prev { get; set; }

        public static EngineList New()
        {
            return new EngineList(ImmutableDictionary<string, EngineListItem>.Empty);
        }
    }

    [IDPrefix(IDConstants.EngineIDPrefix)]
    public record EngineID : IDB
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

    public record Details : IValueObject
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