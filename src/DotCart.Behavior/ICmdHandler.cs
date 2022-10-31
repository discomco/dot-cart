using DotCart.Contract;

namespace DotCart.Behavior;

public interface ICmdHandler
{
    Task<IFeedback> Handle(ICmd cmd);
}