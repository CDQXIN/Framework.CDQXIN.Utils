using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.ReflectionExtension
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMemberAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        object GetValue(object instance, string memberName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="memberName"></param>
        /// <param name="newValue"></param>
        void SetValue(object instance, string memberName, object newValue);
    }
}
