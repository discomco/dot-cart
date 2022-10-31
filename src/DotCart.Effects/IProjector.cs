using DotCart.Behavior;

namespace DotCart.Effects;

public interface IProjector: IReactor
{
    Task Project(IEvt evt);
}