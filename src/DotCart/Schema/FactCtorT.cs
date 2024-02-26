using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Schema;

public delegate FactT<TPayload, TMeta> FactCtorT<TPayload, TMeta>(string aggId, TPayload payload, TMeta meta)
    where TPayload : IPayload;