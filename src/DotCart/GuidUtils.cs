using System.Security.Cryptography;
using System.Text;

namespace DotCart;

public static class GuidUtils
{
    public const string TEST_GUID = "7e577e57-7e57-7e57-7e57-7e577e577e57";
    public static string NewCleanGuidLower = NewCleanGuid.ToLowerInvariant();
    public static string NewCleanGuid => Guid.NewGuid().ToString("N");
    public static string NullCleanGuid => Guid.Empty.ToString("N");
    public static string NewGuid => Guid.NewGuid().ToString();
    public static string LowerCaseGuid => NewGuid.ToLowerInvariant();

    public static Guid ToGuid(this int value)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }

    public static int ToInt(this Guid value)
    {
        var b = value.ToByteArray();
        var bint = BitConverter.ToInt32(b, 0);
        return bint;
    }


    public static Guid AnyStringToGuid(this string anyString)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        var md5Hasher = MD5.Create();
        // Convert the input string to a byte array and compute the hash.
        var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(anyString));
        return new Guid(data);
    }

    public static Guid FromDecimal(this decimal value)
    {
        return new Guid(value.ToByteArray());
    }

    public static class Comb
    {
        public static Guid Create()
        {
            var destinationArray = Guid.NewGuid().ToByteArray();
            var time = new DateTime(0x76c, 1, 1);
            var now = DateTime.Now;
            var span = new TimeSpan(now.Ticks - time.Ticks);
            var timeOfDay = now.TimeOfDay;
            var bytes = BitConverter.GetBytes(span.Days);
            var array = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));

            Array.Reverse(bytes);
            Array.Reverse(array);
            Array.Copy(bytes, bytes.Length - 2, destinationArray, destinationArray.Length - 6, 2);
            Array.Copy(array, array.Length - 4, destinationArray, destinationArray.Length - 4, 4);

            return new Guid(destinationArray);
        }

        public static Guid Empty()
        {
            return Guid.Empty;
        }
    }

    /// <summary>
    ///     Creates a name-based UUID using the algorithm from RFC 4122 §4.3.
    ///     http://code.logos.com/blog/2011/04/generating_a_deterministic_guid.html
    /// </summary>
    public static class Deterministic
    {
        public static Guid Create(Guid namespaceId, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            // Convert the name to a sequence of octets (as defined by the standard or conventions of its namespace) (step 3)
            // ASSUME: UTF-8 encoding is always appropriate
            var nameBytes = Encoding.UTF8.GetBytes(name);

            return Create(namespaceId, nameBytes);
        }

        public static Guid Create(Guid namespaceId, byte[] nameBytes)
        {
            // Always use version 5 (version 3 is MD5, version 5 is SHA1)
            const int version = 5;

            if (namespaceId == default) throw new ArgumentNullException(nameof(namespaceId));
            if (nameBytes.Length == 0) throw new ArgumentNullException(nameof(nameBytes));

            // Convert the namespace UUID to network order (step 3)
            var namespaceBytes = namespaceId.ToByteArray();
            SwapByteOrder(namespaceBytes);

            // Compute the hash of the name space ID concatenated with the name (step 4)
            byte[] hash;
            using (var algorithm = SHA1.Create())
            {
                var combinedBytes = new byte[namespaceBytes.Length + nameBytes.Length];
                Buffer.BlockCopy(namespaceBytes, 0, combinedBytes, 0, namespaceBytes.Length);
                Buffer.BlockCopy(nameBytes, 0, combinedBytes, namespaceBytes.Length, nameBytes.Length);

                hash = algorithm.ComputeHash(combinedBytes);
            }

            // Most bytes from the hash are copied straight to the bytes of the new
            // GUID (steps 5-7, 9, 11-12)
            var newGuid = new byte[16];
            Array.Copy(hash, 0, newGuid, 0, 16);

            // Set the four most significant bits (bits 12 through 15) of the time_hi_and_version
            // field to the appropriate 4-bit version number from Section 4.1.3 (step 8)
            newGuid[6] = (byte)((newGuid[6] & 0x0F) | (version << 4));

            // Set the two most significant bits (bits 6 and 7) of the clock_seq_hi_and_reserved
            // to zero and one, respectively (step 10)
            newGuid[8] = (byte)((newGuid[8] & 0x3F) | 0x80);

            // Convert the resulting UUID to local byte order (step 13)
            SwapByteOrder(newGuid);
            return new Guid(newGuid);
        }

        private static void SwapByteOrder(IList<byte> guid)
        {
            SwapBytes(guid, 0, 3);
            SwapBytes(guid, 1, 2);
            SwapBytes(guid, 4, 5);
            SwapBytes(guid, 6, 7);
        }

        private static void SwapBytes(IList<byte> guid, int left, int right)
        {
            (guid[left], guid[right]) = (guid[right], guid[left]);
        }

        public static class Namespaces
        {
            public static readonly Guid Events = Guid.Parse("387F5B61-9E98-439A-BFF1-15AD0EA91EA0");
            public static readonly Guid Snapshots = Guid.Parse("B61D4D52-B546-43C2-A67F-754DCBD31586");
            public static readonly Guid Commands = Guid.Parse("4286D89F-7F92-430B-8E00-E468FE3C3F59");
        }
    }
}