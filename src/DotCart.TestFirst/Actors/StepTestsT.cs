using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class StepTestsT<TStep, TPayload>
    : IoCTests
    where TStep : IStepT<TPayload>
    where TPayload : IPayload
{
    protected StepTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldHaveNameAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var nameAtt = NameAtt.Get<TStep>();
        // THEN
        Assert.NotEmpty(nameAtt);
    }

    [Fact]
    public void ShouldHaveOrderAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var orderAtt = OrderAtt.Get<TStep>();
        // THEN
    }

    [Fact]
    public void ShouldResolveStep()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var step = TestEnv.ResolveRequired<IStepT<TPayload>>();
        // THEN
        Assert.NotNull(step);
    }
}