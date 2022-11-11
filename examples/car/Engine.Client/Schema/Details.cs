namespace Engine.Client.Schema;

public record Details(string Name = "new engine", string Description = "")
{
    public static Details New(string name, string description = "")
    {
        return new Details(name, description);
    }
}