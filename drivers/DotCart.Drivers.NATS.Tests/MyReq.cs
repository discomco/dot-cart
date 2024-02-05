using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Drivers.NATS.Tests;

// [Topic(TestConstants.RequestTopicV1)]
[HopeTopic(TestConstants.HopeTopicV1)]
public class MyReq
    : IPayload
{
    public string? Name { get; set; }

    public static MyReq New(string name)
    {
        return new MyReq { Name = name };
    }
}

public class MyRsp : IPayload
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int Age { get; set; }
}