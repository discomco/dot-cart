using System.Text.Json;

namespace DotCart.Core;

public static class JsonUtils
{

    public static bool IsJson(this string source)
    {
        if (source == null)
            return false;
        try
        {
            JsonDocument.Parse(source);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public static T? FromBytes<T>(this byte[] data)
    {
        var jsonUtfReader = new Utf8JsonReader(data);
        return JsonSerializer.Deserialize<T>(ref jsonUtfReader);
    }

    public static object ToObject<T>(this byte[] data)
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

    public static string ToJson<T>(this T obj)
    {
        return obj == null
            ? string.Empty
            : JsonSerializer.Serialize(obj);
    }

    public static T FromJson<T>(this string json)
    {
        return string.IsNullOrWhiteSpace(json)
            ? default(T)
            : JsonSerializer.Deserialize<T>(json);
    }


    public static byte[] ToUtf8(object obj)
    {
        return obj == null
            ? Array.Empty<byte>()
            : JsonSerializer.SerializeToUtf8Bytes(obj);
    }

    public static T FromUtf8<T>(this byte[] bytes)
    {
        if (bytes == null) return default;
        var span = new ReadOnlySpan<byte>(bytes);
        return JsonSerializer.Deserialize<T>(span);
    }
}