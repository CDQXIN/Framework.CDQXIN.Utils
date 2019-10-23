using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.ExtensionHelper
{
    /// <summary>
    /// DataTable 扩展
    /// </summary>
    public static class DataTableExtHelper
    {
        #region DataTable转换成list
        /// <summary>
        /// DataTable转换成list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">查询列表（不能是Excel文件的table）</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            //确认参数有效
            List<T> list = new List<T>();

            if (dt == null || dt.Rows.Count == 0) return list;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                T t = Activator.CreateInstance<T>();
                //获取对象所有属性
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            info.SetValue(t, ChangeType(dt.Rows[i][j], info.PropertyType), null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }

        private static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == DBNull.Value)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
        #endregion

    }
}
