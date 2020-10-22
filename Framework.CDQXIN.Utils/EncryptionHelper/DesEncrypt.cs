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
	/// DES加密/解密类。
	/// </summary>
	public class DesEncrypt
	{
		private static readonly string defaultkey = "donchen-c";
		/// <summary>
		/// 使用缺省密钥字符串加密string
		/// </summary>
		/// <param name="plaintext">明文</param>
		/// <returns>密文</returns>
		public static string Encrypt(string plaintext)
		{
			return DesEncrypt.Encrypt(plaintext, DesEncrypt.defaultkey);
		}
		/// <summary> 
		/// 加密数据 
		/// </summary> 
		/// <param name="plaintext">明文</param> 
		/// <param name="keyStr">秘钥</param> 
		/// <returns>密文</returns> 
		public static string Encrypt(string plaintext, string keyStr)
		{
			string result;
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
				string s = DesEncrypt.Repair(keyStr);
				dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.FlushFinalBlock();
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
		/// <param name="keyStr">秘钥</param>
		/// <param name="vector">向量</param>
		/// <remarks>
		/// 密文经过Base64编码
		/// </remarks>
		/// <returns>密文</returns>
		public static string Encrypt(string plaintext, string keyStr, string vector)
		{
			string result;
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
				string s = DesEncrypt.Repair(keyStr);
				string s2 = DesEncrypt.Repair(vector);
				dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s2);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.FlushFinalBlock();
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
		/// 加密文件
		/// </summary>
		/// <param name="filePath">输入文件路径</param>
		/// <param name="savePath">加密后输出文件路径</param>
		/// <returns></returns>
		public static bool EncryptFile(string filePath, string savePath)
		{
			return DesEncrypt.EncryptFile(filePath, savePath, DesEncrypt.defaultkey);
		}
		/// <summary>
		/// 加密文件
		/// </summary>
		/// <param name="filePath">输入文件路径</param>
		/// <param name="savePath">加密后输出文件路径</param>
		/// <param name="keyStr">密码</param>
		/// <returns></returns>  
		public static bool EncryptFile(string filePath, string savePath, string keyStr)
		{
			bool result;
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				string s = DesEncrypt.Repair(keyStr);
				FileStream fileStream = File.OpenRead(filePath);
				byte[] array = new byte[(int)((object)((IntPtr)fileStream.Length))];
				fileStream.Read(array, 0, (int)fileStream.Length);
				fileStream.Close();
				dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(array, 0, array.Length);
						cryptoStream.FlushFinalBlock();
						fileStream = File.OpenWrite(savePath);
						byte[] array2 = memoryStream.ToArray();
						for (int i = 0; i < array2.Length; i++)
						{
							byte value = array2[i];
							fileStream.WriteByte(value);
						}
						fileStream.Close();
						cryptoStream.Close();
						memoryStream.Close();
						result = true;
					}
				}
			}
			return result;
		}
		/// <summary>
		/// 使用缺省密钥字符串解密string
		/// </summary>
		/// <param name="ciphertext">密文</param>
		/// <returns>密文</returns>
		public static string Decrypt(string ciphertext)
		{
			return DesEncrypt.Decrypt(ciphertext, DesEncrypt.defaultkey);
		}
		/// <summary> 
		/// 解密数据 
		/// </summary> 
		/// <param name="ciphertext">密文</param> 
		/// <param name="keyStr">秘钥</param> 
		/// <returns>明文</returns> 
		public static string Decrypt(string ciphertext, string keyStr)
		{
			string @string;
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				int num = ciphertext.Length / 2;
				byte[] array = new byte[num];
				for (int i = 0; i < num; i++)
				{
					int num2 = Convert.ToInt32(ciphertext.Substring(i * 2, 2), 16);
					array[i] = (byte)num2;
				}
				string s = DesEncrypt.Repair(keyStr);
				dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(array, 0, array.Length);
						cryptoStream.FlushFinalBlock();
						@string = Encoding.Default.GetString(memoryStream.ToArray());
					}
				}
			}
			return @string;
		}
		/// <summary>
		/// 解密数据
		/// </summary>
		/// <param name="ciphertext">密文</param>
		/// <param name="keyStr">秘钥</param>
		/// <param name="vector">向量</param>
		/// <returns>明文</returns>
		public static string Decrypt(string ciphertext, string keyStr, string vector)
		{
			string @string;
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				int num = ciphertext.Length / 2;
				byte[] array = new byte[num];
				for (int i = 0; i < num; i++)
				{
					int num2 = Convert.ToInt32(ciphertext.Substring(i * 2, 2), 16);
					array[i] = (byte)num2;
				}
				string s = DesEncrypt.Repair(keyStr);
				string s2 = DesEncrypt.Repair(vector);
				dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s2);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(array, 0, array.Length);
						cryptoStream.FlushFinalBlock();
						@string = Encoding.Default.GetString(memoryStream.ToArray());
					}
				}
			}
			return @string;
		}
		/// <summary>
		/// 解密文件
		/// </summary>
		/// <param name="filePath">输入文件路径</param>
		/// <param name="savePath">解密后输出文件路径</param>
		/// <returns></returns>
		public static bool DecryptFile(string filePath, string savePath)
		{
			return DesEncrypt.DecryptFile(filePath, savePath, DesEncrypt.defaultkey);
		}
		/// <summary>
		/// 解密文件
		/// </summary>
		/// <param name="filePath">输入文件路径</param>
		/// <param name="savePath">解密后输出文件路径</param>
		/// <param name="keyStr">密码</param>
		/// <returns></returns>    
		public static bool DecryptFile(string filePath, string savePath, string keyStr)
		{
			bool result;
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				string s = DesEncrypt.Repair(keyStr);
				FileStream fileStream = File.OpenRead(filePath);
				byte[] array = new byte[(int)((object)((IntPtr)fileStream.Length))];
				fileStream.Read(array, 0, (int)fileStream.Length);
				fileStream.Close();
				dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(s);
				dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(s);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(array, 0, array.Length);
						cryptoStream.FlushFinalBlock();
						fileStream = File.OpenWrite(savePath);
						byte[] array2 = memoryStream.ToArray();
						for (int i = 0; i < array2.Length; i++)
						{
							byte value = array2[i];
							fileStream.WriteByte(value);
						}
						fileStream.Close();
						cryptoStream.Close();
						memoryStream.Close();
						result = true;
					}
				}
			}
			return result;
		}
		/// <summary>
		/// 填充空位
		/// </summary>
		/// <param name="skey"></param>
		/// <returns></returns>
		private static string Repair(string skey)
		{
			if (skey.Length >= 8)
			{
				return skey.Substring(0, 8);
			}
			string text = "";
			for (int i = 0; i < 8 - skey.Length; i++)
			{
				text += "0";
			}
			return skey + text;
		}
	}
}
