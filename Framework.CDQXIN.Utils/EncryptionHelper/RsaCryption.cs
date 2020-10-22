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
	/// RSA加密解密及RSA签名和验证
	/// </summary> 
	public class RsaCryption
	{
		/// <summary>
		/// RSA 的密钥产生 产生私钥 和公钥 
		/// </summary>
		/// <param name="privateKey">私钥</param>
		/// <param name="publicKey">公钥</param>
		public static void RsaKey(out string privateKey, out string publicKey)
		{
			try
			{
				using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
				{
					privateKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(rSACryptoServiceProvider.ToXmlString(true)));
					publicKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(rSACryptoServiceProvider.ToXmlString(false)));
				}
			}
			catch (Exception arg_44_0)
			{
				throw arg_44_0;
			}
		}
		/// <summary>
		/// 根据私钥获取js加密需要的指数和系数
		/// </summary>
		/// <param name="privatekey">私钥</param>
		/// <param name="exponent">指数</param>
		/// <param name="modulus">系数</param>
		/// <returns></returns>
		public static void JsPublicKey(string privatekey, out string exponent, out string modulus)
		{
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(privatekey)));
				RSAParameters rSAParameters = rSACryptoServiceProvider.ExportParameters(false);
				StringBuilder stringBuilder = new StringBuilder();
				byte[] array = rSAParameters.Exponent;
				for (int i = 0; i < array.Length; i++)
				{
					byte b = array[i];
					stringBuilder.AppendFormat("{0:x2}", b);
				}
				exponent = stringBuilder.ToString();
				stringBuilder.Clear();
				array = rSAParameters.Modulus;
				for (int i = 0; i < array.Length; i++)
				{
					byte b2 = array[i];
					stringBuilder.AppendFormat("{0:x2}", b2);
				}
				modulus = stringBuilder.ToString();
			}
		}
		/// <summary>
		/// RSA的加密函数  string
		/// </summary>
		/// <param name="publicKey">公钥</param>
		/// <param name="plaintext">明文</param>
		/// <returns></returns>
		public static string Encrypt(string publicKey, string plaintext)
		{
			return RsaCryption.Encrypt(publicKey, Encoding.UTF8.GetBytes(plaintext));
		}
		/// <summary>
		/// RSA的加密函数  string
		/// </summary>
		/// <param name="publicKey">公钥</param>
		/// <param name="plainbytes">明文字节数组</param>
		/// <returns></returns>
		public static string Encrypt(string publicKey, byte[] plainbytes)
		{
			string result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(publicKey)));
				int num = rSACryptoServiceProvider.KeySize / 8 - 11;
				byte[] array = new byte[num];
				using (MemoryStream memoryStream = new MemoryStream(plainbytes))
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						int num2;
						while ((num2 = memoryStream.Read(array, 0, num)) > 0)
						{
							byte[] array2 = new byte[num2];
							Array.Copy(array, 0, array2, 0, num2);
							byte[] array3 = rSACryptoServiceProvider.Encrypt(array2, false);
							memoryStream2.Write(array3, 0, array3.Length);
						}
						byte[] arg_89_0 = memoryStream2.ToArray();
						rSACryptoServiceProvider.Clear();
						result = Convert.ToBase64String(arg_89_0);
					}
				}
			}
			return result;
		}
		/// <summary>
		/// RSA的解密函数  stirng
		/// </summary>
		/// <param name="privateKey">私钥</param>
		/// <param name="ciphertext">密文字符串</param>
		/// <returns></returns>
		public static string Decrypt(string privateKey, string ciphertext)
		{
			return RsaCryption.Decrypt(privateKey, Convert.FromBase64String(ciphertext));
		}
		/// <summary>
		/// RSA的解密函数  byte
		/// </summary>
		/// <param name="privateKey">私钥</param>
		/// <param name="cipherbytes">密文字节数组</param>
		/// <returns></returns>
		public static string Decrypt(string privateKey, byte[] cipherbytes)
		{
			string @string;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(privateKey)));
				int num = rSACryptoServiceProvider.KeySize / 8;
				byte[] array = new byte[num];
				using (MemoryStream memoryStream = new MemoryStream(cipherbytes))
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						int num2;
						while ((num2 = memoryStream.Read(array, 0, num)) > 0)
						{
							byte[] array2 = new byte[num2];
							Array.Copy(array, 0, array2, 0, num2);
							byte[] array3 = rSACryptoServiceProvider.Decrypt(array2, false);
							memoryStream2.Write(array3, 0, array3.Length);
						}
						byte[] bytes = memoryStream2.ToArray();
						rSACryptoServiceProvider.Clear();
						@string = Encoding.UTF8.GetString(bytes);
					}
				}
			}
			return @string;
		}
		/// <summary>
		/// 获取Hash描述表(md5)
		/// </summary>
		/// <param name="strSource">待签名的字符串</param>
		/// <param name="hashData">Hash描述</param>
		/// <returns></returns>
		public static bool GetHash(string strSource, ref byte[] hashData)
		{
			bool result;
			try
			{
				HashAlgorithm hashAlgorithm = HashAlgorithm.Create("MD5");
				byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(strSource);
				hashData = hashAlgorithm.ComputeHash(bytes);
				result = true;
			}
			catch (Exception arg_29_0)
			{
				throw arg_29_0;
			}
			return result;
		}
		/// <summary>
		/// 获取Hash描述表(md5)
		/// </summary>
		/// <param name="strSource">待签名的字符串</param>
		/// <param name="strHashData">Hash描述</param>
		/// <returns></returns>
		public static bool GetHash(string strSource, ref string strHashData)
		{
			bool result;
			try
			{
				using (MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider())
				{
					byte[] inArray = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(strSource));
					strHashData = Convert.ToBase64String(inArray);
					result = true;
				}
			}
			catch (Exception arg_2E_0)
			{
				throw arg_2E_0;
			}
			return result;
		}
		/// <summary>
		/// 获取Hash描述表(md5)
		/// </summary>
		/// <param name="objFile">待签名的文件</param>
		/// <param name="hashData">Hash描述</param>
		/// <returns></returns>
		public static bool GetHash(FileStream objFile, ref byte[] hashData)
		{
			bool result;
			try
			{
				using (MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider())
				{
					hashData = mD5CryptoServiceProvider.ComputeHash(objFile);
					objFile.Close();
					result = true;
				}
			}
			catch (Exception arg_23_0)
			{
				throw arg_23_0;
			}
			return result;
		}
		/// <summary>
		/// 获取Hash描述表(md5)
		/// </summary>
		/// <param name="objFile">待签名的文件</param>
		/// <param name="strHashData">Hash描述</param>
		/// <returns></returns>
		public static bool GetHash(FileStream objFile, ref string strHashData)
		{
			bool result;
			try
			{
				using (MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider())
				{
					byte[] inArray = mD5CryptoServiceProvider.ComputeHash(objFile);
					objFile.Close();
					strHashData = Convert.ToBase64String(inArray);
					result = true;
				}
			}
			catch (Exception arg_2A_0)
			{
				throw arg_2A_0;
			}
			return result;
		}
		/// <summary>
		/// RSA签名
		/// </summary>
		/// <param name="pStrKeyPrivate">秘钥</param>
		/// <param name="hashbyteSignature">待签名Hash描述(md5)</param>
		/// <param name="encryptedSignatureData">密文签名数据</param>
		/// <returns></returns>
		public static bool SignatureFormatter(string pStrKeyPrivate, byte[] hashbyteSignature, ref byte[] encryptedSignatureData)
		{
			bool result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(pStrKeyPrivate);
				RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
				rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
				encryptedSignatureData = rSAPKCS1SignatureFormatter.CreateSignature(hashbyteSignature);
				result = true;
			}
			return result;
		}
		/// <summary>
		/// RSA签名
		/// </summary>
		/// <param name="pStrKeyPrivate">私钥</param>
		/// <param name="hashbyteSignature">待签名Hash描述(md5)</param>
		/// <param name="mStrEncryptedSignatureData">密文签名数据</param>
		/// <returns></returns>
		public static bool SignatureFormatter(string pStrKeyPrivate, byte[] hashbyteSignature, ref string mStrEncryptedSignatureData)
		{
			bool result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(pStrKeyPrivate);
				RSAPKCS1SignatureFormatter expr_13 = new RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
				expr_13.SetHashAlgorithm("MD5");
				byte[] inArray = expr_13.CreateSignature(hashbyteSignature);
				mStrEncryptedSignatureData = Convert.ToBase64String(inArray);
				rSACryptoServiceProvider.Clear();
				result = true;
			}
			return result;
		}
		/// <summary>
		/// RSA签名 
		/// </summary>
		/// <param name="pStrKeyPrivate">私钥</param>
		/// <param name="mStrHashbyteSignature">待签名Hash描述(md5)</param>
		/// <param name="encryptedSignatureData">签名后的结果</param>
		/// <returns></returns>
		public static bool SignatureFormatter(string pStrKeyPrivate, string mStrHashbyteSignature, ref byte[] encryptedSignatureData)
		{
			byte[] rgbHash = Convert.FromBase64String(mStrHashbyteSignature);
			bool result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(pStrKeyPrivate);
				RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
				rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
				encryptedSignatureData = rSAPKCS1SignatureFormatter.CreateSignature(rgbHash);
				rSACryptoServiceProvider.Clear();
				result = true;
			}
			return result;
		}
		/// <summary>
		/// RSA签名 
		/// </summary>
		/// <param name="pStrKeyPrivate">私钥</param>
		/// <param name="mStrHashbyteSignature">待签名Hash描述(md5)</param>
		/// <param name="mStrEncryptedSignatureData">签名后的结果</param>
		/// <returns></returns>
		public static bool SignatureFormatter(string pStrKeyPrivate, string mStrHashbyteSignature, ref string mStrEncryptedSignatureData)
		{
			byte[] rgbHash = Convert.FromBase64String(mStrHashbyteSignature);
			bool result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(pStrKeyPrivate);
				RSAPKCS1SignatureFormatter expr_1A = new RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
				expr_1A.SetHashAlgorithm("MD5");
				byte[] inArray = expr_1A.CreateSignature(rgbHash);
				mStrEncryptedSignatureData = Convert.ToBase64String(inArray);
				rSACryptoServiceProvider.Clear();
				result = true;
			}
			return result;
		}
		/// <summary>
		/// RSA签名验证
		/// </summary>
		/// <param name="pStrKeyPublic">公钥</param>
		/// <param name="hashbyteDeformatter">待签名Hash描述(md5)</param>
		/// <param name="deformatterData">签名后的结果</param>
		/// <returns></returns>
		public static bool SignatureDeformatter(string pStrKeyPublic, byte[] hashbyteDeformatter, byte[] deformatterData)
		{
			bool result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(pStrKeyPublic);
				RSAPKCS1SignatureDeformatter expr_13 = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
				expr_13.SetHashAlgorithm("MD5");
				rSACryptoServiceProvider.Clear();
				result = expr_13.VerifySignature(hashbyteDeformatter, deformatterData);
			}
			return result;
		}
		/// <summary>
		/// RSA签名验证
		/// </summary>
		/// <param name="pStrKeyPublic">公钥</param>
		/// <param name="pStrHashbyteDeformatter">待签名Hash描述(md5)</param>
		/// <param name="deformatterData">签名后的结果</param>
		/// <returns></returns>
		public static bool SignatureDeformatter(string pStrKeyPublic, string pStrHashbyteDeformatter, byte[] deformatterData)
		{
			byte[] rgbHash = Convert.FromBase64String(pStrHashbyteDeformatter);
			bool result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(pStrKeyPublic);
				RSAPKCS1SignatureDeformatter expr_1A = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
				expr_1A.SetHashAlgorithm("MD5");
				rSACryptoServiceProvider.Clear();
				result = expr_1A.VerifySignature(rgbHash, deformatterData);
			}
			return result;
		}
		/// <summary>
		/// RSA签名验证
		/// </summary>
		/// <param name="pStrKeyPublic">公钥</param>
		/// <param name="hashbyteDeformatter">待签名Hash描述(md5)</param>
		/// <param name="pStrDeformatterData">签名后的结果</param>
		/// <returns></returns>
		public static bool SignatureDeformatter(string pStrKeyPublic, byte[] hashbyteDeformatter, string pStrDeformatterData)
		{
			bool result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(pStrKeyPublic);
				RSAPKCS1SignatureDeformatter expr_13 = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
				expr_13.SetHashAlgorithm("MD5");
				byte[] rgbSignature = Convert.FromBase64String(pStrDeformatterData);
				rSACryptoServiceProvider.Clear();
				result = expr_13.VerifySignature(hashbyteDeformatter, rgbSignature);
			}
			return result;
		}
		/// <summary>
		/// RSA签名验证
		/// </summary>
		/// <param name="pStrKeyPublic">公钥</param>
		/// <param name="pStrHashbyteDeformatter">待签名Hash描述(md5)</param>
		/// <param name="pStrDeformatterData">签名后的结果</param>
		/// <returns></returns>
		public static bool SignatureDeformatter(string pStrKeyPublic, string pStrHashbyteDeformatter, string pStrDeformatterData)
		{
			byte[] rgbHash = Convert.FromBase64String(pStrHashbyteDeformatter);
			bool result;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(pStrKeyPublic);
				RSAPKCS1SignatureDeformatter expr_1A = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
				expr_1A.SetHashAlgorithm("MD5");
				byte[] rgbSignature = Convert.FromBase64String(pStrDeformatterData);
				rSACryptoServiceProvider.Clear();
				result = expr_1A.VerifySignature(rgbHash, rgbSignature);
			}
			return result;
		}
	}
}
