using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Context.Actors;
using DotCart.TestKit;
using Engine.Behavior;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class SequenceBuilderTests : IoCTests
{
    public SequenceBuilderTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetEnVars()
    {
    }

    [Fact]
    public void ShouldResolveSequenceBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var _seqBuild = TestEnv.ResolveRequired<ISequenceBuilderT<TheContract.Payload>>();
        // THEN
        Assert.NotNull(_seqBuild);
    }

    [Fact]
    public void ShouldCreateDifferentCommandHandlers()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var seq1 = TestEnv.ResolveRequired<ISequenceBuilderT<TheContract.Payload>>();
        // WHEN
        var ch1 = seq1.Build();
        var ch2 = seq1.Build();
        // THEN
        Assert.NotNull(ch1);
        Assert.NotNull(ch2);
        Assert.NotSame(ch1, ch2);
    }


    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSingleton(_ => A.Fake<IAggregateStore>())
            .AddInitializeBehavior()
            .AddSequenceBuilder<TheContract.Payload>();
    }
}