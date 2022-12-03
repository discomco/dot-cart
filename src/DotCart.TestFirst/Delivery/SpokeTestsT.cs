using DotCart.Abstractions.Actors;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Delivery;

public abstract class SpokeTestsT<TSpoke> : IoCTests
    where TSpoke : ISpokeT<TSpoke>
{
    protected IEnumerable<IActor<TSpoke>> _actors;
    protected ISpokeBuilder<TSpoke> _builder;
    protected Exception _caught;
    protected string _name;
    protected TSpoke _spoke;

    public SpokeTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveEmptySpoke()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _spoke = TestEnv.ResolveHosted<TSpoke>();
        // THEN
        Assert.NotNull(_spoke);
    }


    [Fact]
    public void ShouldBuildSpoke()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _builder = TestEnv.ResolveRequired<ISpokeBuilder<TSpoke>>();
        // WHEN
        _spoke = _builder.Build();
        // THEN
        Assert.NotNull(_spoke);
    }

    [Fact]
    public void ShouldResolveSpokeBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _builder = TestEnv.ResolveRequired<ISpokeBuilder<TSpoke>>();
        // THEN
        Assert.NotNull(_builder);
    }

    [Fact]
    public void ShouldResolveActors()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _actors = TestEnv.ResolveAll<IActor<TSpoke>>();
        // THEN
        Assert.NotNull(_actors);
        Assert.NotEmpty(_actors);
    }

    [Fact]
    public void ShouldHaveNameAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        try
        {
            _name = NameAtt.Get<TSpoke>();
            var expectedName = GetExpectedSpokeName();
            Assert.Equal(expectedName, _name);
        }
        catch (Exception e)
        {
            _caught = e;
        }

        // THEN
        Assert.Null(_caught);
    }

    [Fact]
    public async Task ShouldStartSpoke()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _spoke = TestEnv.ResolveHosted<TSpoke>();
        var cts = new CancellationTokenSource(2_000);
        // WHEN
        var _executor = TestEnv.ResolveRequired<IHostExecutor>();
        Assert.NotNull(_executor);
        // THEN
        _executor.StartAsync(cts.Token);
        var isStarted = false;
        await Task.Run(() =>
        {
            Thread.Sleep(500);
            while (!cts.IsCancellationRequested)
            {
                Thread.Sleep(1);
                isStarted = _spoke.Status == ComponentStatus.Inactive;
            }
        }, cts.Token);
        Assert.True(isStarted);
    }


    private string GetExpectedSpokeName()
    {
        return NameAtt.Get(this);
    }
}