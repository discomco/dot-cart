using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;


public interface IEvt : IMsg
{
    string AggregateId { get;  }
    string EventId { get;  }
    byte[] MetaData { get;  }
    long Version { get;  }
    string Topic { get; }
    DateTime TimeStamp { get; }
    byte[] Data { get; }
    void SetVersion(long version);
    void SetTimeStamp(DateTime timeStamp);
    void SetData(byte[] data);
    void SetEventType(string eventType);
    void SetMetaData(byte[] metaData);
    
    void SetMetaPayload<TPayload>(TPayload payload);
    TPayload GetMetaPayload<TPayload>();
    TPayload GetPayload<TPayload>() where TPayload : IPayload;
    void SetPayload<TPayload>(TPayload payload) where TPayload : IPayload;

}