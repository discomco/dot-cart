using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Contract.Tests;

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