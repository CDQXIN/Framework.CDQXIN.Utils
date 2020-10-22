using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils.ExtensionHelper
{
	/// <summary>
	/// 时间操作相关类
	/// </summary>
	public class TimeHelper
	{
		/// <summary>
		/// 返回每月的第一天和最后一天
		/// </summary>
		/// <param name="month">月份</param>
		/// <param name="firstDay">第一天</param>
		/// <param name="lastDay">最后一天</param>
		public static void ReturnDateFormat(int month, out string firstDay, out string lastDay)
		{
			DateTime now = DateTime.Now;
			int num = now.Year + month / 12;
			if (month != 12)
			{
				month %= 12;
			}
			switch (month)
			{
				case 1:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-31"
						}));
						return;
					}
				case 2:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						if (DateTime.IsLeapYear(now.Year))
						{
							now = DateTime.Now;
							lastDay = now.ToString(string.Concat(new object[]
							{
							num,
							"-0",
							month,
							"-29"
							}));
							return;
						}
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-28"
						}));
						return;
					}
				case 3:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString("yyyy-0" + month + "-31");
						return;
					}
				case 4:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-30"
						}));
						return;
					}
				case 5:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-31"
						}));
						return;
					}
				case 6:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-30"
						}));
						return;
					}
				case 7:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-31"
						}));
						return;
					}
				case 8:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-31"
						}));
						return;
					}
				case 9:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-0",
						month,
						"-30"
						}));
						return;
					}
				case 10:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-",
						month,
						"-31"
						}));
						return;
					}
				case 11:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-",
						month,
						"-30"
						}));
						return;
					}
				default:
					{
						now = DateTime.Now;
						firstDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-",
						month,
						"-01"
						}));
						now = DateTime.Now;
						lastDay = now.ToString(string.Concat(new object[]
						{
						num,
						"-",
						month,
						"-31"
						}));
						return;
					}
			}
		}
		/// <summary>
		/// 把秒转换成分钟
		/// </summary>
		/// <param name="second">秒数</param>
		/// <returns></returns>
		public static int SecondToMinute(int second)
		{
			return Convert.ToInt32(Math.Ceiling(second / 60m));
		}
		/// <summary>
		/// 返回某年某月最后一天
		/// </summary>
		/// <param name="year">年份</param>
		/// <param name="month">月份</param>
		/// <returns>日</returns>
		public static int GetMonthLastDate(int year, int month)
		{
			DateTime dateTime = new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
			return dateTime.Day;
		}
		/// <summary>
		/// 日期比较
		/// </summary>
		/// <param name="dateTime1">开始时间</param>
		/// <param name="dateTime2">结束时间</param>
		/// <returns></returns>
		public static string DateDiff(DateTime dateTime1, DateTime dateTime2)
		{
			string result = null;
			try
			{
				TimeSpan timeSpan = dateTime2 - dateTime1;
				if (timeSpan.Days >= 1)
				{
					int num = dateTime1.Month;
					string arg_3C_0 = num.ToString();
					string arg_3C_1 = "月";
					num = dateTime1.Day;
					result = arg_3C_0 + arg_3C_1 + num.ToString() + "日";
				}
				else
				{
					if (timeSpan.Hours > 1)
					{
						int num = timeSpan.Hours;
						result = num.ToString() + "小时前";
					}
					else
					{
						int num = timeSpan.Minutes;
						result = num.ToString() + "分钟前";
					}
				}
			}
			catch
			{
			}
			return result;
		}
		/// <summary>
		/// 获得两个日期的间隔
		/// </summary>
		/// <param name="dateTime1">日期一。</param>
		/// <param name="dateTime2">日期二。</param>
		/// <returns>日期间隔TimeSpan。</returns>
		public static TimeSpan DateDiff2(DateTime dateTime1, DateTime dateTime2)
		{
			TimeSpan timeSpan = new TimeSpan(dateTime1.Ticks);
			TimeSpan ts = new TimeSpan(dateTime2.Ticks);
			return timeSpan.Subtract(ts).Duration();
		}
		/// <summary>
		/// 得到随机日期
		/// </summary>
		/// <param name="datetime1">datetime1</param>
		/// <param name="datetime2">datetime2</param>
		/// <returns></returns>
		public static DateTime GetRandomTime(DateTime datetime1, DateTime datetime2)
		{
			DateTime dateTime = (datetime1 > datetime2) ? datetime2 : datetime1;
			TimeSpan timeSpan = new TimeSpan(((datetime1 > datetime2) ? datetime1 : datetime2).Ticks - dateTime.Ticks);
			int num = RandomHelper.NextInt(0, (int)timeSpan.TotalSeconds);
			return dateTime.AddSeconds((double)num);
		}
	}
}
