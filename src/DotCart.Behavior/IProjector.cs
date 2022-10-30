namespace DotCart.Behavior;

public interface IProjector
{
    Task Project(IEvt evt);
}