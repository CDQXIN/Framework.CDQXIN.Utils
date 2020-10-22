using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    /// <summary>
    /// web.config AppSettings操作帮助类
    /// </summary>
    public class ConfigHelper_v1
    {
		/// <summary>
		/// 得到AppSettings中的所配置字符串信息 (RuntimeCache 缓存1分钟)
		/// </summary>
		/// <param name="key">AppSettings的Key</param>
		/// <returns>AppSettings中的Value</returns>
		public static string GetString(string key)
		{
			string key2 = "AppSettings-" + key;
			object obj = RuntimeCacheHelper.Get(key2);
			if (obj == null)
			{
				obj = ConfigurationManager.AppSettings[key];
				if (obj != null)
				{
					RuntimeCacheHelper.Set(key2, obj, DateTime.Now.AddMinutes(1.0), TimeSpan.Zero);
				}
			}
			if (obj == null)
			{
				return null;
			}
			return obj.ToString();
		}
		/// <summary>
		/// 得到AppSettings中的配置Bool信息 (RuntimeCache 缓存1分钟)
		/// </summary>
		/// <param name="key">AppSettings的Key</param>
		/// <returns></returns>
		public static bool GetBool(string key)
		{
			bool result = false;
			string @string = ConfigHelper_v1.GetString(key);
			if (string.IsNullOrEmpty(@string))
			{
				return false;
			}
			try
			{
				result = bool.Parse(@string);
			}
			catch (FormatException)
			{
				throw;
			}
			return result;
		}
		/// <summary>
		/// 得到AppSettings中的配置Decimal信息 (RuntimeCache 缓存1分钟)
		/// </summary>
		/// <param name="key">AppSettings的Key</param>
		/// <returns></returns>
		public static decimal GetDecimal(string key)
		{
			decimal result = 0m;
			string @string = ConfigHelper_v1.GetString(key);
			if (string.IsNullOrEmpty(@string))
			{
				return result;
			}
			try
			{
				result = decimal.Parse(@string);
			}
			catch (FormatException)
			{
				throw;
			}
			return result;
		}
		/// <summary>
		/// 得到AppSettings中的配置int信息 (RuntimeCache 缓存1分钟)
		/// </summary>
		/// <param name="key">AppSettings的Key</param>
		/// <returns></returns>
		public static int GetConfigInt(string key)
		{
			int result = 0;
			string @string = ConfigHelper_v1.GetString(key);
			if (string.IsNullOrEmpty(@string))
			{
				return result;
			}
			try
			{
				result = int.Parse(@string);
			}
			catch (FormatException)
			{
				throw;
			}
			return result;
		}
	}
}
