using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.NATS;

public delegate Task<IFeedback> ProcessHopeAsync<TPayload>(IHopeT<TPayload> hope)
    where TPayload : IPayload;