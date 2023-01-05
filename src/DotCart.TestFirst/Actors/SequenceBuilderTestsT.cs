using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using FakeItEasy;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class SequenceBuilderTestsT<TPayload>
    : IoCTests
    where TPayload : IPayload
{
    protected SequenceBuilderTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveSequenceBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<ISequenceBuilderT<TPayload>>();
        // THEN
        Assert.NotNull(builder);
    }

    [Fact]
    public void ShouldBuildSequence()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<ISequenceBuilderT<TPayload>>();
        // WHEN
        var sequence = builder.Build();
        // THEN
        Assert.NotNull(sequence);
    }

    [Fact]
    public void ShouldContainSteps()
    {
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<ISequenceBuilderT<TPayload>>();
        var sequence = builder.Build();
        Assert.NotNull(sequence);
        // WHEN
        var originalSteps = TestEnv.ResolveAll<IStepT<TPayload>>();
        // THEN
        Assert.True(sequence.StepCount > 0);
       
    }
    

    [Fact]
    public void ShouldOnlyContainDistinctSteps()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<ISequenceBuilderT<TPayload>>();
        var sequence = builder.Build();
        Assert.NotNull(sequence);
        // WHEN
        var originalSteps = TestEnv.ResolveAll<IStepT<TPayload>>();
        // THEN
        Assert.True(originalSteps.Count() >= sequence.StepCount);
    }

    [Fact]
    public void ShouldHaveName()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<ISequenceBuilderT<TPayload>>();
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
        var builder = TestEnv.ResolveRequired<ISequenceBuilderT<TPayload>>();
        var msg = A.Fake<IDtoT<TPayload>>();
        var seq = builder.Build();
        Assert.NotNull(seq);
        // WHEN
        var feedback = await seq.ExecuteAsync(msg);
        // THEN
        Assert.NotNull(feedback);
    }
    
   
}