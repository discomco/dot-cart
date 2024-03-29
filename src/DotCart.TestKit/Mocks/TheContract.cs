using System.Text.Json.Serialization;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestKit.Mocks;

public static class TheContract
{
    private static readonly FactCtorT<Payload, Meta>
        _newFact =
            FactT<Payload, Meta>.New;

    private static readonly Msg2Fact<Payload, Meta, byte[]>
        _bytes2Fact =
            msg => msg.FromBytes<FactT<Payload, Meta>>()!;

    private static readonly Fact2Msg<byte[], Payload, Meta>
        _fact2bytes =
            fact => fact.ToBytes();

    public static IServiceCollection AddTheACLFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _bytes2Fact)
            .AddTransient(_ => _fact2bytes)
            .AddTransient(_ => _newFact);
    }


    [Topic(TheConstants.HopeTopic)]
    public record Hope(string AggId, Payload Payload) : HopeT<Payload>(AggId, Payload),
        IHopeT<Payload>
    {
        public new static Hope New(string aggId, Payload payload)
        {
            return new Hope(aggId, payload);
        }
    }


    [HopeTopic(TheConstants.HopeTopic)]
    [FactTopic(TheConstants.FactTopic)]
    [CmdTopic(TheConstants.CmdTopic)]
    [EvtTopic(TheConstants.EvtTopic)]
    public record Payload
        : IPayload
    {
        [JsonConstructor]
        public Payload(string material, decimal weight, DateTime arrival, string id)
        {
            Material = material;
            Weight = weight;
            Arrival = arrival;
            Id = id;
        }

        public string Material { get; }
        public decimal Weight { get; }
        public DateTime Arrival { get; }
        public string Id { get; }

        public static Payload New(string id, string material, decimal weight, DateTime arrival)
        {
            return new Payload(material, weight, arrival, id);
        }

        public static Payload Random()
        {
            return New(
                GuidUtils.LowerCaseGuid,
                TheSchema.Materials.Random(),
                System.Random.Shared.Next(1, 100),
                DateTime.UtcNow);
        }
    }


    [Topic(TheConstants.FactTopic)]
    public interface IFact
        : IFactB;

    public record Meta(string AggregateType, string AggregateId)
        : MetaB(AggregateType, AggregateId)
    {
        public static Meta New(string aggType, string aggId)
        {
            return new Meta(aggType, aggId);
        }
    }
}