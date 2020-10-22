using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// Session 操作类
	/// 1、GetSession(string name)根据session名获取session对象
	/// 2、SetSession(string name, object val)设置session
	/// </summary>
	public class SessionHelper
	{
		/// <summary>
		/// 根据session名获取session对象
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="isdel">是否删除</param>
		/// <returns></returns>
		public static object Get(string key, bool isdel = false)
		{
			object arg_19_0 = HttpContext.Current.Session[key];
			if (isdel)
			{
				SessionHelper.Remove(key);
			}
			return arg_19_0;
		}
		/// <summary>
		/// 根据session名获取session对象
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="isdel">是否删除</param>
		/// <returns></returns>
		public static string GetString(string key, bool isdel = false)
		{
			object obj = HttpContext.Current.Session[key];
			if (isdel)
			{
				SessionHelper.Remove(key);
			}
			if (obj == null)
			{
				return null;
			}
			return obj.ToString();
		}
		/// <summary>
		/// 获取Session
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">key</param>
		/// <param name="isdel">是否删除</param>
		/// <returns></returns>
		public static T Get<T>(string key, bool isdel = false)
		{
			object obj = HttpContext.Current.Session[key];
			if (isdel)
			{
				SessionHelper.Remove(key);
			}
			if (obj == null)
			{
				return default(T);
			}
			return JsonConvert.DeserializeObject<T>(obj.ToString());
		}
		/// <summary>
		/// 设置session
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="val">value</param>
		public static void Set(string key, object val)
		{
			HttpContext.Current.Session.Remove(key);
			HttpContext.Current.Session.Add(key, val);
		}
		/// <summary>
		/// 设置session
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="val">value</param>
		public static void Set(string key, string val)
		{
			HttpContext.Current.Session.Remove(key);
			HttpContext.Current.Session.Add(key, val);
		}
		/// <summary>
		/// 设置session
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">key</param>
		/// <param name="val">value</param>
		public static void Set<T>(string key, T val)
		{
			HttpContext.Current.Session.Remove(key);
			HttpContext.Current.Session.Add(key, JsonConvert.SerializeObject(val));
		}
		/// <summary>
		/// 清空所有的Session
		/// </summary>
		/// <returns></returns>
		public static void Clear()
		{
			HttpContext.Current.Session.Clear();
		}
		/// <summary>
		/// 删除一个指定的ession
		/// </summary>
		/// <param name="key">key</param>
		/// <returns></returns>
		public static void Remove(string key)
		{
			HttpContext.Current.Session.Remove(key);
		}
	}
}
