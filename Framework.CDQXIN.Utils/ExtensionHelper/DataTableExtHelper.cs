using System;
using System.Collections;
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

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTableExtend<T>(IList<T> list, List<string> removeColNames)
        {
            return ToDataTableExtend<T>(list, removeColNames, null);
        }
        /// <summary>    
        /// 将泛型集合类转换成DataTable
        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>    
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTableExtend<T>(IList<T> list, List<string> removeColNames, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
            {
                propertyNameList.AddRange(propertyName);
            }
            DataTable result = new DataTable();
            var flag = false;
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        //if (DBNull.Value.Equals(pi.PropertyType))
                        //{
                        //   // pi.PropertyType = DateTime;
                        //}
                        Type colType = pi.PropertyType;
                        if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        flag = removeColNames != null && removeColNames.Exists(p => p.ToLower() == pi.Name.ToLower());
                        if (!flag)
                        {
                            result.Columns.Add(pi.Name, colType);
                        }

                        //result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            flag = removeColNames != null && removeColNames.Exists(p => p.ToLower() == pi.Name.ToLower());
                            if (!flag)
                            {
                                result.Columns.Add(pi.Name, pi.PropertyType);
                            }
                        }
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (!removeColNames.Exists(p => p.ToLower() == pi.Name.ToLower()))
                        {
                            if (propertyNameList.Count == 0)
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                            else
                            {
                                if (propertyNameList.Contains(pi.Name))
                                {
                                    object obj = pi.GetValue(list[i], null);
                                    tempList.Add(obj);
                                }
                            }
                        }

                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

    }
}
