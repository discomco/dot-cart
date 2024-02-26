using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.NATS.Tests;

[FactTopic(TestConstants.FactTopicV1)]
public class MyPayload
    : IPayload
{
    public static readonly PayloadCtorT<MyPayload> New =
        () => new MyPayload();

    public string? Name { get; set; } = "John Smith";
    public int Age { get; set; } = 42;
}