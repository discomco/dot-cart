using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DotCart.Core;

internal class YamlSerializer
    : IYamlSerializer
{
    public string Serialize(object? graph, Type type)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();
        return serializer.Serialize(graph, type);
    }

    public void Serialize(TextWriter writer, object graph)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();
        serializer.Serialize(writer, graph);
    }

    public string Serialize(object graph)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();
        return serializer.Serialize(graph);
    }

    public void Serialize(TextWriter writer, object graph, Type type)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();
        serializer.Serialize(writer, graph);
    }

    public void Serialize(IEmitter emitter, object graph)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();
        serializer.Serialize(emitter, graph);
    }

    public void Serialize(IEmitter emitter, object graph, Type type)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();
        serializer.Serialize(emitter, graph);
    }
}

public interface IYamlSerializer : ISerializer
{
}