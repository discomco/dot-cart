using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;
using DotCart.Core;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS
{
    public abstract class NATSRequesterDriverT<THope> : IRequesterDriver<THope>
        where THope : IHope
    {
        private readonly IEncodedConnection _bus;

        protected NATSRequesterDriverT(IEncodedConnection bus)
        {
            _bus = bus;
            _bus.OnDeserialize += OnDeserialize;
            _bus.OnSerialize += OnSerialize;
        }

        private byte[] OnSerialize(object obj)
        {
            return obj.ToBytes();
        }

        private object OnDeserialize(byte[] data)
        {
            return data.FromBytes<THope>();
        }

        private static string Topic => TopicAtt.Get<THope>();

        public async Task<IFeedback> RequestAsync(THope hope)
        {
            var rsp = Feedback.Empty;
            try
            {
                if (!_bus.IsClosed())
                {
                    Log.Debug($"::CONNECT bus: {_bus.ConnectedId}");
                }

                Log.Debug($"::REQUEST hope: AggId:{hope.AggId} on topic {Topic}.");
                var msg = (Feedback)_bus.Request(Topic, hope);
                Log.Debug($"::RECEIVED feedback: AggId:{msg.AggId} on {msg.Topic}.");
                return msg;
            }
            catch (Exception e)
            {
                rsp.SetError(e.AsError());
                Log.Fatal($"::EXCEPTION {e.InnerAndOuter()}");
            }
            return rsp;
        }


        public void Dispose()
        {
            if (_bus == null) return;
            _bus.OnSerialize -= OnSerialize;
            _bus.OnDeserialize -= OnDeserialize;
            _bus.Dispose();
        }

        public void SetActor(IActor actor)
        {}
        
        
    }
}