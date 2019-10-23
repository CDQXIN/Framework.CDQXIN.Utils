using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.ReflectionExtension
{
    /// <summary>
    /// 程序集反射扩展类
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <param name="ctorParameters"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(this Assembly assembly, string typeFullName, params object[] ctorParameters) where T : class
        {
            return (T)assembly.CreateInstance(typeFullName, true, BindingFlags.CreateInstance, null, ctorParameters, CultureInfo.CurrentCulture, null);
        }
    }
}
