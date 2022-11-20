using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Redis;
using Engine.Contract.ChangeRpm;

namespace Engine.Context.ChangeRpm;

public static class Actors
{
    public interface IResponder : IActor<Spoke>
    {
    }


    public class Responder : ResponderT<Drivers.IMemResponderDriver, Hope, Cmd>, IResponder
    {
        protected Responder(IExchange exchange,
            Drivers.IMemResponderDriver responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(exchange,
            responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }


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


    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
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