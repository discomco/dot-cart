using DotCart.Core;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace DotCart.Drivers.Serilog.Tests;

public class LogConstantsTests : IoCTests
{
    [Fact]
    public void ShouldPutAsVerb()
    {
        var s = "STARTING".AsVerb();
        Log.Information(s);
        Output.WriteLine(s);
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

    public LogConstantsTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }
}