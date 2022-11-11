using DotCart.Client.Schemas;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Client.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Schemas;

public class EngineIDTests : IDTests<EngineID>
{
    public EngineIDTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _newID = Container.GetRequiredService<NewID<EngineID>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineIDCtor();
    }
}