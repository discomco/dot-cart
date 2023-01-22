namespace DotCart.Abstractions.Schema;

public delegate IEntityT<TID> EntityCtorT<TID>(TID ID)
    where TID : IID;