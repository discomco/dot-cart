using System.Text.Json;

namespace DotCart.Domain;

public record AggregateMeta
{
    public static readonly AggregateMeta Empty = new();
    public static AggregateMeta New(string id, long version = -1, int status = 0)
    {
        return new AggregateMeta(id, version, status);
    }

    private AggregateMeta()
    {
    }
    
    private AggregateMeta(string id, long version, int status)
    {
        Id = id;
        Status = status;
        Version = version;
    }

    private string Id { get;  }
    private long Version { get;  }
    private int Status { get;  }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}