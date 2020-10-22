using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// Byte扩展方法
	/// </summary>
	public static class ByteExt
	{
		/// <summary>
		/// 转化为16进制
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string ToHex(this byte[] bytes)
		{
			string result = string.Empty;
			if (bytes != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					byte b = bytes[i];
					stringBuilder.Append(b.ToString("X2"));
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
		/// <summary>
		/// 转化为16进制
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string ToHexIs0X(this byte[] bytes)
		{
			string text = string.Empty;
			if (bytes != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					byte b = bytes[i];
					stringBuilder.Append(b.ToString("X2"));
				}
				text = stringBuilder.ToString();
			}
			if (!string.IsNullOrEmpty(text))
			{
				return "0x" + text;
			}
			return text;
		}
		/// <summary>
		/// GZip压缩
		/// </summary>
		/// <param name="rawData">原始数据</param>
		/// <returns>压缩后的数据</returns>
		public static byte[] Compress(this byte[] rawData)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
				{
					gZipStream.Write(rawData, 0, rawData.Length);
					gZipStream.Close();
					result = memoryStream.ToArray();
				}
			}
			return result;
		}
		/// <summary>
		/// GZip解压
		/// </summary>
		/// <param name="zippedData">压缩数据</param>
		/// <returns>解压后的数据</returns>
		public static byte[] Decompress(this byte[] zippedData)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(zippedData))
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						byte[] array = new byte[1024];
						while (true)
						{
							int num = gZipStream.Read(array, 0, array.Length);
							if (num <= 0)
							{
								break;
							}
							memoryStream2.Write(array, 0, num);
						}
						gZipStream.Close();
						result = memoryStream2.ToArray();
					}
				}
			}
			return result;
		}
	}
}
