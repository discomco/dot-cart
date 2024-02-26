using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Schema;

public interface IFactB : IDto
{
    public string Topic { get; }
}

public interface IFactT<TPayload> : IFactB
    where TPayload : IPayload;

public interface IFactT<TPayload, TMeta> : IFactB
    where TPayload : IPayload
    where TMeta : IMetaB
;