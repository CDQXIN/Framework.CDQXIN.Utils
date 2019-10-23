using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.ExtensionHelper
{
    /// <summary>
    /// Object扩展
    /// </summary>
    public static class ObjectExtHelper
    {
        /// <summary>
        /// 对象为Null
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 对象不为Null
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// 验证对象是否有效
        /// </summary>
        /// <param name="obj">要验证的对象</param>
        /// <param name="validationResults"></param>
        /// <returns></returns>
        public static bool IsValid<T>(this T obj, Collection<ValidationResult> validationResults) where T : class, new()
        {
            return Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), validationResults, true);
        }

        /// <summary>
        /// 验证对象是否有效
        /// </summary>
        /// <param name="obj">要验证的对象</param>
        /// <returns></returns>
        public static bool IsValid<T>(this T obj) where T : class, new()
        {
            var validationResults = new Collection<ValidationResult>();

            var b = Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), validationResults, true);

            CallContext.SetData(obj.GetType().FullName, validationResults);

            return b;
        }

        /// <summary>
        /// 验证结果对象
        /// </summary>
        /// <param name="obj">要验证的对象</param>
        /// <returns></returns>
        public static Collection<ValidationResult> ValidationResult<T>(this T obj) where T : class, new()
        {
            var validationresults = CallContext.GetData(obj.GetType().FullName);

            return validationresults as Collection<ValidationResult>;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyname">属性名</param>
        /// <returns></returns>
        public static string GetPropertyValue(this object obj, string propertyname)
        {
            if (obj == null) return null;

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties.Length <= 0) return null;

            return (from item in properties where item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String") where item.Name == propertyname select item.GetValue(obj, null).ToString()).FirstOrDefault();
        }

        /// <summary>
        /// 得到包含对象所有属性的字符串列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetPropertyList(this object model)
        {
            return model.GetType().GetProperties().Select(t => t.Name).ToList();
        }

        /// <summary>
        /// 将Object对象转化为Dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (obj == null) return dic;

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties.Length <= 0) return dic;

            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(obj, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    dic.Add(name, value);
                }
                else
                {
                    ToDictionary(value);
                }
            }

            return dic;
        }

        /// <summary>
        /// 连接实体对象的所有属性值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="seperator">分隔符</param>
        /// <returns></returns>
        public static string JoinStringAll<T>(this T obj, string seperator = "")
        {
            PropertyInfo[] propertys = obj.GetType().GetProperties();

            string result = string.Join(seperator, (from pi in propertys select pi.GetValue(obj, null) into value where value != null select value.ToString()).ToArray());

            return result;
        }

        /// <summary>
        /// 复制一个model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <returns>T.</returns>
        public static T Copy<T>(this T model) where T : class, new()
        {
            var result = new T();
            foreach (var p in model.GetPropertyList())
            {
                result.GetType().GetProperty(p).SetValue(result, model.GetType().GetProperty(p).GetValue(model, null), null);
            }
            return result;
        }

        /// <summary>
        /// 将对象转化为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Ojbect 为Null 扩展方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string IsNullToString(this object obj)
        {
            if (obj == null) return null;

            return obj.ToString();
        }

        /// <summary>
        /// 检查对象是否在集合中
        /// </summary>
        /// <param name="item">对象</param>
        /// <param name="list">集合</param>
        /// <typeparam name="T">对象类型</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
    }
}
