using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotCart.Context.Actors;

public static partial class Inject
{
    public static IServiceCollection AddPipeBuilder<TPipeInfo,TPayload>(this IServiceCollection services)
        where TPayload : IPayload 
        where TPipeInfo : IPipeInfoB
    {
        services.TryAddSingleton<IPipeBuilderT<TPipeInfo,TPayload>, PipeBuilderT<TPipeInfo,TPayload>>();
        return services;
    }
}

public class PipeBuilderT<TPipeInfo, TPayload>
    : IPipeBuilderT<TPipeInfo,TPayload>
    where TPayload : IPayload 
    where TPipeInfo : IPipeInfoB
{
    private readonly IEnumerable<IStepT<TPipeInfo,TPayload>> _steps;


    public PipeBuilderT(IEnumerable<IStepT<TPipeInfo,TPayload>> steps)
    {
        _steps = steps
            .DistinctBy(s => s.Name)
            .OrderBy(s => s.Order);
    }


    public IPipeT<TPipeInfo,TPayload> Build()
    {
        return PipeT<TPipeInfo,TPayload>.New(_steps);
    }
}