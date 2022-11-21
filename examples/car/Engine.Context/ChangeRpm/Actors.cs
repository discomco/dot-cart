using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Drivers.InMem;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using NATS.Client;

namespace Engine.Context.ChangeRpm;

public static class Actors
{

    public interface IToMemDoc
    {
    }

    public class ToMemDoc : ProjectionT<MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>,
        IToMemDoc, IActor<Spoke>

    {
        public ToMemDoc(IExchange exchange,
            MemStore<Common.Schema.Engine> modelStore,
            Evt2State<Common.Schema.Engine, IEvt> evt2State) : base(exchange,
            modelStore,
            evt2State)
        {
        }
    }


    public interface IToRedisDoc
    {
    }


    public class ToRedisDoc : ProjectionT<
        IRedisStore<Common.Schema.Engine>,
        Common.Schema.Engine,
        IEvt>, IToRedisDoc, IActor<Spoke>
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