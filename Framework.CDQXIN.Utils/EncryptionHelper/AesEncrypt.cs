using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.EncryptionHelper
{
	/// <summary>
	///  AES加密/解密类。
	/// </summary>
	public class AesEncrypt
	{
		private static readonly string defaultkey = "donchen-c";
		/// <summary>
		/// 使用缺省秘钥字符串加密
		/// </summary>
		/// <param name="plaintext">明文</param>
		/// <returns>密文</returns>
		public static string Encrypt(string plaintext)
		{
			return AesEncrypt.Encrypt(plaintext, AesEncrypt.defaultkey);
		}
		/// <summary>
		/// 加密数据
		/// </summary>
		/// <param name="plaintext">明文</param>
		/// <param name="key">秘钥</param>
		/// <returns></returns>
		public static string Encrypt(string plaintext, string key)
		{
			string result;
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				string s = AesEncrypt.Repair(key, 32);
				string s2 = AesEncrypt.Repair(key, 16);
				aesCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				aesCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s2);
				ICryptoTransform transform = aesCryptoServiceProvider.CreateEncryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(plaintext);
						}
						StringBuilder stringBuilder = new StringBuilder();
						byte[] array = memoryStream.ToArray();
						for (int i = 0; i < array.Length; i++)
						{
							byte b = array[i];
							stringBuilder.AppendFormat("{0:X2}", b);
						}
						result = stringBuilder.ToString();
					}
				}
			}
			return result;
		}
		/// <summary>
		/// 加密数据
		/// </summary>
		/// <param name="plaintext">明文</param>
		/// <param name="key">秘钥</param>
		/// <param name="vector">向量</param>
		/// <returns></returns>
		public static string Encrypt(string plaintext, string key, string vector)
		{
			string result;
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				string s = AesEncrypt.Repair(key, 32);
				string s2 = AesEncrypt.Repair(vector, 16);
				aesCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				aesCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s2);
				ICryptoTransform transform = aesCryptoServiceProvider.CreateEncryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(plaintext);
						}
						StringBuilder stringBuilder = new StringBuilder();
						byte[] array = memoryStream.ToArray();
						for (int i = 0; i < array.Length; i++)
						{
							byte b = array[i];
							stringBuilder.AppendFormat("{0:X2}", b);
						}
						result = stringBuilder.ToString();
					}
				}
			}
			return result;
		}
		/// <summary>
		/// 解密数据
		/// </summary>
		/// <param name="ciphertext">密文</param>
		/// <returns>明文</returns>
		public static string Decrypt(string ciphertext)
		{
			return AesEncrypt.Decrypt(ciphertext, AesEncrypt.defaultkey);
		}
		/// <summary>
		/// 解密数据
		/// </summary>
		/// <param name="ciphertext">密文</param>
		/// <param name="key">秘钥</param>
		/// <returns>明文</returns>
		public static string Decrypt(string ciphertext, string key)
		{
			int num = ciphertext.Length / 2;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = Convert.ToInt32(ciphertext.Substring(i * 2, 2), 16);
				array[i] = (byte)num2;
			}
			string result;
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				string s = AesEncrypt.Repair(key, 32);
				string s2 = AesEncrypt.Repair(key, 16);
				aesCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				aesCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s2);
				using (ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateDecryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV))
				{
					using (MemoryStream memoryStream = new MemoryStream(array))
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
						{
							using (StreamReader streamReader = new StreamReader(cryptoStream))
							{
								result = streamReader.ReadToEnd();
							}
						}
					}
				}
			}
			return result;
		}
		/// <summary>
		/// 解密数据
		/// </summary>
		/// <param name="ciphertext">密文</param>
		/// <param name="key">秘钥</param>
		/// <param name="vector">向量</param>
		/// <returns>明文</returns>
		public static string Decrypt(string ciphertext, string key, string vector)
		{
			int num = ciphertext.Length / 2;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = Convert.ToInt32(ciphertext.Substring(i * 2, 2), 16);
				array[i] = (byte)num2;
			}
			string result;
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				string s = AesEncrypt.Repair(key, 32);
				string s2 = AesEncrypt.Repair(vector, 16);
				aesCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				aesCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s2);
				using (ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateDecryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV))
				{
					using (MemoryStream memoryStream = new MemoryStream(array))
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
						{
							using (StreamReader streamReader = new StreamReader(cryptoStream))
							{
								result = streamReader.ReadToEnd();
							}
						}
					}
				}
			}
			return result;
		}
		/// <summary>
		/// 填充空位（将不够长度的字符串末尾填充00）
		/// </summary>
		/// <param name="key">秘钥</param>
		/// <param name="length">默认长度8</param>
		/// <returns></returns>
		private static string Repair(string key, int length = 8)
		{
			if (key.Length > length)
			{
				return key.Substring(0, length);
			}
			string text = "";
			for (int i = 0; i < length - key.Length; i++)
			{
				text += "0";
			}
			return key + text;
		}
	}
}
