using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using FakeItEasy;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class PipeBuilderTestsT<TPipeInfo,TPayload>
    : IoCTests
    where TPayload : IPayload
    where TPipeInfo: IPipeInfoB
{
    protected PipeBuilderTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolvePipeBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<IPipeBuilderT<TPipeInfo,TPayload>>();
        // THEN
        Assert.NotNull(builder);
    }

    [Fact]
    public void ShouldBuildPipe()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<IPipeBuilderT<TPipeInfo,TPayload>>();
        // WHEN
        var pipe = builder.Build();
        // THEN
        Assert.NotNull(pipe);
    }

    [Fact]
    public void ShouldContainSteps()
    {
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<IPipeBuilderT<TPipeInfo,TPayload>>();
        var pipe = builder.Build();
        Assert.NotNull(pipe);
        // WHEN
        var originalSteps = TestEnv.ResolveAll<IStepT<TPipeInfo,TPayload>>();
        // THEN
        Assert.True(pipe.StepCount > 0);
    }


    [Fact]
    public void ShouldOnlyContainDistinctSteps()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<IPipeBuilderT<TPipeInfo,TPayload>>();
        var pipe = builder.Build();
        Assert.NotNull(pipe);
        // WHEN
        var originalSteps = TestEnv.ResolveAll<IStepT<TPipeInfo,TPayload>>();
        // THEN
        Assert.True(originalSteps.Count() >= pipe.StepCount);
    }

    [Fact]
    public void ShouldHaveName()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<IPipeBuilderT<TPipeInfo,TPayload>>();
        // WHEN
        var seq = builder.Build();
        var name = seq.Name;
        // THEN
        Assert.NotEmpty(name);
    }

    [Fact]
    public async Task ShouldExecuteAsync()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<IPipeBuilderT<TPipeInfo,TPayload>>();
        var msg = A.Fake<IDtoT<TPayload>>();
        var seq = builder.Build();
        Assert.NotNull(seq);
        // WHEN
        var feedback = await seq.ExecuteAsync(msg);
        // THEN
        Assert.NotNull(feedback);
    }
}