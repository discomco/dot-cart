using DotCart.Drivers.NATS;
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

    internal class Requester : NATSRequesterT<Contract.Initialize.Hope>, IRequester
    {
        protected Requester(IEncodedConnection bus) : base(bus)
        {
        }
    }
}