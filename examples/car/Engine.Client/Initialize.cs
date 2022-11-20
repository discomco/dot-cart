using DotCart.Drivers.NATS;
using Engine.Contract.Initialize;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;

namespace Engine.Client;

public static class Initialize
{
    public static IServiceCollection AddInitializeRequester(this IServiceCollection services)
    {
        return services
            .AddTransient<IRequester, Requester>();
    }

    public interface IRequester
    {
    }

    internal class Requester : NATSRequesterT<Hope>, IRequester
    {
        protected Requester(IEncodedConnection bus) : base(bus)
        {
        }
    }
}