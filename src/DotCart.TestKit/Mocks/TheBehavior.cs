using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestKit.Mocks;

public static class TheBehavior
{
    private static readonly IID _streamDocID = TheSchema.DocID.New;

    public static EventStreamGenFuncT<TheSchema.DocID> EventStreamGenFunc
        =
        _ => new List<IEvtB>
        {
            Event.New<TheContract.Payload>(_streamDocID,
                TheContract.Payload.Random().ToBytes(),
                TheContract.Meta.Empty),
        };

    public static IServiceCollection AddEventStreamGenFunc(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => EventStreamGenFunc);
    }


    [Name(TheConstants.AggregateName)]
    public interface IAggregateInfo : IAggregateInfoB;
}