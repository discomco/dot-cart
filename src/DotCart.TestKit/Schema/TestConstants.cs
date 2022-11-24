using DotCart.Core;

namespace DotCart.TestKit.Schema;

public static class TestConstants
{
    public const string CORRELATION_ID = "TEST_CORRELATION_ID";
    public const string Id = GuidUtils.TEST_GUID;

    public const string GroupName = "TEST_GROUP";
    public const string TheIDPrefix = "the";
    public const string TheHopeTopic = "the_hope.topic";
    public const string CmdTopic = "the_cmd:topic";
    public const string SubscriptionGroup = "the_group";
    public static readonly Guid Guid = Guid.Parse(GuidUtils.TEST_GUID);
}