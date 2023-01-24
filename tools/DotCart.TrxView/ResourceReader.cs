using System.Reflection;

namespace Trxer.CLI
{
    internal class ResourceReader
    {
        internal static string LoadTextFromResource(string name)
        {
            var result = string.Empty;
            using StreamReader sr = new StreamReader(
                StreamFromResource(name)!);
            return sr.ReadToEnd();
        }

        public static Stream? StreamFromResource(string name)
        {
           
            return Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Trxer.CLI." + name);
        }
    }
}
