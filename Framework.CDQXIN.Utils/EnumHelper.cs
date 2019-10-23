using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public class EnumHelper
    {
        #region 枚举类型的描述
        /// <summary>
        /// 枚举类型的描述
        /// </summary>
        /// <param name="enumobj">枚举</param>
        /// <returns>枚举说明</returns>
        public static string GetDescription(Enum enumobj)
        {

            DescriptionAttribute attribute = enumobj.GetType().GetField(enumobj.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            return attribute == null ? enumobj.ToString() : attribute.Description;
        }
        #endregion

        #region 将枚举转换为ArrayList
        /// <summary>
        /// 将枚举转换为ArrayList
        /// 说明：
        /// 若不是枚举类型，则返回NULL
        /// 单元测试-->通过
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>ArrayList</returns>
        public static ArrayList ToArrayList(Type type)
        {
            if (type.IsEnum)
            {
                ArrayList array = new ArrayList();
                Array enumValues = Enum.GetValues(type);
                foreach (Enum value in enumValues)
                {
                    array.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
                }
                return array;
            }
            return null;
        }
        #endregion

        #region 将枚举转成DataTable
        /// <summary>
        /// 将枚举转成DataTable
        /// </summary>
        /// <param name="enumType">类型</param>
        /// <param name="valuetype"></param>
        /// <returns></returns>
        public static DataTable ConvertEnumToTable(Type enumType, int valuetype)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("TEXT", typeof(string)));

            if (valuetype == 1)
            {
                dt.Columns.Add(new DataColumn("VALUE", typeof(int)));
            }
            else if (valuetype == 2)
            {
                dt.Columns.Add(new DataColumn("VALUE", typeof(char)));
            }

            Array items = Enum.GetValues(enumType);

            foreach (var array in items)
            {
                DataRow dr = dt.NewRow();

                dr["TEXT"] = Enum.GetName(enumType, array);

                if (valuetype == 1)
                    dr["VALUE"] = Convert.ToInt32(array);
                else if (valuetype == 2)
                    dr["VALUE"] = Convert.ToChar(array);

                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();

            return dt;
        }
        #endregion

        #region 根据EnumValue获取对应的描述信息
        /// <summary>
        /// 根据EnumValue获取对应的描述信息
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static string GetDescriptionByValue<T>(string value)
        {
            var enumValues = Enum.GetValues(typeof(T));

            foreach (Enum item in enumValues)
            {
                if (item.ToString() == value)
                {
                    return GetDescription(item);
                }
            }

            return "";
        }
        #endregion

        #region 根据Index获取对应的描述信息
        /// <summary>
        /// 根据Index获取枚举描述信息
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="index">index</param>
        /// <returns></returns>
        public static string GetDescriptionByIndex<T>(int index)
        {
            foreach (Enum item in Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(item) == index)
                {
                    return GetDescription(item);
                }
            }

            return "";
        }
        #endregion

        #region 根据枚举描述信息获取对应的枚举值
        /// <summary>
        /// 根据枚举描述信息获取对应的枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="descriptionValue"></param>
        /// <returns></returns>
        public static Enum GetEnum<T>(string descriptionValue)
        {
            var enumValues = Enum.GetValues(typeof(T));

            return enumValues.Cast<Enum>().FirstOrDefault(value => GetDescription(value) == descriptionValue);
        }
        #endregion
    }
}
