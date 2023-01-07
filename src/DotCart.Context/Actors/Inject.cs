using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Actors;

public static partial class Inject
{
    public static IServiceCollection AddHopeInPipe<TPipeInfo, THopePayload, TMeta>(this IServiceCollection services)
        where THopePayload : IPayload
        where TMeta : IMeta
        where TPipeInfo : IPipeInfoB
    {
        return services
            .AddTransient<IStepT<TPipeInfo, THopePayload>, HopeLoggerStepT<TPipeInfo, THopePayload>>()
            .AddTransient<IStepT<TPipeInfo, THopePayload>, CmdHandlerStepT<TPipeInfo, THopePayload, TMeta>>()
            .AddPipeBuilder<TPipeInfo, THopePayload>();
    }


    public static IServiceCollection AddRetroPipe<TPipeInfo, TFactPayload, TMeta>(this IServiceCollection services)
        where TFactPayload : IPayload
        where TMeta : IMeta
        where TPipeInfo : IPipeInfoB
    {
        return services
            .AddTransient<IStepT<TPipeInfo, TFactPayload>, FactLoggerStepT<TPipeInfo, TFactPayload>>()
            .AddPipeBuilder<TPipeInfo, TFactPayload>();
    }

    public static IServiceCollection AddFactInPipe<TPipeInfo, TFactPayload, TMeta>(this IServiceCollection services)
        where TFactPayload : IPayload
        where TMeta : IMeta
        where TPipeInfo : IPipeInfoB
    {
        return services
            .AddTransient<IStepT<TPipeInfo, TFactPayload>, FactLoggerStepT<TPipeInfo, TFactPayload>>()
            .AddTransient<IStepT<TPipeInfo, TFactPayload>, CmdHandlerStepT<TPipeInfo, TFactPayload, TMeta>>()
            .AddPipeBuilder<TPipeInfo, TFactPayload>();
    }
}