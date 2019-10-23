using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils.ExtensionHelper
{
    /// <summary>
    /// String扩展
    /// </summary>
    public static class StringExtHelper
    {
        /// <summary>
        /// 判断字符串是否为Null或者为空
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns>是否为Null或者为空</returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// 判断字符串是否为Null或者为空
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// 判断字符串是否为Null或者为空字符组成
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns>否为Null或者为空字符组成</returns>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// 判断字符串是否为Null或者为空字符组成
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNotNullOrWhiteSpace(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// 获取字符串的Md5值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="code">默认32；code:16 or 32</param>
        /// <returns></returns>
        public static string GetMd5(this string str, int code = 32)
        {
            var hashData = GetMd5ToBytes(str);

            // 由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            string strHashData = BitConverter.ToString(hashData);

            // 替换-
            var md5 = strHashData.Replace("-", string.Empty);

            return code == 16 ? md5.Substring(8, 16) : md5;
        }

        /// <summary>
        /// 获取md5 ToBase64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5ToBase64(this string str)
        {
            var hashData = GetMd5ToBytes(str);

            return Convert.ToBase64String(hashData);
        }

        /// <summary>
        /// 获取Md5 To Bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetMd5ToBytes(this string str)
        {
            using (MD5CryptoServiceProvider oMd5Hasher = new MD5CryptoServiceProvider())
            {
                var arrbytHashValue = oMd5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));

                return arrbytHashValue;
            }
        }

        /// <summary>
        /// 获取字符串的哈希码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string GetSha1(this string str)
        {
            var sha1Hasher = new SHA1CryptoServiceProvider();
            var arrbytHashValue = sha1Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));

            // 由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            string strHashData = BitConverter.ToString(arrbytHashValue);

            // 替换-
            return strHashData.Replace("-", string.Empty);
        }

        /// <summary>
        /// 判断指定字符串是否合法的日期格式
        /// </summary>
        /// <param name="input">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsDateTime(this string input)
        {
            DateTime dt;
            return DateTime.TryParse(input, out dt);
        }

        /// <summary>
        /// 判断指定的字符串是否为数字
        /// </summary>
        /// <param name="str">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsInt(this string str)
        {
            return !string.IsNullOrEmpty(str) && Regex.IsMatch(str, "^-?\\d+$");
        }

        /// <summary>
        /// 判断指定的字符串是否为Url地址
        /// </summary>
        /// <param name="str">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsUrl(this string str)
        {
            return RegexHelper.IsMatch(str, EnumRegex.UrlAddress);
        }

        /// <summary>
        /// 判断指定的字符串是否为合法Email
        /// </summary>
        /// <param name="str">指定的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsEmail(this string str)
        {
            return RegexHelper.IsMatch(str, EnumRegex.Email);
        }

        /// <summary>
        /// 对字符串进行 HTML 编码并返回已编码的字符串
        /// </summary>
        /// <param name="content">要编码的文本字符串</param>
        /// <returns>HTML 已编码的文本</returns>
        public static string HtmlEncode(this string content)
        {
            return HttpUtility.HtmlEncode(content);
        }

        /// <summary>
        /// 对 HTML 编码的字符串进行解码，并返回已解码的字符串
        /// </summary>
        /// <param name="content">要解码的文本字符串</param>
        /// <returns>已解码的字符串</returns>
        public static string HtmlDecode(this string content)
        {
            return HttpUtility.HtmlDecode(content);
        }

        /// <summary>
        /// 对 URL 字符串进行编码, 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">要编码的文本</param>
        /// <param name="e">编码</param>
        /// <returns>一个已编码的字符串</returns>
        public static string UrlEncode(this string str, Encoding e = null)
        {
            e = e ?? Encoding.UTF8;
            return HttpUtility.UrlEncode(str, e);
        }

        /// <summary>
        /// 对 URL 字符串进行解码, 返回 URL 字符串的解码结果
        /// </summary>
        /// <param name="str">要解码的文本</param>
        /// <param name="e">编码</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(this string str, Encoding e = null)
        {
            e = e ?? Encoding.UTF8;
            return HttpUtility.UrlDecode(str, e);
        }

        /// <summary>
        /// url添加参数
        /// </summary>
        /// <param name="url">原url地址</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static string AddUrlParameter(this string url, string key, string value)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key)) return url;

            return string.Concat(url, url.Contains("?") ? "&" : "?", key, "=", value);
        }

        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="html">包括HTML的源码</param>
        /// <returns>已经去除后的文字</returns>
        public static string TrimHtml(this string html)
        {
            // 删除脚本和嵌入式CSS   
            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<style[^>]*?>.*?</style>", string.Empty, RegexOptions.IgnoreCase);

            // 删除HTML   
            var regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            html = regex.Replace(html, string.Empty);
            html = Regex.Replace(html, @"<(.[^>]*)>", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", string.Empty, RegexOptions.IgnoreCase);

            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", string.Empty, RegexOptions.IgnoreCase);

            html = html.Replace("<", string.Empty);
            html = html.Replace(">", string.Empty);
            html = html.Replace("\r\n", string.Empty);

            return html;
        }

        /// <summary>
        /// String to Decimal(字符串 转换成 数值、十进制)
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="def">Default</param>
        /// <returns>Byte</returns>
        public static decimal ToDecimal(this string s, decimal def = default(decimal))
        {
            decimal result;
            return Decimal.TryParse(s, out result) ? result : def;
        }

        /// <summary>
        /// String to Double(字符串 转换成 数值、浮点)
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="def">Default</param>
        /// <returns>Byte</returns>
        public static double ToDouble(this string s, double def = default(double))
        {
            double result;
            return Double.TryParse(s, out result) ? result : def;
        }

        /// <summary>
        /// String to Single(字符串 转换成 数值、浮点)
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="def">Default</param>
        /// <returns>Byte</returns>
        public static float ToSingle(this string s, float def = default(float))
        {
            float result;
            return Single.TryParse(s, out result) ? result : def;
        }

        /// <summary>
        /// String to Byte(字符串 转换成 无符号、数值、整数)
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="def">Default</param>
        /// <returns>Byte</returns>
        public static byte ToByte(this string s, byte def = default(byte))
        {
            byte result;
            return Byte.TryParse(s, out result) ? result : def;
        }

        /// <summary>
        /// 将指定字符转换成byte[]
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// String to Int32(字符串 转换成 有符号、数值、整数)
        /// </summary>
        /// <remarks>
        ///  2014-06-23 16:31 Created By iceStone
        /// </remarks>
        /// <param name="s">String</param>
        /// <param name="def">Default</param>
        /// <returns>Byte</returns>
        public static int ToInt32(this string s, int def = default(int))
        {
            int result;
            return Int32.TryParse(s, out result) ? result : def;
        }

        /// <summary>
        /// String to DateTime(字符串 转换成 时间)
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="def">Default</param>
        /// <returns>Byte</returns>
        public static DateTime ToDateTime(this string s, DateTime def = default(DateTime))
        {
            //时间字符串格式 yyyyMMdd
            if (s.Length == 8 && RegexHelper.IsMatch(s, "^[0-9]{4}[0-9]{2}[0-9]{2}$"))
            {
                return new DateTime(int.Parse(s.Substring(0, 4)), int.Parse(s.Substring(4, 2)), int.Parse(s.Substring(6, 2)));
            }

            DateTime result;
            return DateTime.TryParse(s, out result) ? result : def;
        }

        /// <summary>
        /// 字符串填补字符
        /// <remarks>
		/// 例如: 100 可以在前填充两个0 写法 "100".Repair(4) -->"00100"
        /// </remarks>
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的总长度</param>
        /// <param name="str">填充字符</param>
        /// <param name="isfirst">前填充</param>
        /// <returns></returns>
        public static string Repair(this string text, int limitedLength, string str = "0", bool isfirst = true)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                sb.Append(str);
            }

            if (isfirst)
            {
                return sb.ToString() + text;
            }
            else
            {
                return text + sb.ToString();
            }
        }

        /// <summary>
        /// 数字字符串实现各进制数间的转换扩展
        /// <remarks>
        /// 但数字
        /// </remarks>
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        /// <returns></returns>
        public static string ToScale(this string value, int from, int to)
        {
            try
            {
                var intValue = Convert.ToInt32(value, from);  //先转成10进制

                var result = Convert.ToString(intValue, to);  //再转成目标进制

                if (to != 2) return result;

                var resultLength = result.Length;  //获取二进制的长度

                switch (resultLength)
                {
                    case 7:
                        result = "0" + result;
                        break;
                    case 6:
                        result = "00" + result;
                        break;
                    case 5:
                        result = "000" + result;
                        break;
                    case 4:
                        result = "0000" + result;
                        break;
                    case 3:
                        result = "00000" + result;
                        break;
                }
                return result;
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="str">半角字符串</param>
        /// <returns></returns>
        public static string ToSbc(this string str)
        {
            //半角转全角：
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 转半角的函数(SBC case)
        /// </summary>
        /// <param name="str">全角字符串</param>
        /// <returns></returns>
        public static string ToDbc(this string str)
        {
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="str">原来字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>截取的字符串</returns>
        public static string ClipString(this string str, int len)
        {
            bool isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += str.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            byte[] mybyte = Encoding.UTF8.GetBytes(str);
            if (isShowFix && mybyte.Length > len)
            {
                tempString += "…";
            }

            return tempString;
        }

        /// <summary>
        /// 将字符串转化为Json对象类型
        /// </summary>
        /// <param name="jsonstr">JSON字符串</param>
        /// <returns>Json对象</returns>
        public static JObject ToJObject(this string jsonstr)
        {
            return JObject.Parse(jsonstr);
        }

        /// <summary>
        /// 将字符串转化为实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="jsonstr">JSON字符串</param>
        /// <returns>实体对象</returns>
        public static T ToObject<T>(this string jsonstr)
        {
            T result = default(T);
            try
            {
                if (jsonstr.IsNotNullOrEmpty())
                {
                    result = JsonConvert.DeserializeObject<T>(jsonstr);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// json字符串转换成字典
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonStrToDic(this string jsonString)
        {
            var dic = jsonString.Trim('{', '}')
                 .Split(',')
                 .ToDictionary(s => s.Split(':')[0].Trim('\"').Trim('\''),
                     s => (object)s.Split(':')[1].Trim('\"').Trim('\''));
            return dic;
        }

        /// <summary>
        /// 判断字符串前缀
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strStart">前缀</param>
        /// <returns></returns>
        public static bool IsStartWith(this string str, string strStart)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (str.IndexOf(strStart, StringComparison.Ordinal) == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断字符串后缀
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strEnd">后缀</param>
        /// <returns></returns>
        public static bool IsEndWith(this string str, string strEnd)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (str.LastIndexOf(strEnd, StringComparison.Ordinal) != -1)
            {
                if (str.LastIndexOf(strEnd, StringComparison.Ordinal) == str.Length - strEnd.Length)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 移除换行
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string TrimWrap(this string str)
        {
            if (str == null)
            {
                return null;
            }

            return str.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        }

        /// <summary>
        /// string 转换金钱格式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dotNum">小数位</param>
        /// <returns></returns>
        public static string ToMoney(this string str, int dotNum)
        {
            string dStr = Math.Round(decimal.Parse(str), dotNum) + "";
            return decimal.Parse(dStr).ToString("N2");
        }

        /// <summary>
        /// 模板字符串替换,例如{{UserName}}
        /// </summary>
        /// <param name="oldValue">字符串模板</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Replace(this string oldValue, object obj)
        {
            Dictionary<string, object> dic = obj.ToDictionary();

            return dic.Aggregate(oldValue, (current, item) => current.Replace("{{" + item.Key + "}}", item.Value.ToString()));
        }

        /// <summary>
        /// 模板字符串替换,例如{{UserName}}
        /// </summary>
        /// <param name="oldValue">字符串模板</param>
        /// <param name="dic">键值集合</param>
        /// <returns></returns>
        public static string ReplaceDic(this string oldValue, Dictionary<string, object> dic)
        {
            return dic.Aggregate(oldValue, (current, item) => current.Replace("{{" + item.Key + "}}", item.Value.ToString()));
        }

        /// <summary>
        /// 以GZip算法压缩字符串，返回Base64编码字符
        /// </summary>
        /// <param name="rawString">原字符串</param>
        /// <returns>压缩后的Base64编码的字符串</returns>
        public static string GZipCompressString(this string rawString)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = Encoding.UTF8.GetBytes(rawString);
                byte[] zippedData = rawData.Compress();
                return Convert.ToBase64String(zippedData);
            }
        }

        /// <summary>
        /// 将压缩后的Base64字符串以GZip算法解压
        /// </summary>
        /// <param name="zippedString">压缩后的Base64字符串</param>
        /// <returns>原始未压缩字符串</returns>
        public static string GZipDecompressString(this string zippedString)
        {
            if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] zippedData = Convert.FromBase64String(zippedString);
                return Encoding.UTF8.GetString(zippedData.Decompress());
            }
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase = false) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// 获取掩码字符串
        /// </summary>
        /// <param name="value">输入字符串</param>
        /// <param name="pl">前缀明码长度</param>
        /// <param name="sl">后缀明码长度</param>
        /// <remarks>
        /// 默认输入字符串截取3段，中间掩码
        ///     设置前/后缀长度，前缀长度小于截取长度，以设置长度为准
        /// </remarks>
        /// <returns></returns>
        public static string GetMaskString(this string value, int? pl = null, int? sl = null)
        {
            if (string.IsNullOrEmpty(value)) return value;
            int l = value.Length;
            if (l / 3 == 0) return value;

            int p = l / 3;
            int s = l / 3 + (l % 3 >= 1 ? 1 : 0);

            if (pl.HasValue && p > pl.Value) p = pl.Value;
            if (sl.HasValue && s > sl.Value) s = sl.Value;

            string prefix = value.Substring(0, p);
            string suffix = value.Substring(l - s, s);

            var pp = "";
            for (var i = 0; i < l - p - s; i++)
            {
                pp += "*";
            }

            return prefix + pp + suffix;
        }

    }
}
