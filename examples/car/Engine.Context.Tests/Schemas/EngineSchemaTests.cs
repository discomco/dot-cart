using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.Common.Schema;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Schemas;

public class EngineSchemaTests : SchemaTests<EngineID, Common.Schema.Engine>
{
    public EngineSchemaTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineCtor();
    }
}