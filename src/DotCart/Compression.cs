using System.IO.Compression;

namespace DotCart;

public static class Compression
{
    public static FileInfo Compress(FileInfo fi)
    {
        // Get the stream of the source file.
        using var inFile = fi.OpenRead();
        // Prevent compressing hidden and already compressed files.
        if (!(((File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden) &
              (fi.Extension != ".gz"))) return null;
        var compressedFilePath = $"{fi.FullName}.gz";
        // Create the compressed file.
        using var outFile = File.Create(compressedFilePath);
        using var compress = new GZipStream(outFile, CompressionMode.Compress);
        // Copy the source file into the compression stream.
        inFile.CopyTo(compress);
        return new FileInfo(compressedFilePath);
    }
    public static FileInfo Decompress(FileInfo fi)
    {
        // Get the stream of the source file.
        using var inFile = fi.OpenRead();
        // Get original file name
        var compressedFilePath = fi.FullName;
        var decompressedFilePath = compressedFilePath.Remove(compressedFilePath.Length - fi.Extension.Length);
        // Create the decompressed file.
        using var outFile = File.Create(decompressedFilePath);
        using var decompress = new GZipStream(inFile, CompressionMode.Decompress);
        // Copy the decompression stream into the output file.
        decompress.CopyTo(outFile);

        return new FileInfo(decompressedFilePath);
    }
    public static FileInfo Decompress(MemoryStream ms, string fileName)
    {
        // Get original file name
        var decompressedFilePath = Path.Combine(PathUtils.InternetCacheDir(), fileName.Remove(fileName.Length - 3));
        // Create the decompressed file.
        using var outFile = File.Create(decompressedFilePath);
        using var decompress = new GZipStream(ms, CompressionMode.Decompress);
        // Copy the decompression stream into the output file.
        decompress.CopyTo(outFile);
        return new FileInfo(decompressedFilePath);
    }
    public static FileInfo Decompress(MemoryStream ms, string fileName, string temporaryPath)
    {
        // Get original file name
        var decompressedFilePath = Path.Combine(temporaryPath, fileName.Remove(fileName.Length - 3));

        // Create the decompressed file.
        using var outFile = File.Create(decompressedFilePath);
        using var decompress = new GZipStream(ms, CompressionMode.Decompress);
        // Copy the decompression stream into the output file.
        decompress.CopyTo(outFile);
        return new FileInfo(decompressedFilePath);
    }
}