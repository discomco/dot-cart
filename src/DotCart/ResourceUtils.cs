using System.Reflection;

namespace DotCart;

public static class ResourceUtils
{
    public static string GetEmbeddedResource(string resourceName, Assembly assembly)
    {
        var newResourceName = FormatResourceName(assembly, resourceName);
        using var resourceStream = assembly.GetManifestResourceStream(newResourceName);
        if (resourceStream == null)
            return null;
        using var reader = new StreamReader(resourceStream);
        return reader.ReadToEnd();
    }


    public static Stream GetEmbeddedResourceStream(string resourceName, Assembly assembly)
    {
        var newResourceName = FormatResourceName(assembly, resourceName);
        return assembly.GetManifestResourceStream(newResourceName);
    }


    private static string FormatResourceName(Assembly assembly, string resourceName)
    {
        return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
            .Replace("\\", ".")
            .Replace("/", ".");
    }
}