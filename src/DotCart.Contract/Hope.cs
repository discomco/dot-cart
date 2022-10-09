using DotCart.Schema;

namespace DotCart.Contract;

public interface IHope: IDto {}

public interface IHope<TPayload> : IHope, IDto<TPayload> where TPayload : IPayload
{
}

public abstract record Hope<TPayload>(string AggId, byte[] Data) : Dto<TPayload>(AggId, Data), IHope where TPayload : IPayload;