using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    /// <summary>
    /// 操作正则表达式的公共帮助类
    /// </summary>    
    public class RegexHelper
    {
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// 匹配默认正则
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="enumregex">EnumRegex</param>
        /// <returns></returns>
        public static bool IsMatch(string input, EnumRegex enumregex)
        {
            return IsMatch(input, EnumHelper.GetDescription(enumregex));
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="enumregex">EnumRegex</param>
        /// <param name="option">筛选条件</param>
        /// <returns></returns>
        public static bool IsMatch(string input, EnumRegex enumregex, RegexOptions option)
        {
            return Regex.IsMatch(input, EnumHelper.GetDescription(enumregex), option);
        }
    }

    /// <summary>
    /// 枚举类型
    /// </summary>
    public enum EnumRegex
    {
        #region 邮箱
        /// <summary>
        /// 邮箱
        /// </summary>
        [Description(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        Email,
        #endregion

        #region Url地址
        /// <summary>
        /// Url地址
        /// </summary>
        [Description(@"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?")]
        UrlAddress,
        #endregion

        #region 整数
        /// <summary>
        /// 整数
        /// </summary>
        [Description(@"^-?\d+$")]
        Integer,
        #endregion

        #region 汉字
        /// <summary>
        /// 汉子
        /// </summary>
        [Description(@"^[\u4e00-\u9fa5]{0,}$")]
        汉字,
        #endregion

        #region 身份证号
        /// <summary>
        /// 身份证号
        /// </summary>
        [Description(@"^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$")]
        身份证号,
        #endregion

        #region 手机号码
        /// <summary>
        /// 手机号码
        /// </summary>
        [Description(@"^(13[0-9]|15[0-9]|17[0-9]|18[0-9]|14[0-9]|16[0-9]|19[89])[0-9]{8}$")]
        手机号码,
        #endregion

        #region Ip地址
        /// <summary>
        /// IP
        /// </summary>
        [Description(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$")]
        Ip地址,
        #endregion
    }
}
