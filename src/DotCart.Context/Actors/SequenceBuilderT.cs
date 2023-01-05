using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotCart.Context.Actors;

public static partial class Inject
{
    public static IServiceCollection AddSequenceBuilder<TPayload>(this IServiceCollection services)
        where TPayload : IPayload
    {
        services.TryAddSingleton<ISequenceBuilderT<TPayload>, SequenceBuilderT<TPayload>>();
        return services;
    }
}

public class SequenceBuilderT<TPayload>
    : ISequenceBuilderT<TPayload>
    where TPayload : IPayload
{
    private readonly IEnumerable<IStepT<TPayload>> _steps;


    public SequenceBuilderT(IEnumerable<IStepT<TPayload>> steps)
    {
        _steps = steps
            .DistinctBy(s => s.Name)
            .OrderBy(s => s.Order);
    }


    public ISequenceT<TPayload> Build()
    {
        return SequenceT<TPayload>.New(_steps);
    }
}