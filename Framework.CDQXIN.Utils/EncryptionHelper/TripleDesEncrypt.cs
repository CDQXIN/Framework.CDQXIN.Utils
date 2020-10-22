using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.EncryptionHelper
{
	/// <summary>
	/// TripleDes加密/ji
	/// </summary>
	public class TripleDesEncrypt
	{
		private static readonly string key = "donchen-c";
		/// <summary>
		/// 使用缺省密钥字符串加密string
		/// </summary>
		/// <param name="original">明文</param>
		/// <returns>密文</returns>
		public static string Encrypt(string original)
		{
			return TripleDesEncrypt.Encrypt(original, TripleDesEncrypt.key);
		}
		/// <summary>
		/// 使用缺省密钥字符串解密string
		/// </summary>
		/// <param name="original">密文</param>
		/// <returns>明文</returns>
		public static string Decrypt(string original)
		{
			return TripleDesEncrypt.Decrypt(original, TripleDesEncrypt.key, Encoding.Default);
		}
		/// <summary>
		/// 使用给定密钥字符串加密string
		/// </summary>
		/// <param name="original">原始文字</param>
		/// <param name="key">密钥</param>
		/// <returns>密文</returns>
		public static string Encrypt(string original, string key)
		{
			byte[] arg_18_0 = Encoding.Default.GetBytes(original);
			byte[] bytes = Encoding.Default.GetBytes(key);
			return Convert.ToBase64String(TripleDesEncrypt.Encrypt(arg_18_0, bytes));
		}
		/// <summary>
		/// 使用给定密钥字符串解密string
		/// </summary>
		/// <param name="original">密文</param>
		/// <param name="key">密钥</param>
		/// <returns>明文</returns>
		public static string Decrypt(string original, string key)
		{
			return TripleDesEncrypt.Decrypt(original, key, Encoding.Default);
		}
		/// <summary>
		/// 使用给定密钥字符串解密string,返回指定编码方式明文
		/// </summary>
		/// <param name="encrypted">密文</param>
		/// <param name="key">密钥</param>
		/// <param name="encoding">字符编码方案</param>
		/// <returns>明文</returns>
		public static string Decrypt(string encrypted, string key, Encoding encoding)
		{
			byte[] encrypted2 = Convert.FromBase64String(encrypted);
			byte[] bytes = Encoding.Default.GetBytes(key);
			return encoding.GetString(TripleDesEncrypt.Decrypt(encrypted2, bytes));
		}
		/// <summary>
		/// 使用缺省密钥字符串解密byte[]
		/// </summary>
		/// <param name="encrypted">密文</param>
		/// <returns>明文</returns>
		public static byte[] Decrypt(byte[] encrypted)
		{
			byte[] bytes = Encoding.Default.GetBytes(TripleDesEncrypt.key);
			return TripleDesEncrypt.Decrypt(encrypted, bytes);
		}
		/// <summary>
		/// 使用缺省密钥字符串加密
		/// </summary>
		/// <param name="original">原始数据</param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] original)
		{
			byte[] bytes = Encoding.Default.GetBytes(TripleDesEncrypt.key);
			return TripleDesEncrypt.Encrypt(original, bytes);
		}
		/// <summary>
		/// 生成MD5
		/// </summary>
		/// <param name="original">数据源</param>
		/// <returns></returns>
		public static byte[] MakeMd5(byte[] original)
		{
			byte[] result;
			using (MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider())
			{
				result = mD5CryptoServiceProvider.ComputeHash(original);
			}
			return result;
		}
		/// <summary>
		/// 使用给定密钥加密
		/// </summary>
		/// <param name="original">明文</param>
		/// <param name="key">密钥</param>
		/// <returns>密文</returns>
		public static byte[] Encrypt(byte[] original, byte[] key)
		{
			byte[] result;
			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
			{
				tripleDESCryptoServiceProvider.Key = TripleDesEncrypt.MakeMd5(key);
				tripleDESCryptoServiceProvider.Mode = CipherMode.CBC;
				result = tripleDESCryptoServiceProvider.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
			}
			return result;
		}
		/// <summary>
		/// 使用给定密钥解密数据
		/// </summary>
		/// <param name="encrypted">密文</param>
		/// <param name="key">密钥</param>
		/// <returns>明文</returns>
		public static byte[] Decrypt(byte[] encrypted, byte[] key)
		{
			byte[] result;
			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
			{
				tripleDESCryptoServiceProvider.Key = TripleDesEncrypt.MakeMd5(key);
				tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
				result = tripleDESCryptoServiceProvider.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
			}
			return result;
		}
	}
}
