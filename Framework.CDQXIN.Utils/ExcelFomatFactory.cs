using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public class ExcelFomatFactory
    {
        private static class ExcelFomatHolder
        {
            public static ExcelFomatFactory instance = new ExcelFomatFactory();
        }
        public static ExcelFomatFactory GetIntance() 
        {
            return ExcelFomatHolder.instance;
        }
        public static string Builder<T>(List<T> tList) 
        {
            string htmlContext = "";
            int row = 0;
            T t = default(T);
            htmlContext += BuildHeaderExcelHtml<T>(t,out row);
            var strBody = "";
            if (tList?.Count>0)
            {
                foreach (T perT in tList)
                {
                    row++;
                    strBody += BuildBodyExcelHtml<T>(perT,row);
                }
            }
            htmlContext += strBody;
            return htmlContext;
        }
        /// <summary>
        /// 拼接内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">实体</param>
        /// <param name="row">行索引</param>
        /// <returns></returns>
        public static string BuildBodyExcelHtml<T>(T t,int row)
        {
            string htmlContext = "";
            Type type;
            if (t!=null)
            {
                type = t.GetType();
            }
            else
            {
                type = typeof(T);
            }
            htmlContext += "<tr>";
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var prop in type.GetProperties())
            {
                if (prop.IsDefined(typeof(ExcelRuleAttribute),true))
                {
                    var attributes = prop.GetCustomAttributes(typeof(ExcelRuleAttribute),true);
                    foreach (ExcelRuleAttribute item in attributes)
                    {
                        if (t!=null)
                        {
                            var tValue = prop.GetValue(t);
                            string strShow = tValue == null ? "" : tValue.ToString();
                            if (!string.IsNullOrWhiteSpace(item.ShowFmat))
                            {
                                if (prop.PropertyType==typeof(DateTime?)||prop.PropertyType==typeof(DateTime))
                                {
                                    strShow = ConvertHelper.GetDateTimeString(tValue,item.ShowFmat);
                                }
                                else if (prop.PropertyType==typeof(string))
                                {
                                    if (item.ShowFmat==@"/")
                                    {
                                        strShow = "0.00";
                                    }
                                }
                            }
                            var str = $"<td rowspan='{item.RowSpan}' colspan='{item.ColSpan}' row='{row}' col='{item.Col}' fmat='{item.Fmat}'>{strShow}</td>";
                            dic.Add(item.Col,str);
                        }
                    }
                }
            }

            foreach (var index in dic.Keys.OrderBy(p=>p).ToList())
            {
                htmlContext += dic[index];
            }
            htmlContext += "</tr>";
            return htmlContext;
        }
        /// <summary>
        /// 拼接复杂表头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">null</param>
        /// <param name="row">行数</param>
        /// <returns></returns>
        public static string BuildHeaderExcelHtml<T>(T t,out int row)
        {
            string htmlContext = "";
            Type type;
            if (t!=null)
            {
                type = t.GetType();
            }
            else
            {
                type = typeof(T);
            }
            List<Coordinate> list = new List<Coordinate>();
            foreach (var prop in type.GetProperties())
            {
                Dictionary<int, string> dicx = new Dictionary<int, string>();
                if (prop.IsDefined(typeof(ExcelRuleAttribute),true))
                {
                    var attributes = prop.GetCustomAttributes(typeof(ExcelRuleAttribute),true);
                    foreach (ExcelRuleAttribute item in attributes)
                    {
                        if (t==null)
                        {
                            var str = $"<td rowspan='{item.RowSpan}' colspan='{item.ColSpan}' row='{item.HeaderRol}' col='{item.HeaderCol}' fmat=''>{item.HeaderDescription}</td>";
                            Coordinate coor = new Coordinate { 
                            Rol=item.HeaderRol,
                            Col=item.HeaderCol,
                            Content=str
                            };
                            list.Add(coor);
                        }
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder perHtml = new StringBuilder();
            var rolList = list.OrderBy(p => p.Rol).Select(p => p.Rol).Distinct().ToList();
            foreach (var rol in rolList)
            {
                perHtml.Append("<tr>");
                var perList = list.Where(p => p.Rol == rol).OrderBy(p => p.Col).ToList();
                foreach (var perCoor in perList)
                {
                    perHtml.Append(perCoor.Content);
                }
                perHtml.Append("</tr>");
            }
            row = list.Max(p=>p.Rol);
            htmlContext += perHtml.ToString();
            return htmlContext;
        }

    }
    /// <summary>
    /// 支持复杂表头的Excel导出
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelRuleAttribute : Attribute
    {
        ///<summary>
        ///行合并数
        ///</summary>
        public int RowSpan { get; set; }
        ///<summary>
        ///列合并数
        ///</summary>
        public int ColSpan{ get; set; }
        ///<summary>
        ///列索引
        ///</summary>
        public int Col { get; set; }
        //public int Row{get；set；}
        //<summary>
        ///（已初始化字符串）导出excel层示格式
        ///</summary>
        public string Fmat { get; set; }
        /// <summary>
        /// 初始化字符串层示格式
        /// </summary>
        public string ShowFmat { get; set; }
        ///<summary
        //表头名字
        ///</sumary>
        public string HeaderDescription{ get; set; }
        /// <summary>
        /// 表头行索引
        /// </summary>
        public int HeaderRol { get; set; }
        /// <summary>
        /// 表头列索引
        /// </summary>
        public int HeaderCol{ get; set; }
    }

    public class Coordinate
    {
        /// <summary>
        /// 行索引
        /// </summary>
        public int Rol { get; set; }
        /// <summary>
        /// 列索引
        /// </summary>
        public int Col { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}