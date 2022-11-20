using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Redis;
using Engine.Contract.Start;

namespace Engine.Context.Start;

public static class Actors
{
    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }

    public interface IResponder : IActor<Spoke>
    {
    }


    public class Responder : ResponderT<ResponderDriver, Hope, Cmd>, IResponder
    {
        public Responder(IExchange exchange,
            ResponderDriver responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(exchange,
            responderDriver,
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