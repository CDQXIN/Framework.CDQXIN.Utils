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
    /// List扩展
    /// </summary>
    public static class ListExtHelper
    {

        /// <summary>
        /// 去重复
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector">去重复关键字</param>
        /// <typeparam name="TSource">数据类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list) where T : class, new()
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            DataTable table = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    table.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }
            }

            return table;
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <param name="propertyName">字段名</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list, params string[] propertyName) where T : class, new()
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            List<string> propertyNameList = new List<string>();

            if (propertyName != null)
            {
                propertyNameList.AddRange(propertyName);
            }

            DataTable datatable = new DataTable();

            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        datatable.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            datatable.Columns.Add(pi.Name, pi.PropertyType);
                        }
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
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
                    object[] array = tempList.ToArray();
                    datatable.LoadDataRow(array, true);
                }
            }

            return datatable;
        }

        /// <summary>
        /// List分页拆分成多个List
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="list">待拆分集合</param>
        /// <param name="pageSize">拆分大小（页大小）</param>
        /// <returns>多个List</returns>
        public static List<List<T>> Paging<T>(this List<T> list, int pageSize)
        {
            var result = new List<List<T>>();
            var totalPage = Math.Ceiling(list.Count / ((decimal)pageSize));
            for (int i = 1; i <= totalPage; i++)
            {
                result.Add(list.Skip((i - 1) * pageSize).Take(pageSize).ToList());
            }
            return result;
        }

        /// <summary>
        /// 连接实体泛型里面指定属性的所有值
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="list">实体集合</param>
        /// <param name="propertyName">指定属性</param>
        /// <param name="seperator">分隔符号</param>
        /// <returns>System.String.</returns>
        public static string JoinString<T>(this List<T> list, string propertyName, string seperator)
        {
            List<string> vals = new List<string>();

            foreach (T t in list)
            {
                PropertyInfo[] propertys = t.GetType().GetProperties();

                T t1 = t;
                vals.AddRange(from pi in propertys where pi.Name == propertyName select pi.GetValue(t1, null) into value where value != DBNull.Value select value.ToString());
            }
            string result = string.Join(seperator, vals.ToArray());

            return result;
        }

        /// <summary>
        /// 将对象添加到 <see cref="T:System.Collections.Generic.List`1" /> 的首位
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="list">集合</param>
        /// <param name="t"></param>
        public static void AddToFirst<T>(this List<T> list, T t)
        {
            list.Reverse();
            list.Add(t);
            list.Reverse();
        }

        ///// <summary>
        ///// 将列表转换为树形结构
        ///// </summary>
        ///// <typeparam name="T">树形结构类型</typeparam>
        ///// <typeparam name="TF">列表类型</typeparam>
        ///// <param name="list">数据</param>
        ///// <param name="pidname"></param>
        ///// <param name="childrenname"></param>
        ///// <param name="idname"></param>
        ///// <returns></returns>
        //public static List<T> ToTree<T, TF>(this List<TF> list, string childrenname, string idname, string pidname, string id = "0")
        //{
        //    //TODO: 将列表数据转换为树形结构数据
        //    var treelist = new List<T>();

        //    if (list == null || list.Count == 0) return null;

        //    if (list.Any(e => e.GetPropertyValue(pidname) == id))
        //    {
        //        treelist.AddRange(list.Where(e => e.GetPropertyValue(pidname) == id).Select(e => e.As<T>()));
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    foreach (var item in treelist)
        //    {
        //        if (list.Any(e => e.GetPropertyValue(pidname) == item.GetPropertyValue(idname)))
        //        {
        //            var obj = list.ToTree<T, TF>(childrenname, idname, pidname, item.GetPropertyValue(idname));
        //            PropertyInfo[] pps = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //            foreach (var propert in pps)
        //            {
        //                if (propert.Name == childrenname)
        //                {
        //                    //propert.SetValue(l, childrenname);

        //                    //propert.SetValue(obj, TypeTransaction(propert.Name, propert.PropertyType), null);
        //                }
        //            }
        //        }
        //    }

        //    return treelist;
        //}

        /// <summary>
        /// 将列表转换为树形结构
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">数据</param>
        /// <param name="rootwhere">根条件</param>
        /// <param name="childswhere">节点条件</param>
        /// <param name="addchilds">添加子节点</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<T> ToTree<T>(this List<T> list, Func<T, T, bool> rootwhere, Func<T, T, bool> childswhere, Action<T, IEnumerable<T>> addchilds, T entity = default(T))
        {
            var treelist = new List<T>();
            //空树
            if (list == null || list.Count == 0)
            {
                return treelist;
            }
            if (!list.Any<T>(e => rootwhere(entity, e)))
            {
                return treelist;
            }

            //树根
            if (list.Any<T>(e => rootwhere(entity, e)))
            {
                treelist.AddRange(list.Where(e => rootwhere(entity, e)));
            }

            //树叶
            foreach (var item in treelist)
            {
                if (list.Any(e => childswhere(item, e)))
                {
                    var nodedata = list.Where(e => childswhere(item, e)).ToList();
                    foreach (var child in nodedata)
                    {
                        //添加子集
                        var data = list.ToTree(childswhere, childswhere, addchilds, child);
                        addchilds(child, data);
                    }
                    addchilds(item, nodedata);
                }
            }

            return treelist;
        }

    }
}
