using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Start
{
    public static class Generators
    {
        public static GenerateHope<Hope> _generateHope => () =>
        {
            var pl = Payload.New;
            var aggId = Schema.IDCtor();
            return Hope.New(aggId.Id(), pl.ToBytes());
        };
    }

    public static class Topics
    {
        public const string Hope = "engine.start.v1";
        public const string Fact = "engine.started.v1";
    }

    public record Payload : IPayload
    {
        public static readonly Payload New = new();
    }

    [Topic(Topics.Fact)]
    public interface IFact : IFact<Payload>
    {
    }

    [Topic(Topics.Fact)]
    public record Fact(string AggId, byte[] Data) : Dto(AggId, Data), IFact
    {
        public static Fact New(string AggId, byte[] Data)
        {
            return new Fact(AggId, Data);
        }

        public static Fact New(string AggId, Payload payload)
        {
            return new Fact(AggId, payload.ToBytes());
        }
    }

    [Topic(Topics.Hope)]
    public interface IHope : IHope<Payload>
    {
    }

    [Topic(Topics.Hope)]
    public record Hope(string AggId, byte[] Data) : Dto(AggId, Data), IHope
    {
        public static Hope New(string AggId, byte[] Data)
        {
            return new Hope(AggId, Data);
        }

        public static Hope New(string AggId, Payload payload)
        {
            return new Hope(AggId, payload.ToBytes());
        }
    }
}