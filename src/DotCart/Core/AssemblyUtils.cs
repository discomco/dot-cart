////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c)2023 DisComCo Sp.z.o.o. (http://discomco.pl)
////////////////////////////////////////////////////////////////////////////////////////////////
// Permission is hereby granted, free of charge,
// to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS",
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////

using System.Reflection;

namespace DotCart.Core;

public static class AssemblyUtils
{
    public static IEnumerable<Module> GetLoadedModules()
    {
        var asm = Assembly.GetExecutingAssembly();
        return asm.GetLoadedModules();
    }

    public static string GetVersion(this Assembly assembly)
    {
        return assembly.GetName().Version?.ToString() ?? string.Empty;
    }

    public static string ShortName(this Assembly assembly)
    {
        return assembly.FullName?.Split(',')[0] ?? string.Empty;
    }

    public static Stream GetEmbeddedFile(this Assembly assembly, string fileName)
    {
        fileName = fileName.Replace('/', '.');
        fileName = fileName.Replace('\\', '.');
        var assemblyName = ShortName(assembly);
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

    public static Stream GetEmbeddedFileFromNamedAssembly(string assemblyName, string fileName)
    {
        try
        {
            var a = Assembly.Load(assemblyName);
            return GetEmbeddedFile(a, fileName);
        }
        catch (Exception e)
        {
            throw new Exception(assemblyName + ": " + e.Message);
        }
    }
}