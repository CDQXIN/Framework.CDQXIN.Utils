using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 本地缓存帮助类（HttpRuntime.Cache）
	/// </summary>
	public class RuntimeCacheHelper
	{
		/// <summary>
		/// 设置数据缓存
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <remarks>
		/// 默认值(根据系统全局设置)
		/// </remarks>
		/// </summary>
		public static void Set(string key, object obj)
		{
			HttpRuntime.Cache.Insert(key, obj);
		}
		/// <summary>
		/// 设置数据缓存
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <param name="timeout">过期时间(绝对过期)</param>
		/// <remarks>
		/// 绝对过期
		/// </remarks>
		/// </summary>
		public static void Set(string key, object obj, TimeSpan timeout)
		{
			HttpRuntime.Cache.Insert(key, obj, null, DateTime.MaxValue, timeout, CacheItemPriority.NotRemovable, null);
		}
		/// <summary>
		/// 设置缓存
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <param name="expires">过期时间(分钟)</param>
		/// <remarks>
		/// 滑动过期
		/// </remarks>
		public static void Set(string key, object obj, int expires)
		{
			HttpRuntime.Cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, expires, 0));
		}
		/// <summary>
		/// 设置数据缓存
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <param name="absoluteExpiration">绝对过期时间</param>
		/// <param name="slidingExpiration">滑动过期时间</param>
		/// </summary>
		public static void Set(string key, object obj, DateTime absoluteExpiration, TimeSpan slidingExpiration)
		{
			HttpRuntime.Cache.Insert(key, obj, null, absoluteExpiration, slidingExpiration);
		}
		/// <summary>
		/// 创建缓存项的文件依赖
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <param name="filename">依赖项文件绝对路径</param>
		public static void Set(string key, object obj, string filename)
		{
			HttpRuntime.Cache.Insert(key, obj, new CacheDependency(filename));
		}
		/// <summary>
		/// 获取数据缓存
		/// </summary>
		/// <param name="key">键</param>
		/// <returns>缓存数据</returns>
		public static object Get(string key)
		{
			return HttpRuntime.Cache[key];
		}
		/// <summary>
		/// 获取数据缓存
		/// </summary>
		/// <typeparam name="T">缓存数据类型</typeparam>
		/// <param name="key">键</param>
		/// <returns>缓存数据</returns>
		public static T Get<T>(string key)
		{
			object obj = RuntimeCacheHelper.Get(key);
			if (obj != null)
			{
				return (T)obj;
			}
			return default(T);
		}
		/// <summary>
		/// 移除指定数据缓存
		/// <param name="key">键</param>
		/// </summary>
		public static void Remove(string key)
		{
			HttpRuntime.Cache.Remove(key);
		}
		/// <summary>
		/// 移除全部缓存
		/// </summary>
		public static void RemoveAll()
		{
			Cache cache = HttpRuntime.Cache;
			IDictionaryEnumerator enumerator = cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				cache.Remove(enumerator.Key.ToString());
			}
		}
	}
}
