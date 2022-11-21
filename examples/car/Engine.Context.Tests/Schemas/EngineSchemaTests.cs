using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.Common.Schema;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Schemas;

public class EngineSchemaTests : SchemaTests<Schema.EngineID, Common.Schema.Engine>
{
    public EngineSchemaTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddModelCtor();
    }
}