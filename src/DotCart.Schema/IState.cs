namespace DotCart.Schema;

public delegate TState NewState<out TState>() where TState:IState;

public delegate TStateRepo NewStateRepo<out TStateRepo>() where TStateRepo : IStateRepo;

public interface IStateRepo
{
    
}



public interface IState: IPld
{ }