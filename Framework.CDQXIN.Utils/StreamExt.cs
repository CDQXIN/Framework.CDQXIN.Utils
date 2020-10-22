using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	///
	/// </summary>
	public static class StreamExt
	{
		/// <summary>
		/// 获取流的MD5值
		/// </summary>
		/// <param name="stream">Stream</param>
		/// <param name="code">默认32；code:16 or 32</param>
		/// <returns>MD5值</returns>
		public static string GetMd5(this Stream stream, int code = 32)
		{
			string text = BitConverter.ToString(stream.GetMd5ToBytes()).Replace("-", string.Empty);
			if (code != 16)
			{
				return text;
			}
			return text.Substring(8, 16);
		}
		/// <summary>
		/// 获取md5 ToBase64
		/// </summary>
		/// <param name="stream">Stream</param>
		/// <returns></returns>
		public static string GetMd5ToBase64(this Stream stream)
		{
			return Convert.ToBase64String(stream.GetMd5ToBytes());
		}
		/// <summary>
		/// 获取Md5 To Bytes
		/// </summary>
		/// <param name="stream">Stream</param>
		/// <returns></returns>
		public static byte[] GetMd5ToBytes(this Stream stream)
		{
			byte[] result;
			using (MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider())
			{
				result = mD5CryptoServiceProvider.ComputeHash(stream);
			}
			return result;
		}
		/// <summary>
		/// 将数据流转化为字节数组
		/// </summary>
		/// <param name="stream">数据流</param>
		/// <returns></returns>
		public static byte[] ToBytes(this Stream stream)
		{
			byte[] result;
			try
			{
				MemoryStream memoryStream = stream as MemoryStream;
				if (memoryStream == null)
				{
					memoryStream = new MemoryStream();
					stream.CopyTo(memoryStream);
				}
				result = memoryStream.ToArray();
			}
			finally
			{
				if (stream != null)
				{
					((IDisposable)stream).Dispose();
				}
			}
			return result;
		}
	}
}
