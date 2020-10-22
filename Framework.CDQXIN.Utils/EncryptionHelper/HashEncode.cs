using Framework.CDQXIN.Utils.ExtensionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.EncryptionHelper
{
	/// <summary>
	/// 得到随机安全码（哈希加密）。
	/// </summary>
	public class HashEncode
	{
		/// <summary>
		/// 得到随机哈希加密字符串
		/// </summary>
		/// <returns></returns>
		public static string GetSecurity()
		{
			return HashEncode.HashEncoding(Guid.NewGuid().ToString());
		}
		/// <summary>
		/// 得到一个随机数值
		/// </summary>
		/// <returns></returns>
		public static string GetRandomValue()
		{
			return new Random().Next(1, 2147483647).ToString();
		}
		/// <summary>
		/// 哈希加密一个字符串
		/// </summary>
		/// <param name="security">安全码</param>
		/// <returns>将二进制流转化为0x...字符串</returns>
		public static string HashEncoding(string security)
		{
			byte[] bytes = new UnicodeEncoding().GetBytes(security);
			SHA512Managed expr_11 = new SHA512Managed();
			byte[] bytes2 = expr_11.ComputeHash(bytes);
			expr_11.Clear();
			expr_11.Dispose();
			return bytes2.ToHexIs0X();
		}
	}
}
