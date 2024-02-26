using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Schema;

public delegate
    TDriverMsg Fact2Msg<out TDriverMsg, TPayload, TMeta>(FactT<TPayload, TMeta> fact)
    where TPayload : IPayload
    where TDriverMsg : class;