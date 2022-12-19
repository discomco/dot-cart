using System.Collections;
using System.Collections.Immutable;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Context.Tests.Behavior;

public delegate Feedback MyFunc<in TDis>(TDis dis) where TDis : IEvtB;

public static class MyFuncs
{
    [Topic("Event1")] public static readonly MyFunc<IEvtB> _doFunc1 = evt => Feedback.New($"blabla1, {evt.EventId}");

    [Topic("Event2")] public static readonly MyFunc<IEvtB> _doFunc2 = evt => Feedback.New($"blabla2, {evt.EventId}");

    public interface IEvt1 : IEvtB
    {
    }

    public interface IEvt2 : IEvtB
    {
    }
}

public class Receiver : IReceiver
{
    public readonly IEnumerable<MyFunc<IEvtB>> _funcs;
    private IImmutableDictionary<string, MyFunc<IEvtB>> _funcsDict = ImmutableDictionary<string, MyFunc<IEvtB>>.Empty;

    public Receiver(IEnumerable<MyFunc<IEvtB>> funcs)
    {
        _funcs = funcs;
    }

    public IEnumerable FuncsDict => _funcsDict.AsEnumerable();

    public IImmutableDictionary<string, MyFunc<IEvtB>> InjectFuncs(IEnumerable<MyFunc<IEvtB>> funcs)
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