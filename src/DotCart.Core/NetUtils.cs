using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DotCart.Core;

public static class NetUtils
{
    public static IPAddress[] GetIp(string hostName)
    {
        var ipEntry = Dns.GetHostEntry(hostName);
        var addr = ipEntry.AddressList;

        return addr;
    }

    public static IPAddress[] GetIp()
    {
        var hostName = Dns.GetHostName();

        return GetIp(hostName);
    }

    public static IPAddress[] GetIPv4AssociatedWithLocalHost()
    {
        var hostName = Dns.GetHostName();
        const bool pv4Only = true;
        return GetIPs(hostName, pv4Only);
    }

    public static string GetFirstIp4(string hostName)
    {
        const bool pv4Only = true;
        var ips = GetIPs(hostName, pv4Only);
        if (ips.Length > 0) return ips[0].ToString();
        return string.Empty;
    }

    public static IPAddress[] GetIPs(string hostName, bool pv4Only)
    {
        var hostEntry = Dns.GetHostEntry(hostName);
        var addressList = hostEntry.AddressList;

        if (pv4Only)
        {
            var ipList = new List<IPAddress>();
            foreach (var iPAddress in addressList)
                if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
                    ipList.Add(iPAddress);
            return ipList.ToArray();
        }

        return addressList;
    }

    public static string GetHostName()
    {
        return Dns.GetHostName();
    }

    public static IPAddress[] GetIPv4()
    {
        List<IPAddress> addr; // Hold existing Ip v4 addresses.
        NetworkInterface[] interfaces; // Hold network interfaces.

        // Get list of all interfaces.
        interfaces = NetworkInterface.GetAllNetworkInterfaces();
        addr = new List<IPAddress>();

        // Loop through interfaces.
        foreach (var iface in interfaces)
        {
            // Create collection to hold IP information for interfaces.
            UnicastIPAddressInformationCollection ips;

            // Get list of all unicast IPs from current interface.
            ips = iface.GetIPProperties().UnicastAddresses;

            // Loop through IP address collection.
            addr.AddRange(from ip in ips
                where ip.Address.AddressFamily == AddressFamily.InterNetwork
                select ip.Address);
        }

        return addr.ToArray();
    }

    public static int GetFirstUnusedPort(IPAddress localAddr)
    {
        for (var p = 1024; p <= IPEndPoint.MaxPort; p++)
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // If the port is used, an exception 10048 will be raised when we try to bind to...
                s.Bind(new IPEndPoint(localAddr, p));
                s.Close();
                return p;
            }
            catch (SocketException ex)
            {
                // Address in use ?
                if (ex.ErrorCode == 10048)
                    continue;
                return 0;
            }
        }

        return 0;
    }

    /// <summary>
    ///     Get the first unused port above startPort.
    /// </summary>
    /// <param name="localAddr">Ip to bind to</param>
    /// <param name="startPort">Starting port</param>
    /// <returns>Port number, 0 in case of error</returns>
    public static int GetFirstUnusedPort(IPAddress localAddr, int startPort)
    {
        for (var p = startPort; p <= IPEndPoint.MaxPort; p++)
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // If the port is used, an exception 10048 will be raised when we try to bind to...
                s.Bind(new IPEndPoint(localAddr, p));
                s.Close();
                return p;
            }
            catch (SocketException ex)
            {
                // Address in use ?
                if (ex.ErrorCode == 10048)
                    continue;
                return 0;
            }
        }

        return 0;
    }


    /// <summary>
    ///     Get the first unused port between startPort and endPort.
    /// </summary>
    /// <param name="localAddr">Ip to bind to</param>
    /// <param name="startPort">Starting port</param>
    /// <param name="endPort">Ending port</param>
    /// <returns>Port number, 0 in case of error</returns>
    public static int GetFirstUnusedPort(IPAddress localAddr, int startPort, int endPort)
    {
        for (var p = startPort; p <= endPort; p++)
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // If the port is used, an exception 10048 will be raised when we try to bind to...
                s.Bind(new IPEndPoint(localAddr, p));
                s.Close();
                return p;
            }
            catch (SocketException ex)
            {
                // Address in use ?
                if (ex.ErrorCode == 10048)
                    continue;
                return 0;
            }
        }

        return 0;
    }

    /// <summary>
    ///     Verify is a computer + port is connectable on the network.
    /// </summary>
    /// <param name="addr">Ip to bind to</param>
    /// <param name="port">Starting port</param>
    /// <returns>Port number, 0 in case of error</returns>
    public static bool IsConnectable(string addr, int port)
    {
        var connector = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //      IPEndPoint host = new IPEndPoint(IPAddress.Parse(addr), (int)port);

        try
        {
            connector.Blocking = false;
            connector.Connect(addr, port);

            if (connector.Poll(1000, SelectMode.SelectRead)) return true;

            return false;
            throw new Exception("Timeout detected");
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    ///     Compresses the given data using GZip algorithm.
    /// </summary>
    /// <param name="data">The data to be compressed.</param>
    /// <returns>The compressed data</returns>
    public static byte[] CompressGZip(byte[] data)
    {
        Stream memoryStream = new MemoryStream();
        var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
        //Stream gZipStream = new GZipOutputStream(memoryStream);  // Does not work, only returns a header of 10 bytes
        gZipStream.Write(data, 0, data.Length);
        gZipStream.Close();

        // Reposition memoryStream to the beginning
        memoryStream.Position = 0;

        var compressedData = new byte[memoryStream.Length];
        memoryStream.Read(compressedData, 0, (int)memoryStream.Length);

        memoryStream.Close();

        return compressedData;
    }

    /// <summary>
    ///     Decompresses the given data using GZip algorithm.
    /// </summary>
    /// <param name="data">The data to be decompressed.</param>
    /// <returns>The decompressed data</returns>
    public static byte[] DecompressGZip(byte[] data)
    {
        Stream compressedMemoryStream = new MemoryStream(data);
        var gZipStream = new GZipStream(compressedMemoryStream, CompressionMode.Decompress, true);
        Stream decompressedMemoryStream = new MemoryStream(data.Length);
        int byteRead;
        //while ((byteRead = compressedMemoryStream.ReadByte()) != -1)
        while ((byteRead = gZipStream.ReadByte()) != -1) decompressedMemoryStream.WriteByte((byte)byteRead);
        gZipStream.Close();
        compressedMemoryStream.Close();

        // Reposition memoryStream to the beginning
        decompressedMemoryStream.Position = 0;

        var decompressedData = new byte[decompressedMemoryStream.Length];
        decompressedMemoryStream.Read(decompressedData, 0, (int)decompressedMemoryStream.Length);
        decompressedMemoryStream.Close();

        return decompressedData;
    }

    public static IPAddress GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip;
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public record IpRange
    {
        private uint _ipEnd; // Last IP address of the range.
        private uint _ipStart; // First IP address of the range.

        public IpRange(uint pIpStart, uint pIpEnd)
        {
            _ipStart = pIpStart;
            _ipEnd = pIpEnd;
        }

        public IpRange(string pIpStart, string pIpEnd)
        {
            _ipStart = IpAddressToUInt(pIpStart);
            _ipEnd = IpAddressToUInt(pIpEnd);
        }

        public IpRange(IPAddress pIpStart, IPAddress pIpEnd)
        {
            _ipStart = IpAddressToUInt(pIpStart);
            _ipEnd = IpAddressToUInt(pIpEnd);
        }

        public IPAddress IpStart
        {
            get => UIntToIpAddress(_ipStart);
            set => _ipStart = IpAddressToUInt(value);
        }

        public IPAddress IpEnd
        {
            get => UIntToIpAddress(_ipEnd);
            set => _ipEnd = IpAddressToUInt(value);
        }

        public int Contains(IPAddress pIp)
        {
            var ip = IpAddressToUInt(pIp);

            return Contains(ip);
        }

        public int Contains(string pIp)
        {
            var ip = IpAddressToUInt(pIp);

            return Contains(ip);
        }

        public int Contains(uint pIp)
        {
            if (_ipStart <= pIp && _ipEnd >= pIp) return 0;

            if (_ipStart > pIp) return -1;

            if (_ipEnd < pIp) return 1;

            return -1;
        }

        public static uint IpAddressToUInt(IPAddress pIp)
        {
            // Split ip address in 4 element array consisting of the octets in the IP string.
            var byteIp = pIp.GetAddressBytes();

            // Shift the first octet over 24 bits so that it's in the highest position.
            // Thus if addressOctets[0] is 0x8D, addressValue will be 0x8D000000.
            var ip = (uint)byteIp[3] << 24;
            ip += (uint)byteIp[2] << 16;
            ip += (uint)byteIp[1] << 8;
            ip += byteIp[0];

            return ip;
        }

        public static uint IpAddressToUInt(string pIp)
        {
            var oIp = IPAddress.Parse(pIp);

            return IpAddressToUInt(oIp);
        }

        public static string UIntToCanonicalIpAddress(uint pIp)
        {
            return new IPAddress(pIp).ToString();
        }

        public static IPAddress UIntToIpAddress(uint pIp)
        {
            return new IPAddress(pIp);
        }

        private static uint IpAddressToUIntBackwards(string pIp)
        {
            var oIp = IPAddress.Parse(pIp);
            var byteIp = oIp.GetAddressBytes();


            var ip = (uint)byteIp[0] << 24;
            ip += (uint)byteIp[1] << 16;
            ip += (uint)byteIp[2] << 8;
            ip += byteIp[3];

            return ip;
        }
    }
} // End NetUtils