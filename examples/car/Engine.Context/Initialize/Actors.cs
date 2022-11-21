using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Core;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Redis;

namespace Engine.Context.Initialize;

public static class Actors
{
    public interface IToMemDoc : IActor<Spoke>
    {
    }

    [Name("Engine.Initialize.ToMemDocProjection")]
    public class ToMemDoc : ProjectionT<MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>,
        IToMemDoc
    {
        public ToMemDoc(IExchange exchange,
            MemStore<Common.Schema.Engine> modelStore,
            Evt2State<Common.Schema.Engine, IEvt> evt2State) : base(exchange,
            modelStore,
            evt2State)
        {
        }
    }

    public interface IToRedisDoc : IActor<Spoke>
    {
    }

    public class ToRedisDoc : ProjectionT<
            IRedisStore<Common.Schema.Engine>,
            Common.Schema.Engine,
            IEvt>,
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