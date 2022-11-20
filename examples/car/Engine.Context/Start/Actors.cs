using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.InMem;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Contract.Start;
using NATS.Client;

namespace Engine.Context.Start;

public static class Actors
{

    public interface IResponder : IActor<Spoke>
    {
    }


    public class Responder : NATSResponderT<Hope, Cmd>, IResponder
    {
        public Responder(IEncodedConnection bus,
            IExchange exchange,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(bus,
            exchange,
            cmdHandler,
            hope2Cmd)
        {
        }
    }

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