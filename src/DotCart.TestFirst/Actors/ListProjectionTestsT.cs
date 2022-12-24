using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class
    ListProjectionTestsT<TSpoke,TProjection,TTargetDoc,TIEvt> 
    : ProjectionTestsT<TSpoke,TProjection,TTargetDoc,TIEvt>
    where TSpoke : ISpokeT<TSpoke>
    where TProjection : IActor<TSpoke>
    where TTargetDoc : IState
    where TIEvt : IEvtB
{
    protected ListProjectionTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldTProjectionHaveDocIdAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var docId = DocIdAtt.Get<TProjection>();
        // THEN
        Assert.NotNull(docId);
    }
    
    [Fact]
    public void ShouldTestHaveDocIdAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var expected = DocIdAtt.Get(this);
        // THEN
        Assert.NotNull(expected);
    }
    
    [Fact]
    public void ShouldTestAndTProjectionHaveSameAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var expected = DocIdAtt.Get(this);
        var actual = DocIdAtt.Get<TProjection>();
        // THEN
        Assert.Equal(expected,actual);
    }
    
    
}