using System.Text;

namespace DotCart;

public static class HexUtils {
    private static readonly char[] HexChars = "0123456789ABCDEF".ToCharArray();

    static HexUtils()
    {
        Array.Sort(HexChars);
    }
    public static byte[] GetBytesFromHex(string hexString) {
        if (!IsValidHexCharLength(hexString)) throw new ArgumentException("Invalid hexString size");
        // Check for non hex characters
        var tempString = new StringBuilder(hexString.Length);
        foreach (var c in hexString)
            if (IsHexChar(c))
                tempString.Append(c);
            else
                throw new ArgumentException("Non Hexadecimal character '" + c + "' in hexString");
        var verifiedHexString = tempString.ToString();
        if (verifiedHexString.Length % 2 != 0)
            verifiedHexString = verifiedHexString.Substring(0, verifiedHexString.Length - 1);
        var byteArrayLength = verifiedHexString.Length / 2;
        var hexbytes = new byte[byteArrayLength];
        try
        {
            for (var i = 0; i < hexbytes.Length; i++)
            {
                var charIndex = i * 2;
                var tmpSubstring = verifiedHexString.Substring(charIndex, 2);
                hexbytes[i] = Convert.ToByte(tmpSubstring, 16);
            }
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("hexString must be a valid hexadecimal", ex);
        }
        return hexbytes;
    }

    public static string GetStringFromHex(string hexString) {
        var ch = GetBytesFromHex(hexString);
        return Encoding.ASCII.GetString(ch);
    }
    public static string ToHexString(byte[] bytes) {
        if (bytes == null || bytes.Length == 0) throw new ArgumentNullException("bytes");
        var hexString = new StringBuilder();
        foreach (var byt in bytes) hexString.Append(byt.ToString("X2"));
        return hexString.ToString();
    }

    public static string ToHexString(string clearString) {
        var ab = Encoding.ASCII.GetBytes(clearString);
        return ToHexString(ab);
    }

    public static bool IsHexString(string hexString) {
        var hexFormat = IsValidHexCharLength(hexString);
        if (!hexFormat) return hexFormat;
        return hexString.All(IsHexChar) && hexFormat;
    }
    private static bool IsValidHexCharLength(string hexString) {
        return hexString is { Length: >= 2 };
    }

    public static bool IsHexChar(char c) {
        c = char.ToUpper(c);
        // Look-up the char in HEX_CHARS Array
        return Array.BinarySearch(HexChars, c) >= 0;
    }
    
    public static byte[] CreateLegalByteArray(byte[] inputBytes, int requiredSize) {
        byte[] newBytes = null;
        var inputLength = inputBytes.Length;
        if (inputLength == requiredSize) {
            newBytes = inputBytes;
        }
        else {
            newBytes = new byte[requiredSize];
            var len = newBytes.Length;
            if (len > inputLength) len = inputLength;
            Array.Copy(inputBytes, newBytes, len); //note: balance is filled with 0 (zero)
        }
        return newBytes;
    }
}