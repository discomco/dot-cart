using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using NATS.Client;

namespace Engine.Context.Start;

public static class Actors
{
    public interface IToRedisDoc : IActor<Spoke>
    {
    }

    public class ToRedisDoc : ProjectionT<IRedisStore<Common.Schema.Engine>,
            Common.Schema.Engine, IEvt>,
        IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Common.Schema.Engine> modelStore,
            Evt2State<Common.Schema.Engine, IEvt> evt2State) : base(exchange,
            modelStore,
            evt2State)
        {
        }
    }
}