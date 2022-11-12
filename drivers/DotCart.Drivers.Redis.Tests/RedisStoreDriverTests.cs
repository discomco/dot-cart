using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;
using DotCart.Core;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.Redis.Tests;

[IDPrefix("my")]
public record MyID : ID
{
    public MyID(string value = "") : base("my", value)
    {
    }

    public static NewID<MyID> Ctor => () => New;

    public static MyID New => new(GuidUtils.LowerCaseGuid);
}

[DbName("1")]
public record MyDoc(string Id, string Name, int Age, double Height) : IState
{
    public static NewState<MyDoc> Rand => RandomMyDoc;


    private static MyDoc RandomMyDoc()
    {
        var names = new[] { "John Lennon", "Paul McCartney", "Ringo Starr", "George Harrison" };
        var randNdx = Random.Shared.Next(names.Length);
        var name = names[randNdx];
        return new MyDoc(MyID.New.Id(), name, Random.Shared.Next(21, 80), Random.Shared.NextDouble());
    }
}

internal class MyRedisStore : RedisStore<MyDoc>
{
    public MyRedisStore(
        IRedisDb redisDb) : base(redisDb)
    {
    }
}

internal static class Inject
{
    public static IServiceCollection AddMyRedisStoreDriver(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => MyDoc.Rand)
            .AddTransient(_ => MyID.Ctor)
            .AddTransient<IModelStore<MyDoc>, MyRedisStore>();
    }
}

public class MyRedisStoreDriverTests : RedisStoreDriverTestsT<MyID, MyDoc>
{
    public MyRedisStoreDriverTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddMyRedisStoreDriver()
            .AddSingletonRedisDb<MyDoc>();
    }
}