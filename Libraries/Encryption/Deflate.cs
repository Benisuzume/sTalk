using System.IO;
using System.IO.Compression;

namespace sTalk.Libraries.Encryption
{
    public static class Deflate
    {
        public static byte[] Compress(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                using (var ds = new DeflateStream(ms, CompressionMode.Compress))
                {
                    ds.Write(bytes, 0, bytes.Length);
                }

                return ms.ToArray();
            }
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using (var compressed = new MemoryStream(bytes))
            {
                using (var ds = new DeflateStream(compressed, CompressionMode.Decompress))
                {
                    using (var ms = new MemoryStream())
                    {
                        var buffer = new byte[4096];

                        var count = 0;
                        do
                        {
                            count = ds.Read(buffer, 0, 4096);
                            if (count == 0)
                                break;

                            ms.Write(buffer, 0, count);
                        } while (count == 4096);

                        return ms.ToArray();
                    }
                }
            }
        }
    }
}