using System.Text.Json.Serialization;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestKit.Mocks;

public static class TheSchema
{
    public static IServiceCollection AddTheDocCtors(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => EntityID.Ctor)
            .AddTransient(_ => Entity.Rand)
            .AddTransient(_ => DocID.Ctor)
            .AddTransient(_ => Doc.Rand);
    }

    
    [IDPrefix(TheConstants.EntityIDPrefix)]
    public record EntityID: IDB
    {
        [JsonConstructor]
        public EntityID(string value = "") 
            : base(IDPrefixAtt.Get<EntityID>(), value)
        {
        }

        public static readonly IDCtorT<EntityID>
            Ctor =
                _ => new EntityID("B1750D90-1E6B-415B-B631-CD8301BF48BD");
    }


    public record Entity(string Id, string Name) 
        : IEntityT<EntityID>
    {
        // public string Id { get; } = Id;
        //
        // public string Name { get; } = Name;

        public static readonly EntityCtorT<EntityID> 
            Rand = 
                _ => new Entity(EntityID.Ctor().Id(), "Some Random Name");
    }


    [DbName("1")]
    public record Doc(string Id, string Name, int Age, double Height) : IState
    {
        public static StateCtorT<Doc> Rand => RandomTheDoc;

        public string Rev { get; set; }


        private static Doc RandomTheDoc()
        {
            var names = new[] { "John Lennon", "Paul McCartney", "Ringo Starr", "George Harrison" };
            var randNdx = Random.Shared.Next(names.Length);
            var name = names[randNdx];
            return new Doc(DocID.New.Id(), name, Random.Shared.Next(21, 80), Random.Shared.NextDouble());
        }
    }

    public static class Materials
    {
        public static string[] Kinds = { "Wood", "Metal", "Rock", "Fabric" };

        public static string Random()
        {
            var ndx = System.Random.Shared.Next(0, Kinds.Length);
            return Kinds[ndx];
        }
    }


    [IDPrefix(TheConstants.DocIDPrefix)]
    public record DocID : IDB
    {
        public static readonly IDCtorT<DocID> Ctor =
            _ => New;

        public DocID(string value = "") : base(IDPrefixAtt.Get<DocID>(), value)
        {
        }

        public static DocID New => new("FD1A9876-158F-4EF5-A5B5-468465404551");
    }

    [Topic(TheConstants.MsgTopic)]
    public record Msg : IMsg
    {
        public static Msg Random => new();
    }
}