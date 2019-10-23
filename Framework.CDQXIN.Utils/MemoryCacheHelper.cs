using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    /// <summary>
    /// 基于MemoryCache的缓存辅助类
    /// </summary>
    public static class MemoryCacheHelper
    {
        private static readonly object _locker = new object();

        public static bool Contains(string key)
        {
            return MemoryCache.Default.Contains(key);
        }


        /// <summary>
        /// 获取Catch元素
        /// </summary>
        /// <typeparam name="T">所获取的元素的类型</typeparam>
        /// <param name="key">元素的键</param>
        /// <returns>特定的元素值</returns>
        public static T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("不合法的key!");
            if (!MemoryCache.Default.Contains(key))
                throw new ArgumentException("获取失败,不存在该key!");
            if (!(MemoryCache.Default[key] is T))
                throw new ArgumentException("未找到所需类型数据!");
            return (T)MemoryCache.Default[key];
        }

        /// <summary>
        /// 添加Catch元素
        /// </summary>
        /// <param name="key">元素的键</param>
        /// <param name="value">元素的值</param>
        /// <param name="slidingExpiration">元素过期时间(时间间隔)</param>
        /// <param name="absoluteExpiration">元素过期时间(绝对时间)</param>
        /// <returns></returns>
        public static bool Add(string key, object value, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            var item = new CacheItem(key, value);
            var policy = CreatePolicy(slidingExpiration, absoluteExpiration);
            lock (_locker)
                return MemoryCache.Default.Add(item, policy);
        }

        /// <summary>
        /// 移出Cache元素
        /// </summary>
        /// <typeparam name="T">待移出元素的类型</typeparam>
        /// <param name="key">待移除元素的键</param>
        /// <returns>已经移出的元素</returns>
        public static T Remove<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("不合法的key!");
            if (!MemoryCache.Default.Contains(key))
                throw new ArgumentException("获取失败,不存在该key!");
            var value = MemoryCache.Default.Get(key);
            if (!(value is T))
                throw new ArgumentException("未找到所需类型数据!");
            return (T)MemoryCache.Default.Remove(key);
        }

        /// <summary>
        /// 移出多条缓存数据,默认为所有缓存
        /// </summary>
        /// <typeparam name="T">待移出的缓存类型</typeparam>
        /// <param name="keyList"></param>
        /// <returns></returns>
        public static List<T> RemoveAll<T>(IEnumerable<string> keyList = null)
        {
            if (keyList != null)
                return (from key in keyList
                        where MemoryCache.Default.Contains(key)
                        where MemoryCache.Default.Get(key) is T
                        select (T)MemoryCache.Default.Remove(key)).ToList();
            while (MemoryCache.Default.GetCount() > 0)
                MemoryCache.Default.Remove(MemoryCache.Default.ElementAt(0).Key);
            return new List<T>();
        }

        /// <summary>
        /// 设置过期信息
        /// </summary>
        /// <param name="slidingExpiration"></param>
        /// <param name="absoluteExpiration"></param>
        /// <returns></returns>
        private static CacheItemPolicy CreatePolicy(TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
            {
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            else if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = slidingExpiration.Value;
            }

            policy.Priority = CacheItemPriority.Default;

            return policy;
        }
    }
}
