using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Effects;
using DotCart.Contract;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Redis;
using Engine.Contract.ChangeRpm;

namespace Engine.Context.ChangeRpm;

public static class Actors
{
    public interface IResponder : IResponder<MemResponderDriver<Hope>, Hope, Cmd>
    {
    }


    public class Responder : ResponderT<Spoke, MemResponderDriver<Hope>, Hope, Cmd>, IResponder
    {
        // public Responder(
        //     IExchange exchange,
        //     IResponderDriver<Hope> responderDriver,
        //     ICmdHandler cmdHandler,
        //     Hope2Cmd<Cmd, Hope> hope2Cmd) : base(
        //     responderDriver,
        //     cmdHandler,
        //     hope2Cmd) : base()
        // {
        // }
        public Responder(IExchange exchange,
            MemResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(exchange,
            responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }


    public interface IToMemDoc : IProjection<MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>
    {
    }

    public class ToMemDoc : ProjectionT<Spoke, MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>,
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


    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
    
    
    public interface IToRedisDoc : IProjection<
        IRedisStore<Common.Schema.Engine>, 
        Common.Schema.Engine, 
        IEvt>
    {
        
    }
    

    public class ToRedisDoc : ProjectionT<
        Spoke, 
        IRedisStore<Common.Schema.Engine>, 
        Common.Schema.Engine, 
        IEvt>, IToRedisDoc
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