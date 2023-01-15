using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestKit.Mocks;

public static class TheSchema
{
    public static IServiceCollection AddTheDocCtors(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => ID.Ctor)
            .AddTransient(_ => Doc.Rand);
    }


    [DbName("1")]
    public record Doc(string Id, string Name, int Age, double Height) : IState
    {
        public static StateCtorT<Doc> Rand => RandomTheDoc;

        public string Prev { get; set; }


        private static Doc RandomTheDoc()
        {
            var names = new[] { "John Lennon", "Paul McCartney", "Ringo Starr", "George Harrison" };
            var randNdx = Random.Shared.Next(names.Length);
            var name = names[randNdx];
            return new Doc(ID.New.Id(), name, Random.Shared.Next(21, 80), Random.Shared.NextDouble());
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


    [IDPrefix(TheConstants.IDPrefix)]
    public record ID : IDB
    {
        public static readonly IDCtorT<ID> Ctor =
            _ => New;

        public ID(string value = "") : base(TheConstants.IDPrefix, value)
        {
        }

        public static ID New => new("FD1A9876-158F-4EF5-A5B5-468465404551");
    }

    [Topic(TheConstants.MsgTopic)]
    public record Msg : IMsg
    {
        public static Msg Random => new();
    }
}