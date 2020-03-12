using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public class StringHelper
    {
        #region 获取字符串的实际字节长度的方法
        /// <summary>
        /// 获取字符串的实际字节长度的方法
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>实际长度</returns>
        public static int GetRealLength(string source)
        {
            return Encoding.Default.GetByteCount(source);
        }
        #endregion

        #region 按字节数截取字符串的方法
        /// <summary>
        /// 按字节数截取字符串的方法
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="n">要截取的字节数</param>
        /// <param name="needEndDot">是否需要结尾的省略号</param>
        /// <returns>截取后的字符串</returns>
        public static string SubString(string source, int n, bool needEndDot)
        {
            string temp = string.Empty;
            if (GetRealLength(source) <= n)//如果长度比需要的长度n小,返回原字符串
            {
                return source;
            }
            else
            {
                int t = 0;
                char[] q = source.ToCharArray();
                for (int i = 0; i < q.Length && t < n; i++)
                {
                    if ((int)q[i] > 127)//是否汉字
                    {
                        temp += q[i];
                        t += 2;
                    }
                    else
                    {
                        temp += q[i];
                        t++;
                    }
                }
                if (needEndDot)
                    temp += "...";
                return temp;
            }
        }
        #endregion

        #region 去除价格小数点后末尾0的方法
        /// <summary>
        /// 去除价格小数点后末尾0的方法
        /// </summary>
        /// <param name="price">去0之前的价格字符串</param>
        /// <returns>去0之后的价格字符串</returns>
        public static string TrimEndZeroForPrice(string price)
        {
            while (price.EndsWith("0") && price.IndexOf(".") > 0)
            {
                price = price.TrimEnd('0');
            }
            if (price.EndsWith("."))
                price = price.Substring(0, price.Length - 1);
            return price;
        }
        #endregion

        #region 截取规定小数点后位数的方法
        /// <summary>
        /// 截取规定小数点后位数的方法(带四舍五入）
        /// </summary>
        /// <param name="objDecimal">截取前的小数对象</param>
        /// <param name="length">要截取的小数位长度</param>
        /// <returns>截取后的小数字符串</returns>
        public static string SubSpecialLengthDecimal(object objDecimal, int length)
        {
            decimal strDecimal = ConvertHelper.GetDecimal(objDecimal);
            return strDecimal.ToString("f" + length);
        }
        #endregion

        #region 过滤字符串中注入SQL脚本的方法
        /// <summary>
        /// 过滤字符串中注入SQL脚本的方法
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string SqlFilter(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                //单引号替换成两个单引号
                input = input.Replace("'", "''");
                //半角封号替换为全角封号，防止多语句执行
                input = input.Replace(";", "；");
                //半角括号替换为全角括号
                input = input.Replace("(", "（");
                input = input.Replace(")", "）");
                //去除执行SQL语句的命令关键字
                Regex regexSelect = new Regex("select", RegexOptions.IgnoreCase);
                input = regexSelect.Replace(input, "");
                Regex regexInsert = new Regex("insert", RegexOptions.IgnoreCase);
                input = regexInsert.Replace(input, "");
                Regex regexUpdate = new Regex("update", RegexOptions.IgnoreCase);
                input = regexUpdate.Replace(input, "");
                Regex regexDelete = new Regex("delete", RegexOptions.IgnoreCase);
                input = regexDelete.Replace(input, "");
                Regex regexDrop = new Regex("drop", RegexOptions.IgnoreCase);
                input = regexDrop.Replace(input, "");
                Regex regexTruncate = new Regex("truncate", RegexOptions.IgnoreCase);
                input = regexTruncate.Replace(input, "");
                //去除执行存储过程的命令关键字
                Regex regexExec = new Regex("exec", RegexOptions.IgnoreCase);
                input = regexExec.Replace(input, "");
                Regex regexExecute = new Regex("execute", RegexOptions.IgnoreCase);
                input = regexExecute.Replace(input, "");

                //去除系统存储过程或扩展存储过程关键字
                Regex regexXp = new Regex("xp_", RegexOptions.IgnoreCase);
                input = regexXp.Replace(input, "x p_");
                Regex regexSp = new Regex("sp_", RegexOptions.IgnoreCase);
                input = regexSp.Replace(input, "s p_");

                //防止16进制注入
                Regex regexox = new Regex("0x", RegexOptions.IgnoreCase);
                input = regexox.Replace(input, "0 x");
                input = input.Trim();
            }

            return input;
        }
        public static string SqlFilter_Simple(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                //去除执行SQL语句的命令关键字
                Regex regexSelect = new Regex(@"\b(select)\b", RegexOptions.IgnoreCase);
                input = regexSelect.Replace(input, "");
                Regex regexInsert = new Regex(@"\b(insert)\b", RegexOptions.IgnoreCase);
                input = regexInsert.Replace(input, "");
                Regex regexUpdate = new Regex(@"\b(update)\b", RegexOptions.IgnoreCase);
                input = regexUpdate.Replace(input, "");
                Regex regexDelete = new Regex(@"\b(delete)\b", RegexOptions.IgnoreCase);
                input = regexDelete.Replace(input, "");
                Regex regexDrop = new Regex(@"\b(drop)\b", RegexOptions.IgnoreCase);
                input = regexDrop.Replace(input, "");
                Regex regexTruncate = new Regex(@"\b(truncate)\b", RegexOptions.IgnoreCase);
                input = regexTruncate.Replace(input, "");

                //去除执行存储过程的命令关键字
                Regex regexExec = new Regex(@"\b(exec)\b", RegexOptions.IgnoreCase);
                input = regexExec.Replace(input, "");
                Regex regexExecute = new Regex(@"\b(execute)\b", RegexOptions.IgnoreCase);
                input = regexExecute.Replace(input, "");

                //去除系统存储过程或扩展存储过程关键字
                Regex regexXp = new Regex(@"\b(xp_)\w", RegexOptions.IgnoreCase);
                input = regexXp.Replace(input, "x p_");
                Regex regexSp = new Regex(@"\b(sp_)\w", RegexOptions.IgnoreCase);
                input = regexSp.Replace(input, "s p_");
                input = input.Trim();
            }
            else
                input = string.Empty;

            return input;
        }
        #endregion

        #region 移除Html标签的方法
        /// <summary>
        /// 移除Html标签的方法
        /// </summary>
        /// <param name="str">移除Html标签之前的字符串</param>
        /// <returns>移除Html标签之后的字符串</returns>
        public static string RemoveHtmlTag(string source)
        {
            source = source.Replace("&nbsp;", "");
            source = source.Replace("\r\n", "");
            return Regex.Replace(source, "<[^>]*>", "");
        }

        #endregion
        /// <summary>  
        /// 清除文本中Html的标签    不包括图片
        /// </summary>  
        /// <param name="Content"></param>  
        /// <returns></returns>  
        public static string ClearHtmlLeftPic(string Content)
        {
            Content = Zxj_ReplaceHtml("&#[^>]*;", "", Content);
            Content = Zxj_ReplaceHtml("</?marquee[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?object[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?param[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?embed[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?table[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml(" ", "", Content);
            Content = Zxj_ReplaceHtml("</?tr[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?p[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?a[^>]*>", "", Content);
            //Content = Zxj_ReplaceHtml("</?img[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?tbody[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?li[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?span[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?div[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?td[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?script[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("(javascript|jscript|vbscript|vbs):", "", Content);
            Content = Zxj_ReplaceHtml("on(mouse|exit|error|click|key)", "", Content);
            Content = Zxj_ReplaceHtml("<\\?xml[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("<\\/?[a-z]+:[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?font[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?b[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?u[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?i[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?strong[^>]*>", "", Content);
            string clearHtml = Content;
            return clearHtml;
        }

        /// <summary>  
        /// 清除文本中的Html标签  
        /// </summary>  
        /// <param name="patrn">要替换的标签正则表达式</param>  
        /// <param name="strRep">替换为的内容</param>  
        /// <param name="content">要替换的内容</param>  
        /// <returns></returns>  
        public static string Zxj_ReplaceHtml(string patrn, string strRep, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                content = "";
            }
            Regex rgEx = new Regex(patrn, RegexOptions.IgnoreCase);
            string strTxt = rgEx.Replace(content, strRep);
            return strTxt;
        }

        #region 获取8位当前日期字符串的方法
        /// <summary>
        /// 获取8位当前日期字符串的方法
        /// </summary>
        /// <returns>8位当前日期字符串</returns>
        public static string GetCurrentDateString()
        {
            DateTime now = DateTime.Now;
            return string.Concat(now.Year, GetSpecialNumericString(now.Month, 2), GetSpecialNumericString(now.Day, 2));
        }
        #endregion

        #region 获取6位当前日期字符串的方法
        /// <summary>
        /// 获取6位当前日期字符串的方法
        /// </summary>
        /// <returns>6位当前日期字符串</returns>
        public static string GetSmallCurrentDateString()
        {
            string currentDateString = GetCurrentDateString();
            return currentDateString.Substring(2);
        }
        #endregion

        #region 获取指定长度数字字符串的方法，不足位数用0填充
        /// <summary>
        /// 获取指定长度数字字符串的方法，不足位数用0填充
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="length">指定的长度</param>
        /// <returns>指定长度数字字符串</returns>
        public static string GetSpecialNumericString(int number, int length)
        {
            return number.ToString("d" + length);
        }
        #endregion

        #region 移除数字字符串开始0的方法
        /// <summary>
        /// 移除数字字符串开始0的方法
        /// </summary>
        /// <param name="source">移除前的字符串</param>
        /// <returns>移除后的字符串</returns>
        public static string TrimStartZero(string source)
        {
            while (source.StartsWith("0"))
            {
                source = source.Substring(1);
            }
            return source;
        }
        #endregion

        #region 转换字符串编码的方法
        /// <summary>
        /// 转换字符串编码的方法
        /// </summary>
        /// <param name="dstEncoding">转换后的编码格式</param>
        /// <param name="s">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ConvertEncoding(Encoding dstEncoding, string s)
        {
            return ConvertEncoding(Encoding.Default, dstEncoding, s);
        }
        /// <summary>
        /// 转换字符串编码的方法
        /// </summary>
        /// <param name="srcEncoding">转换前的编码格式</param>
        /// <param name="dstEncoding">转换后的编码格式</param>
        /// <param name="s">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ConvertEncoding(Encoding srcEncoding, Encoding dstEncoding, string s)
        {
            byte[] bytes = Encoding.Default.GetBytes(s);
            bytes = Encoding.Convert(srcEncoding, dstEncoding, bytes);
            return Encoding.Default.GetString(bytes);
        }
        #endregion

        #region 将全角字符串转成半角字符串的方法
        /// <summary>
        /// 将全角字符串转成半角字符串的方法
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>半角字符串</returns>
        public static string ConvertDbcToSbcString(string source)
        {
            StringBuilder sb = new StringBuilder();
            char[] c = source.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                sb.Append(ConvertDbcToSbcChar(c[i]));
            }
            return sb.ToString();
        }
        #endregion

        #region 将字符串数组转成逗号分割字符串的方法
        /// <summary>
        /// 将字符串数组转成逗号分割字符串的方法
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns>逗号分割字符串</returns>
        public static string ConvertStringArrayToStrings(string[] strArray)
        {
            if (strArray == null)
            {
                return "";
            }

            string result = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                result += strArray[i] + ",";
            }
            return result.TrimEnd(new char[] { ',' });
        }
        #endregion

        #region 对字符串进行Url编码的方法
        /// <summary>
        /// 对字符串进行Url编码的方法
        /// </summary>
        /// <param name="s">要进行Url编码的字符串</param>
        /// <returns>Url编码后的字符串</returns>
        public static string UrlEncode(string s)
        {
            return UrlEncode(s, Encoding.Default);
        }
        /// <summary>
        /// 对字符串进行Url编码的方法
        /// </summary>
        /// <param name="s">要进行Url编码的字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>Url编码后的字符串</returns>
        public static string UrlEncode(string s, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = encoding.GetBytes(s);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 将全角字符转成半角字符的方法
        /// </summary>
        /// <param name="c">转换前的字符</param>
        /// <returns>半角字符</returns>
        private static char ConvertDbcToSbcChar(char c)
        {
            //得到c的编码
            byte[] bytes = Encoding.Unicode.GetBytes(c.ToString());

            int H = Convert.ToInt32(bytes[1]);
            int L = Convert.ToInt32(bytes[0]);

            //得到unicode编码
            int value = H * 256 + L;

            //是全角
            if (value >= 65281 && value <= 65374)
            {
                int halfvalue = value - 65248;//65248是全半角间的差值。
                byte halfL = Convert.ToByte(halfvalue);

                bytes[0] = halfL;
                bytes[1] = 0;
            }
            else if (value == 12288)
            {
                int halfvalue = 32;
                byte halfL = Convert.ToByte(halfvalue);

                bytes[0] = halfL;
                bytes[1] = 0;
            }
            else
            {
                return c;
            }

            //将bytes转换成字符
            string ret = Encoding.Unicode.GetString(bytes);

            return Convert.ToChar(ret);
        }
        #endregion
        /// 去除标签中的script属性
        private static string StripScriptAttributesFromTags(string str)
        {

            //整体去除
            string pattern = @"(?&lt;ScriptAttr&gt;on\w+=\s*(['""\s]?)([/s/S]*[^\1]*?)\1)[\s|&gt;|/&gt;]";
            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match m in r.Matches(str))
            {
                string attrs = m.Groups["ScriptAttr"].Value;
                if (!string.IsNullOrEmpty(attrs))
                {
                    str = str.Replace(attrs, string.Empty);
                }
            }

            //滤除包含script的href
            str = FilterHrefScript(str);

            return str;
        }
        /// 滤除包含script的href
        public static string FilterHrefScript(string str)
        {
            //整体去除，不能去除不被单引号或双引号包含的属性值
            string regexstr = @" href[ ^=]*=\s*(['""\s]?)[\w]*script+?:([/s/S]*[^\1]*?)\1[\s]*";
            return Regex.Replace(str, regexstr, " ", RegexOptions.IgnoreCase);
        }
        /// 滤除script引用和区块
        public static string FilterScript(string str)
        {
            string pattern = @"&lt;script[\s\S]+&lt;/script *&gt;";
            return StripScriptAttributesFromTags(Regex.Replace(str, pattern, string.Empty, RegexOptions.IgnoreCase));
        }

        //替换后几位字符
        public static string ReplaceEndChar(string str, char c, int len)
        {
            if (str != "" && str.Length > 4)
            {
                string endstr = "";

                for (int i = 0; i < len; i++)
                {
                    endstr = endstr + c;
                }

                return str.Substring(0, str.Length - len) + endstr;
            }
            else
            {
                return "";
            }

        }


        #region 对bg2312和utf-8编码的url进行解码
        /// <summary>
        /// 对bg2312和utf-8编码的url进行解码
        /// </summary>
        /// <param name="url">需要解码的url</param>
        /// <returns></returns>
        public static string UrlDecode(string url)
        {
            string result = "";
            byte[] buf = GetUrlCodingToBytes(url);
            if (IsUTF8(buf))
            {
                result = HttpUtility.UrlDecode(url, Encoding.UTF8);
            }
            else
            {
                result = HttpUtility.UrlDecode(url, Encoding.GetEncoding("GB2312"));
            }
            return result;
        }
        //将编码的url拆分成数组
        public static byte[] GetUrlCodingToBytes(string url)
        {
            StringBuilder sb = new StringBuilder();
            int i = url.IndexOf('%');
            while (i >= 0)
            {
                if (url.Length < i + 3)
                {
                    break;
                }
                sb.Append(url.Substring(i, 3));
                url = url.Substring(i + 3);
                i = url.IndexOf('%');
            }
            string urlCoding = sb.ToString();
            if (string.IsNullOrEmpty(urlCoding))
                return new byte[0];
            urlCoding = urlCoding.Replace("%", string.Empty);
            int len = urlCoding.Length / 2;
            byte[] result = new byte[len];
            len *= 2;
            for (int index = 0; index < len; index++)
            {
                string s = urlCoding.Substring(index, 2);
                int b = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
                result[index / 2] = (byte)b;
                index++;
            }
            return result;
        }
        //判断url编码格式是不是utf-8
        public static bool IsUTF8(byte[] buf)
        {
            int i;
            byte cOctets; // octets to go in this UTF-8 encoded character
            bool bAllAscii = true;
            long iLen = buf.Length;
            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                if ((buf[i] & 0x80) != 0) bAllAscii = false;
                if (cOctets == 0)
                {
                    if (buf[i] >= 0x80)
                    {
                        do
                        {
                            buf[i] <<= 1;
                            cOctets++;
                        }
                        while ((buf[i] & 0x80) != 0);
                        cOctets--;
                        if (cOctets == 0)
                            return false;
                    }
                }
                else
                {
                    if ((buf[i] & 0xC0) != 0x80)
                        return false;
                    cOctets--;
                }
            }
            if (cOctets > 0)
                return false;
            if (bAllAscii)
                return false;
            return true;
        }
        #endregion

        public static List<string> SplitStringOfSpace(string strSearch)
        {
            List<string> lisStr = new List<string>();
            string[] mm = Regex.Split(strSearch, "\\s+", RegexOptions.IgnoreCase);
            for (int i = 0; i < mm.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(mm[i]))
                {
                    lisStr.Add(mm[i]);
                }
            }
            return lisStr;
        }

        #region 金额转换成中文大写金额
        /// <summary>
        /// 金额转换成中文大写金额
        /// </summary>
        /// <param name="LowerMoney">eg:10.74</param>
        /// <returns></returns>
        public static string MoneyToUpper(string LowerMoney)
        {
            if (LowerMoney == "0" || LowerMoney == "0.00" || string.IsNullOrWhiteSpace(LowerMoney))
                return "零元整";
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (LowerMoney.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                LowerMoney = LowerMoney.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
            if (LowerMoney.IndexOf(".") > 0)
            {
                if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
                {
                    LowerMoney = LowerMoney + "0";
                }
            }
            else
            {
                LowerMoney = LowerMoney + ".00";
            }
            strLower = LowerMoney;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }
        #endregion

        #region 分析 url 字符串中的参数信息
        /// <summary>
        /// 分析 url 字符串中的参数信息
        /// </summary>
        /// <param name="url">输入的 URL</param>
        /// <param name="baseUrl">输出 URL 的基础部分</param>
        /// <param name="nvc">输出分析后得到的 (参数名,参数值) 的集合</param>
        public static NameValueCollection ParseUrl(string url)
        {
            NameValueCollection nvc = new NameValueCollection();
            if (url == null)
                throw new ArgumentNullException("url");
            nvc = new NameValueCollection();
            string baseUrl = "";
            if (url == "")
                return nvc;
            int questionMarkIndex = url.IndexOf('?');
            if (questionMarkIndex == -1)
            {
                baseUrl = url;
                return nvc;
            }
            baseUrl = url.Substring(0, questionMarkIndex);
            if (questionMarkIndex == url.Length - 1)
                return nvc;
            string ps = url.Substring(questionMarkIndex + 1);
            // 开始分析参数对  
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);
            foreach (Match m in mc)
            {
                nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
            return nvc;
        }
        #endregion

        #region 获取url字符串参数，返回参数值字符串
        /// 获取url字符串参数，返回参数值字符串
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="url">url字符串</param>
        /// <returns></returns>
        public static string GetQueryString(string name, string url)
        {
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(url);
            foreach (Match m in mc)
            {
                if (m.Result("$2").Equals(name))
                {
                    return m.Result("$3");
                }
            }
            return "";
        }
        #endregion

        #region 隐藏手机号中间几位
        /// <summary>
        /// 隐藏手机号中间几位
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string HideMobile(object mobile)
        {
            if (mobile != null)
            {
                if (!string.IsNullOrWhiteSpace(mobile.ToString()))
                    return Regex.Replace(mobile.ToString(), "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
                else
                    return "";
            }
            else
                return "";
        }
        #endregion

        #region  判断是否是手机号
        /// <summary>
        /// 判断是否是手机号
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool IsMobil(string strInput)
        {
            return !string.IsNullOrEmpty(strInput) && Regex.IsMatch(strInput, @"^1[3456789]\d{9}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region C#实现Java版的UrlEncode方法
        /// <summary>
        /// C#实现Java版的UrlEncode方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeOfJava(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).Length > 1)
                {
                    sb.Append(HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).ToUpper());
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        #endregion

    }
}
