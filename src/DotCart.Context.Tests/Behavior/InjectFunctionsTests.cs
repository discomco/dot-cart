using System.Collections;
using System.Collections.Immutable;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Context.Tests.Behavior;

public delegate Feedback MyFunc<in TDis>(TDis dis) where TDis: IEvt;

public static class MyFuncs
{
    public interface IEvt1 : IEvt
    {
    }
    public interface IEvt2 : IEvt
    {
    }
    [Topic("Event1")]
    public static readonly MyFunc<IEvt> _doFunc1 = evt => Feedback.New($"blabla1, {evt.MsgId}");
    [Topic("Event2")]
    public static readonly MyFunc<IEvt> _doFunc2 = evt => Feedback.New($"blabla2, {evt.MsgId}");
}


public class Receiver : IReceiver
{
    public readonly IEnumerable<MyFunc<IEvt>> _funcs;
    public IEnumerable FuncsDict => _funcsDict.AsEnumerable(); 
    private IImmutableDictionary<string, MyFunc<IEvt>> _funcsDict =  ImmutableDictionary<string, MyFunc<IEvt>>.Empty;

    public Receiver(IEnumerable<MyFunc<IEvt>> funcs)
    {
        _funcs = funcs;
    }

    public IImmutableDictionary<string,MyFunc<IEvt>> InjectFuncs(IEnumerable<MyFunc<IEvt>> funcs)
    {
        foreach (var func in funcs)
        {
            var topic = TopicAtt.Get(func);
            _funcsDict = _funcsDict.Add(topic, func);
        }
        return _funcsDict;
    }
}

public interface IReceiver
{
    IEnumerable FuncsDict { get; }
}

public class InjectFunctionsTests : IoCTests
{
    public InjectFunctionsTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
        
    }

    protected override void SetTestEnvironment()
    {
    }

    [Fact]
    public void ShouldResolveReceiver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var rcv = TestEnv.ResolveRequired<IReceiver>();
        // THEN
        Assert.NotNull(rcv);
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSingleton<IReceiver, Receiver>()
            .AddTransient(_ => MyFuncs._doFunc1)
            .AddTransient(_ => MyFuncs._doFunc2);
    }
}