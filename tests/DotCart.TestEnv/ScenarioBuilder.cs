using DotCart.Schema;

namespace DotCart.TestEnv;



public class ScenarioBuilder<TState> : IScenarioBuilder
    where TState : IState
{
    public ScenarioBuilder()
    {
        
    }
    
}

public interface IScenarioBuilder
{
}
