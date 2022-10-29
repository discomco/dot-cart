using System.Text.Json;

namespace DotCart.Schema;

[Serializable]
public sealed record ErrorState
{
    public bool IsSuccessful => !Errors.Any();
    public Errors Errors { get; } = new();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}