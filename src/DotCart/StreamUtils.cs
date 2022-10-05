namespace DotCart;

public static class StreamUtils
{
   
    public sealed class StreamWithProgress : Stream
    {
        private readonly FileStream _file;
        private readonly long _length;
        private long _bytesRead;
        public StreamWithProgress(FileStream file)
        {
            _file = file;
            _length = file.Length;
            _bytesRead = 0;
            DoProgressChanged(_bytesRead, _length);
        }
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new Exception("The method or operation is not implemented.");
        public override long Position
        {
            get => _bytesRead;
            set => throw new Exception("The method or operation is not implemented.");
        }
        public event EventHandler<ProgressChangedEventArgs> OnProgressChanged;
        private void DoProgressChanged(long bytesRead, long length)
        {
            OnProgressChanged?.Invoke(this, new ProgressChangedEventArgs(bytesRead, length));
        }
        public double GetProgress()
        {
            return (double)_bytesRead / _file.Length;
        }
        public override void Flush()
        {
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            var result = _file.Read(buffer, offset, count);
            _bytesRead += result;
            OnProgressChanged?.Invoke(this, new ProgressChangedEventArgs(_bytesRead, _length));
            return result;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    
    
    public class ProgressChangedEventArgs : EventArgs
    {
        public long BytesRead;
        public long Length;
        public ProgressChangedEventArgs(long bytesRead, long length)
        {
            BytesRead = bytesRead;
            Length = length;
        }
    }
    
    public static Stream ToStream(this string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Seek(0, 0);
        return stream;
    }

    
    
    public static byte[] ToBytes(this Stream stream)
    {
        long originalPosition = 0;

        if (stream.CanSeek)
        {
            originalPosition = stream.Position;
            stream.Position = 0;
        }

        try
        {
            var readBuffer = new byte[4096];

            var totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead != readBuffer.Length) continue;
                var nextByte = stream.ReadByte();
                if (nextByte == -1) continue;
                var temp = new byte[readBuffer.Length * 2];
                Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                readBuffer = temp;
                totalBytesRead++;
            }

            var buffer = readBuffer;
            if (readBuffer.Length == totalBytesRead) return buffer;
            buffer = new byte[totalBytesRead];
            Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            return buffer;
        }
        finally
        {
            if (stream.CanSeek) stream.Position = originalPosition;
        }
    }
    public static string AsString(this Stream sIn)
    {
        if (sIn.CanSeek)
            sIn.Position = 0;
        var sr = new StreamReader(sIn);
        var s = sr.ReadToEnd();
        return s;
    }
    public static string AsBase64String(this Stream sIn)
    {
        return Convert.ToBase64String(sIn.ToByteArray());
    }
    public static void CopyStream(Stream input, Stream output)
    {
        var b = new byte[32768];
        int r;
        while ((r = input.Read(b, 0, b.Length)) > 0)
            output.Write(b, 0, r);
    }


    public static byte[] ToByteArray(this Stream sIn)
    {
        if (sIn == null) return null;
        sIn.Seek(0, 0);
        var mem = new MemoryStream();
        CopyStream(sIn, mem);
        var buffer = mem.GetBuffer();
        var length = mem.Length; // the actual length of the data 
        return mem.ToArray(); // makes another copy
    }
}