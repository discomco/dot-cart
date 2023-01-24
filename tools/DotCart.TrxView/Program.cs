using System.Xml;
using System.Xml.Xsl;


namespace Trxer.CLI;

internal static class Program
{
    /// <summary>
    /// Embedded Resource name
    /// </summary>
    private const string XSLT_FILE = "Trxer.xslt";
    /// <summary>
    /// Trxer output format
    /// </summary>
    private const string OUTPUT_FILE_EXT = ".html";

    /// <summary>
    /// Main entry of TrxerConsole
    /// </summary>
    /// <param name="args">First cell shoud be TRX path</param>
    static void Main(string[] args)
    {
        if (args.Any() == false)
        {
            Console.WriteLine("No trx file,  Trxer.exe <filename>");
            return;
        }
        Console.WriteLine("Trx File\n{0}", args[0]);
        Transform(args[0], PrepareXsl());
    }

    /// <summary>
    /// Transforms trx int html document using xslt
    /// </summary>
    /// <param name="fileName">Trx file path</param>
    /// <param name="xsl">Xsl document</param>
    private static void Transform(string fileName, XmlDocument xsl)
    {
        XslCompiledTransform x = new XslCompiledTransform(true);
        x.Load(xsl, new XsltSettings(true, true), null);
        Console.WriteLine("Transforming...");
        x.Transform(fileName, fileName + OUTPUT_FILE_EXT);
        Console.WriteLine("Done transforming xml into html");
    }

    /// <summary>
    /// Loads xslt form embedded resource
    /// </summary>
    /// <returns>Xsl document</returns>
    private static XmlDocument PrepareXsl()
    {
        XmlDocument xslDoc = new XmlDocument();
        Console.WriteLine("Loading xslt template...");
        xslDoc.Load(ResourceReader.StreamFromResource(XSLT_FILE)!);
        MergeCss(xslDoc);
        MergeJavaScript(xslDoc);
        return xslDoc;
    }

    /// <summary>
    /// Merges all javascript linked to page into Trxer html report itself
    /// </summary>
    /// <param name="xslDoc">Xsl document</param>
    private static void MergeJavaScript(XmlDocument xslDoc)
    {
        Console.WriteLine("Loading javascript...");
        var scriptEl = xslDoc.GetElementsByTagName("script")[0];
        var scriptSrc = scriptEl.Attributes["src"];
        var script = ResourceReader.LoadTextFromResource(scriptSrc.Value);
        scriptEl.Attributes.Remove(scriptSrc);
        scriptEl.InnerText = script;
    }

    /// <summary>
    /// Merges all css linked to page ito Trxer html report itself
    /// </summary>
    /// <param name="xslDoc">Xsl document</param>
    private static void MergeCss(XmlDocument xslDoc)
    {
        Console.WriteLine("Loading css...");
        var headNode = xslDoc.GetElementsByTagName("head")[0];
        var linkNodes = xslDoc.GetElementsByTagName("link");
        var toChangeList = linkNodes.Cast<XmlNode>().ToList();

        foreach (var xmlElement in toChangeList)
        {
            var styleEl = xslDoc.CreateElement("style");
            styleEl.InnerText = ResourceReader.LoadTextFromResource(xmlElement.Attributes["href"].Value);
            headNode.ReplaceChild(styleEl, xmlElement);
        }
    }
}