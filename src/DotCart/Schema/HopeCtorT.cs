using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Schema;

public delegate HopeT<TPayload> HopeCtorT<TPayload>(string aggId, TPayload payload)
    where TPayload : IPayload;