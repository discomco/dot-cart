// ***********************************************************************
// <copyright file="TypeExtensions.cs" company="Flint Group">
//     Copyright (c) Flint Group. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;

namespace DotCart;

public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<Type, string> PrettyPrintCache =
        new();

    public static IEnumerable<Type> KnownTypes(this Type type)
    {
        return Attribute
            .GetCustomAttributes(type)
            .OfType<KnownTypeAttribute>()
            .Select(attr => attr.Type)!;
    }


    public static Stream GetEmbeddedFile(this Type type, string fileName)
    {
        return AssemblyUtils.GetEmbeddedFile(type.Assembly, fileName);
    }

    public static XmlDocument GetEmbeddedXml(this Type type, string fileName)
    {
        var str = GetEmbeddedFile(type, fileName);
        var tr = new XmlTextReader(str);
        var xml = new XmlDocument();
        xml.Load(tr);
        return xml;
    }

    public static string PrettyPrint(this Type type)
    {
        return PrettyPrintCache.GetOrAdd(
            type,
            t =>
            {
                try
                {
                    return PrettyPrintRecursive(t, 0);
                }
                catch (Exception)
                {
                    return t.Name;
                }
            });
    }

    private static string PrettyPrintRecursive(Type type, int depth)
    {
        if (depth > 3) return type.Name;

        var nameParts = type.Name.Split('`');
        if (nameParts.Length == 1) return nameParts[0];

        var genericArguments = type.GetTypeInfo().GetGenericArguments();
        return !type.IsConstructedGenericType
            ? $"{nameParts[0]}<{new string(',', genericArguments.Length - 1)}>"
            : $"{nameParts[0]}<{string.Join(",", genericArguments.Select(t => PrettyPrintRecursive(t, depth + 1)))}>";
    }
}