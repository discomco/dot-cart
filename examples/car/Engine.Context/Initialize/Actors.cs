using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Core;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Redis;
using Engine.Contract.Initialize;

namespace Engine.Context.Initialize;

public static class Actors
{
    public interface IResponder : IActor<Spoke>
    {
    }

    [Name("Engine.Initialize.Responder")]
    public class Responder : ResponderT<IResponderDriverT<Hope>, Hope, Cmd>, IResponder
    {
        public Responder(IExchange exchange,
            IResponderDriverT<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(exchange,
            responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }

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