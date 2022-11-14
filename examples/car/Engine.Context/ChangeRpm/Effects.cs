using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Effects;
using DotCart.Contract;
using DotCart.Drivers.InMem;
using Engine.Contract.ChangeRpm;

namespace Engine.Context.ChangeRpm;

public static class Effects
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
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(exchange,
            responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }


    public interface IToMemDocProjection : IProjection<MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>
    {
    }

    public class ToMemDocProjection : ProjectionT<Spoke, MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>,
        IToMemDocProjection

    {
        public ToMemDocProjection(IExchange exchange,
            IModelStore<Common.Schema.Engine> modelStore,
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
}