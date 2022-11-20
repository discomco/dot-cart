using DotCart.Abstractions.Schema;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Schemas;

public class EngineIDTests : IDTests<EngineID>
{
    public EngineIDTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
        _newID = TestEnv.ResolveRequired<NewID<EngineID>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddModelIDCtor();
    }
}