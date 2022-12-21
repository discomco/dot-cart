using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Core.Tests;

public delegate string TestFtor(string input);

public class StringUtils : IStringUtils
{
    public StringUtils()
    {
        TestFtor = Funcs._myFunc;
    }

    public TestFtor TestFtor { get; }
}

public interface IStringUtils
{
    TestFtor TestFtor { get; }
}

static class Funcs
{
    public static readonly TestFtor
        _myFunc =
            input =>
            {
                var output = input.ToUpper();
                Console.WriteLine($"Input: {input} Output: {output}");
                return output;
            };
}

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
    public async Task ShouldBeThreadSafe()
    {
        var cts = new CancellationTokenSource(5_000);
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var utils1 = TestEnv.ResolveRequired<IStringUtils>();
        var utils2 = TestEnv.ResolveRequired<IStringUtils>();
        // THEN
        Assert.NotSame(utils1, utils2);
        var f1 = utils1.TestFtor;
        var f2 = utils2.TestFtor;
        Assert.Same(f1, f2);
        var f1Res = f1("hello1");
        var f2Res = f2("bye");
        Assert.Equal("HELLO1", f1Res);
        Assert.Equal("BYE", f2Res);
        var task1 = Task.Run(() =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                f1("task1");
                Thread.Sleep(1);
            }
        }, cts.Token);
        var task2 = Task.Run(() =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                f2("task2");
                Thread.Sleep(1);
            }
        }, cts.Token);
        while (!cts.Token.IsCancellationRequested)
        {
            Task.WaitAll(task1, task2);
        }
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
            .AddTransient(_ => Funcs._myFunc)
            .AddTransient<IStringUtils, StringUtils>()
            .AddSingleton<IFuncDI, FuncDI>();
    }
}