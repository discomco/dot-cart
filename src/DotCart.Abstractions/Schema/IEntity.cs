namespace DotCart.Abstractions.Schema;

public interface IEntity
{
}

public interface IEntityT<TID> : IEntity where TID : IID
{
}