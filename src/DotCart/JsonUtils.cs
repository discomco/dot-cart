using System.Text.Json;

namespace DotCart;

public static class JsonUtils
{
    public static T? FromBytes<T>(this byte[] data)
    {
        var jsonUtfReader = new Utf8JsonReader(data);
        return JsonSerializer.Deserialize<T>(ref jsonUtfReader);
    }

    public static byte[] ToBytes<T>(this T obj)
    {
        return obj == null
            ? Array.Empty<byte>()
            : JsonSerializer.SerializeToUtf8Bytes(obj);
    }
}