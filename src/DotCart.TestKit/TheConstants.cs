using DotCart.Core;

namespace DotCart.TestKit;

public static class TheConstants
{
    public const string CORRELATION_ID = "TEST_CORRELATION_ID";
    public const string Id = GuidUtils.TEST_GUID;
    public const string GroupName = "TEST_GROUP";
    public const string IDPrefix = "the";
    public const string HopeTopic = "the_hope.topic";
    public const string CmdTopic = "the_cmd:topic";
    public const string SubscriptionGroup = "the_group";
    public const string EvtTopic = "the_event.topic";
    public const string MsgTopic = "dotcart:the_msg";
    public const string FactTopic = "the_fact.topic";
    public const string AggregateName = "the_aggregate";
    public const string StepName = "the_step";
    public const string PipeName = "the_pipe";
    public static readonly Guid Guid = Guid.Parse(GuidUtils.TEST_GUID);
}