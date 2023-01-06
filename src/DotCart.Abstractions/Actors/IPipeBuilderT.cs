using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IPipeBuilderT<TPipeInfo,TPayload> 
    where TPayload : IPayload 
    where TPipeInfo: IPipeInfoB
{
    IPipeT<TPipeInfo,TPayload> Build();
}