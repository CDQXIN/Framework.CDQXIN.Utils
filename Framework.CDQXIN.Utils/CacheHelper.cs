using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Framework.CDQXIN.Utils
{
    public static class CacheHelper
    {


        static Cache m_Cache = HttpContext.Current.Cache;
        public static void CacheValue(string key, Object value, int minutes)
        {
            m_Cache.Insert(key, value, null
                , DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }


        /// <summary>
        /// 滑动过期时间  20180504
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        private static void CacheSlideValue(string key, Object value, int minutes)
        {
            m_Cache.Insert(key, value, null
                , System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes));
        }


        //获取配置信息存储键
        private static string GetCacheKey(string prefix, string name)
        {
            return prefix + name;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheModel">prefix</param>
        /// <param name="name">key</param>
        /// <returns></returns>
        public static object GetCache(string cacheModel, string name)
        {
            string key = CacheHelper.GetCacheKey(cacheModel, name);
            return m_Cache.Get(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        ///<param name="cacheModel">prefix</param>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        public static void AddCache(string cacheModel, string name, object value)
        {
            string key = CacheHelper.GetCacheKey(cacheModel, name);
            CacheHelper.CacheValue(key, value, 10);


        }

        /// <summary>
        /// 添加缓存（有效期依赖文件）
        /// </summary>
        /// <param name="cacheModel"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="filepath"></param>
        public static void AddCache(string cacheModel, string name, object value, string filepath)
        {
            string key = CacheHelper.GetCacheKey(cacheModel, name);

            m_Cache.Insert(key, value, new CacheDependency(filepath));
        }

        /// <summary>
        /// 添加滑动过期时间
        /// </summary>
        /// <param name="cacheModel"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddSlideCache(string cacheModel, string name, object value)
        {
            string key = CacheHelper.GetCacheKey(cacheModel, name);
            CacheHelper.CacheSlideValue(key, value, 20);
        }

        public static void AddCache(string cacheModel, string name, object value, int minutes)
        {
            string key = CacheHelper.GetCacheKey(cacheModel, name);
            CacheHelper.CacheValue(key, value, minutes);
        }

        /// <summary>
        /// 移除缓存。
        /// </summary>
        /// <param name="cacheModel">prefix</param>
        /// <param name="name">key</param>
        public static void RemoveCache(string cacheModel, string name)
        {
            string key = GetCacheKey(cacheModel, name);
            object value = m_Cache.Get(key);
            if (value != null)
                m_Cache.Remove(key);
        }

    }
}
