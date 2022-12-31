using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestKit;

public static class TheConstants
{
    public const string CORRELATION_ID = "TEST_CORRELATION_ID";
    public const string Id = GuidUtils.TEST_GUID;
    public const string GroupName = "TEST_GROUP";
    public const string IDPrefix = "the";
    public const string HopeTopic = "the_hope.topic";
    public const string CmdTopic = "the_cmd:topic";
    public const string SubscriptionGroup = "the_group";
    public const string EvtTopic = "the_event.topic";
    public const string MsgTopic = "dotcart:the_msg";
    public const string FactTopic = "the_fact.topic";
    public static readonly Guid Guid = Guid.Parse(GuidUtils.TEST_GUID);
}

public static class TheSchema
{
    public static IServiceCollection AddTheIDCtor(this IServiceCollection services)
    {
        return services.AddTransient(_ => ID.Ctor);
    }


    [DbName("1")]
    public record Doc(string Id, string Name, int Age, double Height) : IState
    {
        public static StateCtorT<Doc> Rand => RandomTheDoc;


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