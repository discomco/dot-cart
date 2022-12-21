using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Core.Tests;

public class FuncDITests : IoCTests
{
    private IFuncDI _funcDI;

    [Fact]
    public async Task ShouldResolveFuncDI()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _funcDI = TestEnv.ResolveRequired<IFuncDI>();
        // THEN
        Assert.NotNull(_funcDI);
    }

    [Fact]
    public async Task ShouldCreateNewFuncDI()
    {
    }
    
    
    
    public FuncDITests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddSingleton<IFuncDI, FuncDI>();
    }
}