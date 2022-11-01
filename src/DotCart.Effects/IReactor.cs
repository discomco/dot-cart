using DotCart.Contract;

namespace DotCart.Effects;

public interface IReactor
{
    Task HandleAsync(IMsg msg);
    void SetSpoke(ISpoke spoke);    
}

