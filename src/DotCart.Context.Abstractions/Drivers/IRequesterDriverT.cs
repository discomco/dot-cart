using DotCart.Contract.Dtos;

namespace DotCart.Context.Abstractions.Drivers;

public interface IRequesterDriver<in THope> : IDriver 
    where THope: IHope
{
    Task<IFeedback> RequestAsync(THope hope);
}