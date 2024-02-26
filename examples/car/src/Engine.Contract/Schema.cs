using System.Collections.Immutable;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Schema;

namespace Engine.Contract;

public static class Schema
{

    private static readonly object _setFlagMutex = new();

    public static Engine.Flags SetFlag(this Engine.Flags status, Engine.Flags flag)
    {
        lock (_setFlagMutex)
        {
            var newStatus = ((int)status).SetFlag((int)flag);
            return (Engine.Flags)newStatus;
        }
    }

    public static Engine.Flags UnsetFlag(this Engine.Flags status, Engine.Flags flag)
    {
        lock (_setFlagMutex)
        {
            var newStatus = ((int)status).UnsetFlag((int)flag);
            return (Engine.Flags)newStatus;
        }
    }

    public static bool HasFlagFast(this Engine.Flags value, Engine.Flags flag)
    {
        return (value & flag) != 0;
    }



    [DbName(DbConstants.RedisDocDbName)]
    public record Engine(string Id, Engine.Flags Status, Details Details, Rpm Rpm)
        : IState
    {

        [Flags]
        public enum Flags
        {
            Unknown = 0,
            Initialized = 1,
            Started = 2,
            Stopped = 4,
            Overheated = 8
        }



        // private Engine()
        // {
        //     Details = new Details();
        //     Status = EngineStatus.Unknown;
        //     Rpm = new Rpm(0);
        // }
        //
        // [JsonConstructor]
        // public Engine(string Id, EngineStatus Status, Details Details, Rpm Rpm)
        // {
        //     Id = Id;
        //     Status = Status;
        //     Details = Details;
        //     Rpm = Rpm;
        // }

        // private Engine(string id)
        // {
        //     Id = id;
        //     Status = EngineStatus.Unknown;
        //     Details = Details.New("New Engine");
        //     Rpm = Rpm.New(0);
        // }

        public Flags Status { get; set; } = Status;

        public Rpm Rpm { get; set; } = Rpm;
        public Details Details { get; set; } = Details;
        public string Id { get; set; } = Id;

        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // public string Id { get; set; }

        public string? Rev { get; set; } = "";

        public static Engine New(string id, Flags status, Details details, Rpm rpm)
        {
            return new Engine(id, status, details, rpm);
        }

        public static Engine New()
        {
            var ID = EngineID.New();
            return new Engine(ID.Id(), Flags.Unknown, Details.New("New Engine"), Rpm.New(0));
        }
    }

    public record EngineListItem
        : IValueObject, IState
    {
        public string Name { get; set; }
        public Engine.Flags Status { get; set; }
        public int Power { get; set; }
        public string Id { get; set; }
        public string? Rev { get; set; }

        public static EngineListItem New(string aggId, string name, Engine.Flags status, int power)
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
    public record EngineListID()
        : IDB(IDConstants.EngineListIDPrefix, IDConstants.EngineListIDValue)
    {
        public static EngineListID New()
        {
            return new EngineListID();
        }
    }

    [DbName(DbConstants.RedisListDbName)]
    public record EngineList(string Id, ImmutableDictionary<string, EngineListItem> Items)
        : IListState
    {
        public ImmutableDictionary<string, EngineListItem> Items { get; set; } = Items;

        public string Id { get; } = Id;
        public string? Rev { get; set; }

        public static EngineList New(string id)
        {
            return new EngineList(id, ImmutableDictionary<string, EngineListItem>.Empty);
        }
    }

    [IDPrefix(IDConstants.EngineIDPrefix)]
    public record EngineID
        : IDB
    {
        public EngineID(string value = "")
            : base(IDPrefixAtt.Get<EngineID>(), value)
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
        : IValueObject
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

    public record Rpm(int Value)
        : IValueObject
    {
        public int Value { get; } = Value;

        public static Rpm? New(int value)
        {
            return new Rpm(value);
        }
    }

}