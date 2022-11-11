using System.Reflection;

namespace DotCart.Core;

public static class ResourceUtils
{
    public static string GetEmbeddedResource(string resourceName, Assembly assembly)
    {
        if (assembly == null) return string.Empty;
        var newResourceName = FormatResourceName(assembly, resourceName);
        using var resourceStream = assembly.GetManifestResourceStream(newResourceName);
        if (resourceStream == null) return string.Empty;
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