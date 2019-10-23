using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.ExtensionHelper
{
    /// <summary>
    /// DateTime 扩展
    /// </summary>
    public static class DateTimeExtHelper
    {
        /// <summary>
        /// 将DateTime时间换成中文
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        /// <example>
        /// 2012-12-21 12:12:21.012 → 1月前
        /// 2011-12-21 12:12:21.012 → 1年前
        /// </example>
        public static string ToChsStr(this DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;

            if (dateTime < DateTime.Now)
            {
                if ((int)ts.TotalDays >= 365)
                {
                    return (int)ts.TotalDays / 365 + "年前";
                }
                if ((int)ts.TotalDays >= 30 && ts.TotalDays <= 365)
                {
                    return (int)ts.TotalDays / 30 + "月前";
                }
                if ((int)ts.TotalDays == 1)
                {
                    return "昨天";
                }
                if ((int)ts.TotalDays == 2)
                {
                    return "前天";
                }
                if ((int)ts.TotalDays >= 3 && ts.TotalDays <= 30)
                {
                    return (int)ts.TotalDays + "天前";
                }
                if ((int)ts.TotalDays != 0)
                {
                    return dateTime.ToString("yyyy年MM月dd日");
                }
                if ((int)ts.TotalHours != 0)
                {
                    return (int)ts.TotalHours + "小时前";
                }
                if ((int)ts.TotalMinutes <= 0)
                {
                    return "刚刚";
                }
            }
            else
            {
                ts = dateTime - DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddSeconds(-1);

                if ((int)ts.Days >= 365)
                {
                    return (int)ts.TotalDays / 365 + "年后";
                }

                if ((int)ts.TotalDays >= 30 && ts.TotalDays <= 365)
                {
                    return (int)ts.TotalDays / 30 + "月后";
                }

                if ((int)ts.TotalDays > 2)
                {
                    return (int)ts.TotalDays + "天后";
                }

                if ((int)ts.TotalDays == 0)
                {
                    return "今天";
                }

                if ((int)ts.TotalDays > 0 && (int)ts.TotalDays <= 1)
                {
                    return "明天";
                }

                if ((int)ts.TotalDays > 1 && (int)ts.TotalDays <= 3)
                {
                    return "后天";
                }
            }


            return (int)ts.TotalMinutes + "分钟前";
        }

        /// <summary>
        /// 将DateTime时间类型转化为时间戳
        /// </summary>
        /// <param name="dateTime">DateTime 时间格式</param>
        /// <returns></returns>
        public static long ToUnix(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的当前时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的当前时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 将时间转化为字符串格式【yyyy-MM-dd】
        /// </summary>
        /// <param name="datetime">by DateTime</param>
        /// <returns></returns>
        public static string Toyyyy_MM_dd(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 将时间转化为字符串格式【yyyy-MM-dd HH:mm:ss】
        /// </summary>
        /// <param name="datetime">by DateTime</param>
        /// <returns></returns>
        public static string Toyyyy_MM_dd_HH_mm_ss(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取当天最小时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetDayMin(this DateTime dt)
        {
            return dt.Toyyyy_MM_dd().ToDateTime();
        }

        /// <summary>
        /// 获取当天最大时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetDayMax(this DateTime dt)
        {
            return dt.AddDays(1).Toyyyy_MM_dd().ToDateTime().AddMilliseconds(-1);
        }

        /// <summary>
        /// 得到本周第一天最小时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekDayMin(this DateTime datetime)
        {
            //星期一为第一天
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天
            string dt = datetime.AddDays(daydiff).Toyyyy_MM_dd();
            return dt.ToDateTime();
        }

        /// <summary>
        /// 得到本周第后一天天最大时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekDayMax(this DateTime datetime)
        {
            //星期天为最后一天
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天
            string dt = datetime.AddDays(daydiff + 1).ToString("yyyy-MM-dd");
            return dt.ToDateTime().AddMilliseconds(-1);
        }

        /// <summary>
        /// 获取当月最小时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthMin(this DateTime dt)
        {
            return DateTime.Parse(dt.ToString("yyyy-MM") + "-01 00:00:00");
        }

        /// <summary>
        /// 获取当月最大时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthMax(this DateTime dt)
        {
            return DateTime.Parse(dt.ToString("yyyy-MM") + "-01 00:00:00").AddMonths(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// 获取当年最小时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetYearMin(this DateTime dt)
        {
            return DateTime.Parse(dt.ToString("yyyy") + "-01-01 00:00:00");
        }

        /// <summary>
        /// 获取当年最大时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetYearMax(this DateTime dt)
        {
            return DateTime.Parse(dt.ToString("yyyy") + "-01-01 00:00:00").AddYears(1).AddMilliseconds(-1);
        }
    }
}
