using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Drivers.NATS.Tests;

[FactTopic(TestConstants.FactTopicV1)]
public class MyPayload
    :  IPayload
{
    public static readonly PayloadCtorT<MyPayload> New =
        () => new MyPayload();

    public string? Name { get; set; } = "John Smith";
    public int Age { get; set; } = 42;
}
