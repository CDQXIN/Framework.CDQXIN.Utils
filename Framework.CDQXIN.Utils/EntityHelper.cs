using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public class EntityHelper
    {
        /// <summary>
        /// 把指定的实体转换为DataTable。
        /// </summary>
        /// <param name="list">实体列表。</param>
        /// <returns>数据表。</returns>
        public static DataTable ConvertToDataTable(IList list)
        {
            Type elementType = null;
            Type listType = list.GetType();

            if (listType.IsGenericType)
                elementType = listType.GetGenericArguments()[0];
            else if (listType.HasElementType)
                elementType = listType.GetElementType();
            else if (list.Count != 0)
                elementType = list[0].GetType();
            else
                throw new NotSupportedException("list can not be converted.");

            DataTable dt = CreateDataTable(elementType);
            foreach (object info in list)
                FillDataTable(dt, elementType, info);

            return dt;
        }

        /// <summary>
        /// 把指定数据表转为实体列表（要求数据表列名和实体属性名一致）。
        /// </summary>
        /// <typeparam name="T">实体类。</typeparam>
        /// <param name="dt">数据表。</param>
        /// <returns>实体实例。</returns>
        public static List<T> ConvertToEntityList<T>(DataTable dt)
            where T : class
        {
            List<T> list = new List<T>();
            if (dt == null)
                return list;
            foreach (DataRow row in dt.Rows)
                list.Add(ConvertToEntity<T>(row));
            return list;
        }

        /// <summary>
        /// 把指定的数据行转换为实体（要求数据表列名和实体属性名一致）。
        /// </summary>
        /// <typeparam name="T">实体类。</typeparam>
        /// <param name="row">数据行。</param>
        /// <returns>实体实例。</returns>
        public static T ConvertToEntity<T>(DataRow row)
            where T : class
        {
            Type elementType = typeof(T);
            T info = Activator.CreateInstance<T>();
            PropertyInfo[] properties = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length != 0)
            {
                foreach (PropertyInfo p in properties)
                {
                    if (p.CanWrite)
                    {
                        if (row.Table.Columns.Contains(p.Name))
                        {
                            if (row[p.Name] != DBNull.Value)
                                p.SetValue(info, GenerateSpecifiedTypeValue(p.PropertyType, row[p.Name]), null);
                        }
                    }
                }
            }
            else
            {
                foreach (FieldInfo f in elementType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (row.Table.Columns.Contains(f.Name))
                    {
                        if (row[f.Name] != DBNull.Value)
                            f.SetValue(info, GenerateSpecifiedTypeValue(f.FieldType, row[f.Name]));
                    }
                }
            }
            return info;
        }

        public static void ConvertEntity<S, T>(S source, T target)
            where S : class
            where T : class
        {
            PropertyInfo[] sProperties = typeof(S).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] tProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in tProperties)
            {
                if (p.CanWrite)
                {
                    foreach (PropertyInfo s in sProperties)
                    {
                        if (s.CanRead && s.Name == p.Name && s.PropertyType == p.PropertyType)
                        {
                            p.SetValue(target, s.GetValue(source, null), null);
                            break;
                        }
                    }
                }
            }

            FieldInfo[] sFields = typeof(S).GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] tFields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo f in tFields)
            {
                foreach (FieldInfo s in sFields)
                {
                    if (f.Name == s.Name && s.FieldType == f.FieldType)
                    {
                        f.SetValue(target, s.GetValue(source));
                        break;
                    }
                }
            }
        }

        private static Type GenerateDataColumnType(Type typ)
        {
            if (typ.IsEnum)
                return typeof(int);
            else if (typ == typeof(string))
                return typ;
            else if (typ.IsValueType && typ.IsPrimitive)
                return typ;
            else if (typ.IsGenericType && typ.IsValueType)
                return GenerateDataColumnType(typ.GetGenericArguments()[0]);
            else
                return null;
        }
        private static DataTable CreateDataTable(Type elementType)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] properties = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length != 0)
            {
                foreach (PropertyInfo p in properties)
                {
                    if (p.CanRead && GenerateDataColumnType(p.PropertyType) != null)
                        dt.Columns.Add(p.Name, GenerateDataColumnType(p.PropertyType));
                }
            }
            else
            {
                foreach (FieldInfo f in elementType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (GenerateDataColumnType(f.FieldType) != null)
                        dt.Columns.Add(f.Name, GenerateDataColumnType(f.FieldType));
                }
            }
            return dt;
        }
        private static object GenerateSpecifiedTypeValue(Type typ, object value)
        {
            if (typ.IsEnum)
                return System.Enum.ToObject(typ, value);
            else if (typ == typeof(string))
                return value;
            else if (typ.IsValueType && typ.IsPrimitive)
                return value;
            else if (typ.IsGenericType && typ.IsValueType)
                return GenerateSpecifiedTypeValue(typ.GetGenericArguments()[0], value);
            else
                return value;
        }
        private static void FillDataTable(DataTable dt, Type elementType, object info)
        {
            DataRow row = dt.NewRow();
            PropertyInfo[] properties = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length != 0)
            {
                foreach (PropertyInfo p in properties)
                {
                    if (p.CanRead)
                    {
                        if (dt.Columns.Contains(p.Name))
                        {
                            object v = p.GetValue(info, null);
                            if (v != null) row[p.Name] = v;
                        }
                    }
                }
            }
            else
            {
                foreach (FieldInfo f in elementType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (dt.Columns.Contains(f.Name))
                    {
                        object v = f.GetValue(info);
                        if (v != null) row[f.Name] = v;
                    }
                }
            }
            dt.Rows.Add(row);
        }
        /// <summary>
        /// 判断两个两个类的所有字段值是否相等
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool ObjectEquel(object obj1, object obj2)
        {
            Type type1 = obj1.GetType();
            Type type2 = obj2.GetType();

            PropertyInfo[] properties1 = type1.GetProperties();
            PropertyInfo[] properties2 = type2.GetProperties();

            bool IsMatch = true;
            for (int i = 0; i < properties1.Length; i++)
            {
                string s = properties1[i].DeclaringType.Name;
                if (properties1[i].GetValue(obj1, null).ToString() != properties2[i].GetValue(obj2, null).ToString())
                {
                    IsMatch = false;
                    break;
                }
            }

            return IsMatch;
        }
    }
}
