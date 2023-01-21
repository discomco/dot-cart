namespace DotCart.Abstractions.Schema;

// public interface IEntityB
// {
// }

public interface IEntityT<TID> //: IEntityB 
    where TID : IID
{
}