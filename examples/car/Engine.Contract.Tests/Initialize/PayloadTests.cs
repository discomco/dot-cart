using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Initialize;

public class PayloadTests : PayloadTestsT<Contract.Initialize.Payload>
{
    public PayloadTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => TestUtils.Initialize.PayloadCtor);
    }
}