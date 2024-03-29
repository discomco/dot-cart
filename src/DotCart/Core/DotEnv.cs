using System.Reflection;

namespace DotCart.Core;

public static class DotEnv
{
    public static bool IsDevelopment =>
        string.IsNullOrEmpty(Get(EnVars.ASPNETCORE_ENVIRONMENT)) &&
        Get("ASPNETCORE_ENVIRONMENT")!
            .Equals("Development");

    public static void FromEmbedded(Assembly assembly, string name = ".env")
    {
        var sIn = GetEmbeddedFile(assembly, name);
        var lines = AsString(sIn).Split('\n');
        LoadEnv(lines);
    }

    public static Stream GetEmbeddedFile(this Assembly assembly, string fileName)
    {
        var assemblyName = assembly.ShortName();
        try
        {
            var str = assembly.GetManifestResourceStream($"{assemblyName}.{fileName}");
            if (str == null)
                throw new Exception("Could not locate embedded resource '" + fileName + "' in assembly '" +
                                    assemblyName + "'");
            return str;
        }
        catch (Exception e)
        {
            throw new Exception(assemblyName + ": " + e.Message);
        }
    }


    public static string AsString(this Stream sIn)
    {
        if (sIn.CanSeek)
            sIn.Position = 0;
        var sr = new StreamReader(sIn);
        var s = sr.ReadToEnd();
        return s;
    }


    public static void FromFile(string filePath)
    {
        if (!File.Exists(filePath))
            return;
        LoadEnv(File.ReadAllLines(filePath));
    }

    private static void LoadEnv(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                continue;
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }

    public static void FromEmbedded(string fileName = ".env")
    {
        var assy = Assembly.GetCallingAssembly();
        FromEmbedded(assy, fileName);
    }

    public static string? Get(string name)
    {
        var s = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrWhiteSpace(s)) return s;
        if (s.StartsWith('"')) s = s.Split('"')[1];
        if (s.StartsWith("'")) s = s.Split('"')[1];
        return s;
    }

    public static string? Set(string name, object value)
    {
        Environment.SetEnvironmentVariable(name, Convert.ToString(value));
        return Get(name);
    }

    public static string Expand(string variables)
    {
        return Environment.ExpandEnvironmentVariables(variables);
    }
}