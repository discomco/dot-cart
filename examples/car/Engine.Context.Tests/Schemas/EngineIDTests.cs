using DotCart.Abstractions.Schema;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Schemas;

public class EngineIDTests : IDTests<Schema.EngineID>
{
    public EngineIDTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
        _newID = TestEnv.ResolveRequired<NewID<Schema.EngineID>>();
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