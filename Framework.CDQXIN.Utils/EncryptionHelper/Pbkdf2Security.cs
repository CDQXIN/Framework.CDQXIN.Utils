using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.EncryptionHelper
{
	/// <summary>
	/// 使用加密服务提供程序 (CSP) 提供的实现来实现加密随机数生成器 (RNG)。
	/// </summary>
	public class Pbkdf2Security
	{
		private const int SaltByteSize = 12;
		private const int HashByteSize = 12;
		private const int Pbkdf2Iterations = 1000;
		/// <summary>
		/// Creates a salted PBKDF2 hash of the password.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <returns>The hash of the password.</returns>
		public static string CreateHash(string password)
		{
			string result;
			using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
			{
				byte[] array = new byte[12];
				rNGCryptoServiceProvider.GetBytes(array);
				byte[] inArray = Pbkdf2Security.Pbkdf2(password, array, 1000, 12);
				result = Convert.ToBase64String(array) + Convert.ToBase64String(inArray);
			}
			return result;
		}
		/// <summary>
		/// Validates a password given a hash of the correct one.
		/// </summary>
		/// <param name="password">The password to check.</param>
		/// <param name="correctHash">A hash of the correct password.</param>
		/// <returns>True if the password is correct. False otherwise.</returns>
		public static bool ValidatePassword(string password, string correctHash)
		{
			bool result;
			try
			{
				int iterations = 1000;
				int num = correctHash.Length / 2;
				byte[] salt = Convert.FromBase64String(correctHash.Substring(0, num));
				byte[] array = Convert.FromBase64String(correctHash.Substring(num, num));
				byte[] b = Pbkdf2Security.Pbkdf2(password, salt, iterations, array.Length);
				result = Pbkdf2Security.SlowEquals(array, b);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		/// <summary>
		/// Compares two byte arrays in length-constant time. This comparison
		/// method is used so that password hashes cannot be extracted from
		/// on-line systems using a timing attack and then attacked off-line.
		/// </summary>
		/// <param name="a">The first byte array.</param>
		/// <param name="b">The second byte array.</param>
		/// <returns>True if both byte arrays are equal. False otherwise.</returns>
		private static bool SlowEquals(byte[] a, byte[] b)
		{
			uint num = (uint)(a.Length ^ b.Length);
			int num2 = 0;
			while (num2 < a.Length && num2 < b.Length)
			{
				num |= (uint)(a[num2] ^ b[num2]);
				num2++;
			}
			return num == 0u;
		}
		/// <summary>
		/// Computes the PBKDF2-SHA1 hash of a password.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="iterations">The PBKDF2 iteration count.</param>
		/// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
		/// <returns>A hash of the password.</returns>
		private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
		{
			byte[] bytes;
			using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt))
			{
				rfc2898DeriveBytes.IterationCount = iterations;
				bytes = rfc2898DeriveBytes.GetBytes(outputBytes);
			}
			return bytes;
		}
	}
}
