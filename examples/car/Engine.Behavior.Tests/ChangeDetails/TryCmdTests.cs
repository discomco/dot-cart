using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

public class TryCmdTests : TryCmdTestsT<
    Behavior.ChangeDetails.TryCmd, 
    Behavior.ChangeDetails.Cmd, 
    Engine,
    Schema.EngineID, 
    Contract.ChangeDetails.Payload>
{
    public TryCmdTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddChangeDetailsBehavior()
            .AddTransient(_ => TestUtils.ChangeDetails.CmdCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.IDCtor);

    }

    protected override Engine GetInValidState()
    {
        return TestUtils.ChangeDetails.InvalidEngineCtor();
    }

    protected override Engine GetValidState()
    {
        return TestUtils.ChangeDetails.ValidEngineCtor(); }
}
