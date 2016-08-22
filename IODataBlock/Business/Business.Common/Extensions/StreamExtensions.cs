using System.IO;

namespace Business.Common.Extensions
{
    public static class StreamExtensions
    {
        public static void BufferCopyStream(this byte[] data, Stream outputStream, int bufferSize = 4096)
        {
            var inputStream = new MemoryStream(data);
            if (inputStream.CanSeek) inputStream.Seek(0, SeekOrigin.Begin);
            int readCount;
            var buffer = new byte[bufferSize];
            while ((readCount = inputStream.Read(buffer, 0, bufferSize)) != 0)
                outputStream.Write(buffer, 0, readCount);
        }

        public static void BufferCopyStream(this Stream inputStream, Stream outputStream, int bufferSize = 4096)
        {
            if (inputStream.CanSeek) inputStream.Seek(0, SeekOrigin.Begin);
            int readCount;
            var buffer = new byte[bufferSize];
            while ((readCount = inputStream.Read(buffer, 0, bufferSize)) != 0)
                outputStream.Write(buffer, 0, readCount);
        }
    }
}