using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Schema
{

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


    [Flags]
    public enum EngineStatus
    {
        Unknown = 0,
        Initialized = 1,
        Started = 2,
        Stopped = 4,
        Overheated = 8
    }



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


    public record EngineListItem : IValueObject, IEntityT<EngineID>
    {
        public string EngineId { get; set; }
        public string Name { get; set;}
        public EngineStatus Status  { get; set; }
        public int Power { get; set; }

        public static EngineListItem New(string aggId, string name, EngineStatus status, int power)
        {
            return new EngineListItem
            {
                EngineId = aggId,
                Name = name,
                Status = status,
                Power = power
            };
        }
    }

    
    [IDPrefix(IDConstants.EngineListIDPrefix)]
    public record EngineListID() : ID(IDConstants.EngineListIDPrefix, IDConstants.EngineListIDValue)
    {
        public static EngineListID New() 
            => new();
    }

    [DbName(Constants.ListRedisDbName)]
    public record EngineList(ImmutableDictionary<string,EngineListItem> Items) : IListState
    {
        // public EngineListID ID { get; set; } = new();
        public ImmutableDictionary<string,EngineListItem> Items { get; set; } = Items;

        public static EngineList New()
        {
            return new EngineList(ImmutableDictionary<string, EngineListItem>.Empty);
        }
    }

    [IDPrefix(IDConstants.EngineIDPrefix)]
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