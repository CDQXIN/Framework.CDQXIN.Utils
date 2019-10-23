using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.ExtensionHelper
{
    /// <summary>
    /// Byte扩展方法
    /// </summary>
    public static class ByteExtHelper
    {
        /// <summary>
        /// 转化为16进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                foreach (byte t in bytes)
                {
                    strB.Append(t.ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 转化为16进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexIs0X(this byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                foreach (byte t in bytes)
                {
                    strB.Append(t.ToString("X2"));
                }
                hexString = strB.ToString();
            }

            return string.IsNullOrEmpty(hexString) ? hexString : "0x" + hexString;
        }

        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData">原始数据</param>
        /// <returns>压缩后的数据</returns>
        public static byte[] Compress(this byte[] rawData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    compressedzipStream.Write(rawData, 0, rawData.Length);
                    compressedzipStream.Close();
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// GZip解压
        /// </summary>
        /// <param name="zippedData">压缩数据</param>
        /// <returns>解压后的数据</returns>
        public static byte[] Decompress(this byte[] zippedData)
        {
            using (MemoryStream ms = new MemoryStream(zippedData))
            {
                using (GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress))
                {
                    using (MemoryStream outBuffer = new MemoryStream())
                    {
                        byte[] block = new byte[1024];
                        while (true)
                        {
                            int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                            if (bytesRead <= 0)
                            {
                                break;
                            }
                            else
                            {
                                outBuffer.Write(block, 0, bytesRead);
                            }
                        }
                        compressedzipStream.Close();
                        return outBuffer.ToArray();
                    }
                }
            }
        }
    }
}
