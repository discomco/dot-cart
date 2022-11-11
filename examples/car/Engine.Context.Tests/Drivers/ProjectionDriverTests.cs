using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Drivers;

public class ProjectionDriverTests : IoCTests
{
    public ProjectionDriverTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
    }
}