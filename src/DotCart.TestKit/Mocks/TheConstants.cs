using DotCart.Core;

namespace DotCart.TestKit.Mocks;

public static class TheConstants
{
    public const string CORRELATION_ID = "TEST_CORRELATION_ID";
    public const string Id = GuidUtils.TEST_GUID;
    public const string GroupName = "TEST_GROUP";
    public const string DocIDPrefix = "thedoc";
    public const string HopeTopic = "the_hope.topic";
    public const string CmdTopic = "the_cmd:topic";
    public const string SubscriptionGroup = "the_group";
    public const string EvtTopic = "the_event.topic";
    public const string MsgTopic = "dotcart:the_msg";
    public const string FactTopic = "the_fact.topic";
    public const string AggregateName = "the_aggregate";
    public const string StepName = "the_step";
    public const string PipeName = "the_pipe";
    public const string CouchDocDbName = "the_doc_db";
    public const string CouchListDbName = "the_list_db";
    public const string RedisDocDbName = "3";
    public const string RedisListDbName = "2";
    public const string EntityIDPrefix = "theentity";
    public static readonly Guid Guid = Guid.Parse(GuidUtils.TEST_GUID);
}