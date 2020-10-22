using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 中国日历帮助类
	/// </summary>
	public class ChineseCalendarHelper
	{
		/// <summary>
		/// 阳历
		/// </summary>
		private struct SolarHolidayStruct
		{
			public readonly int Month;
			public readonly int Day;
			private int _recess;
			public readonly string HolidayName;
			public SolarHolidayStruct(int month, int day, int recess, string name)
			{
				this.Month = month;
				this.Day = day;
				this._recess = recess;
				this.HolidayName = name;
			}
		}
		/// <summary>
		/// 农历
		/// </summary>
		private struct LunarHolidayStruct
		{
			public readonly int Month;
			public int Day;
			private int Recess;
			public string HolidayName;
			public LunarHolidayStruct(int month, int day, int recess, string name)
			{
				this.Month = month;
				this.Day = day;
				this.Recess = recess;
				this.HolidayName = name;
			}
		}
		private struct WeekHolidayStruct
		{
			public int Month;
			public int WeekAtMonth;
			public int WeekDay;
			public string HolidayName;
			public WeekHolidayStruct(int month, int weekAtMonth, int weekDay, string name)
			{
				this.Month = month;
				this.WeekAtMonth = weekAtMonth;
				this.WeekDay = weekDay;
				this.HolidayName = name;
			}
		}
		private DateTime _date;
		private readonly DateTime _datetime;
		private readonly int _cYear;
		private readonly int _cMonth;
		private readonly int _cDay;
		private readonly bool _cIsLeapMonth;
		private readonly bool _cIsLeapYear;
		private const int MinYear = 1900;
		private const int MaxYear = 2050;
		private static DateTime MinDay = new DateTime(1900, 1, 30);
		private static DateTime MaxDay = new DateTime(2049, 12, 31);
		private const int GanZhiStartYear = 1864;
		private static DateTime GanZhiStartDay = new DateTime(1899, 12, 22);
		private const string HZNum = "零一二三四五六七八九";
		private const int AnimalStartYear = 1900;
		private static DateTime ChineseConstellationReferDay = new DateTime(2007, 9, 13);
		/// <summary>
		/// 来源于网上的农历数据
		/// </summary>
		/// <remarks>
		/// 数据结构如下，共使用17位数据
		/// 第17位：表示闰月天数，0表示29天   1表示30天
		/// 第16位-第5位（共12位）表示12个月，其中第16位表示第一月，如果该月为30天则为1，29天为0
		/// 第4位-第1位（共4位）表示闰月是哪个月，如果当年没有闰月，则置0
		///             </remarks>
		private static int[] LunarDateArray = new int[]
		{
			19416,
			19168,
			42352,
			21717,
			53856,
			55632,
			91476,
			22176,
			39632,
			21970,
			19168,
			42422,
			42192,
			53840,
			119381,
			46400,
			54944,
			44450,
			38320,
			84343,
			18800,
			42160,
			46261,
			27216,
			27968,
			109396,
			11104,
			38256,
			21234,
			18800,
			25958,
			54432,
			59984,
			28309,
			23248,
			11104,
			100067,
			37600,
			116951,
			51536,
			54432,
			120998,
			46416,
			22176,
			107956,
			9680,
			37584,
			53938,
			43344,
			46423,
			27808,
			46416,
			86869,
			19872,
			42416,
			83315,
			21168,
			43432,
			59728,
			27296,
			44710,
			43856,
			19296,
			43748,
			42352,
			21088,
			62051,
			55632,
			23383,
			22176,
			38608,
			19925,
			19152,
			42192,
			54484,
			53840,
			54616,
			46400,
			46752,
			103846,
			38320,
			18864,
			43380,
			42160,
			45690,
			27216,
			27968,
			44870,
			43872,
			38256,
			19189,
			18800,
			25776,
			29859,
			59984,
			27480,
			21952,
			43872,
			38613,
			37600,
			51552,
			55636,
			54432,
			55888,
			30034,
			22176,
			43959,
			9680,
			37584,
			51893,
			43344,
			46240,
			47780,
			44368,
			21977,
			19360,
			42416,
			86390,
			21168,
			43312,
			31060,
			27296,
			44368,
			23378,
			19296,
			42726,
			42208,
			53856,
			60005,
			54576,
			23200,
			30371,
			38608,
			19415,
			19152,
			42192,
			118966,
			53840,
			54560,
			56645,
			46496,
			22224,
			21938,
			18864,
			42359,
			42160,
			43600,
			111189,
			27936,
			44448,
			84835
		};
		private static string[] _constellationName = new string[]
		{
			"白羊座",
			"金牛座",
			"双子座",
			"巨蟹座",
			"狮子座",
			"处女座",
			"天秤座",
			"天蝎座",
			"射手座",
			"摩羯座",
			"水瓶座",
			"双鱼座"
		};
		private static string[] _lunarHolidayName = new string[]
		{
			"小寒",
			"大寒",
			"立春",
			"雨水",
			"惊蛰",
			"春分",
			"清明",
			"谷雨",
			"立夏",
			"小满",
			"芒种",
			"夏至",
			"小暑",
			"大暑",
			"立秋",
			"处暑",
			"白露",
			"秋分",
			"寒露",
			"霜降",
			"立冬",
			"小雪",
			"大雪",
			"冬至"
		};
		private static string[] _chineseConstellationName = new string[]
		{
			"角木蛟",
			"亢金龙",
			"女土蝠",
			"房日兔",
			"心月狐",
			"尾火虎",
			"箕水豹",
			"斗木獬",
			"牛金牛",
			"氐土貉",
			"虚日鼠",
			"危月燕",
			"室火猪",
			"壁水獝",
			"奎木狼",
			"娄金狗",
			"胃土彘",
			"昴日鸡",
			"毕月乌",
			"觜火猴",
			"参水猿",
			"井木犴",
			"鬼金羊",
			"柳土獐",
			"星日马",
			"张月鹿",
			"翼火蛇",
			"轸水蚓"
		};
		private static string[] SolarTerm = new string[]
		{
			"小寒",
			"大寒",
			"立春",
			"雨水",
			"惊蛰",
			"春分",
			"清明",
			"谷雨",
			"立夏",
			"小满",
			"芒种",
			"夏至",
			"小暑",
			"大暑",
			"立秋",
			"处暑",
			"白露",
			"秋分",
			"寒露",
			"霜降",
			"立冬",
			"小雪",
			"大雪",
			"冬至"
		};
		private static int[] sTermInfo = new int[]
		{
			0,
			21208,
			42467,
			63836,
			85337,
			107014,
			128867,
			150921,
			173149,
			195551,
			218072,
			240693,
			263343,
			285989,
			308563,
			331033,
			353350,
			375494,
			397447,
			419210,
			440795,
			462224,
			483532,
			504758
		};
		private static string ganStr = "甲乙丙丁戊己庚辛壬癸";
		private static string zhiStr = "子丑寅卯辰巳午未申酉戌亥";
		private static string animalStr = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
		private static string nStr1 = "日一二三四五六七八九";
		private static string nStr2 = "初十廿卅";
		private static string[] _monthString = new string[]
		{
			"出错",
			"正月",
			"二月",
			"三月",
			"四月",
			"五月",
			"六月",
			"七月",
			"八月",
			"九月",
			"十月",
			"十一月",
			"腊月"
		};
		private static ChineseCalendarHelper.SolarHolidayStruct[] sHolidayInfo = new ChineseCalendarHelper.SolarHolidayStruct[]
		{
			new ChineseCalendarHelper.SolarHolidayStruct(1, 1, 1, "元旦"),
			new ChineseCalendarHelper.SolarHolidayStruct(2, 2, 0, "世界湿地日"),
			new ChineseCalendarHelper.SolarHolidayStruct(2, 10, 0, "国际气象节"),
			new ChineseCalendarHelper.SolarHolidayStruct(2, 14, 0, "情人节"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 1, 0, "国际海豹日"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 5, 0, "学雷锋纪念日"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 8, 0, "妇女节"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 12, 0, "植树节 孙中山逝世纪念日"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 14, 0, "国际警察日"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 15, 0, "消费者权益日"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 17, 0, "中国国医节 国际航海日"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 21, 0, "世界森林日 消除种族歧视国际日 世界儿歌日"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 22, 0, "世界水日"),
			new ChineseCalendarHelper.SolarHolidayStruct(3, 24, 0, "世界防治结核病日"),
			new ChineseCalendarHelper.SolarHolidayStruct(4, 1, 0, "愚人节"),
			new ChineseCalendarHelper.SolarHolidayStruct(4, 7, 0, "世界卫生日"),
			new ChineseCalendarHelper.SolarHolidayStruct(4, 22, 0, "世界地球日"),
			new ChineseCalendarHelper.SolarHolidayStruct(5, 1, 1, "劳动节"),
			new ChineseCalendarHelper.SolarHolidayStruct(5, 2, 1, "劳动节假日"),
			new ChineseCalendarHelper.SolarHolidayStruct(5, 3, 1, "劳动节假日"),
			new ChineseCalendarHelper.SolarHolidayStruct(5, 4, 0, "青年节"),
			new ChineseCalendarHelper.SolarHolidayStruct(5, 8, 0, "世界红十字日"),
			new ChineseCalendarHelper.SolarHolidayStruct(5, 12, 0, "国际护士节"),
			new ChineseCalendarHelper.SolarHolidayStruct(5, 31, 0, "世界无烟日"),
			new ChineseCalendarHelper.SolarHolidayStruct(6, 1, 0, "国际儿童节"),
			new ChineseCalendarHelper.SolarHolidayStruct(6, 5, 0, "世界环境保护日"),
			new ChineseCalendarHelper.SolarHolidayStruct(6, 26, 0, "国际禁毒日"),
			new ChineseCalendarHelper.SolarHolidayStruct(7, 1, 0, "建党节 香港回归纪念 世界建筑日"),
			new ChineseCalendarHelper.SolarHolidayStruct(7, 11, 0, "世界人口日"),
			new ChineseCalendarHelper.SolarHolidayStruct(8, 1, 0, "建军节"),
			new ChineseCalendarHelper.SolarHolidayStruct(8, 8, 0, "中国男子节 父亲节"),
			new ChineseCalendarHelper.SolarHolidayStruct(8, 15, 0, "抗日战争胜利纪念"),
			new ChineseCalendarHelper.SolarHolidayStruct(9, 9, 0, "毛主席逝世纪念"),
			new ChineseCalendarHelper.SolarHolidayStruct(9, 10, 0, "教师节"),
			new ChineseCalendarHelper.SolarHolidayStruct(9, 18, 0, "九·一八事变纪念日"),
			new ChineseCalendarHelper.SolarHolidayStruct(9, 20, 0, "国际爱牙日"),
			new ChineseCalendarHelper.SolarHolidayStruct(9, 27, 0, "世界旅游日"),
			new ChineseCalendarHelper.SolarHolidayStruct(9, 28, 0, "孔子诞辰"),
			new ChineseCalendarHelper.SolarHolidayStruct(10, 1, 1, "国庆节 国际音乐日"),
			new ChineseCalendarHelper.SolarHolidayStruct(10, 2, 1, "国庆节假日"),
			new ChineseCalendarHelper.SolarHolidayStruct(10, 3, 1, "国庆节假日"),
			new ChineseCalendarHelper.SolarHolidayStruct(10, 6, 0, "老人节"),
			new ChineseCalendarHelper.SolarHolidayStruct(10, 24, 0, "联合国日"),
			new ChineseCalendarHelper.SolarHolidayStruct(11, 10, 0, "世界青年节"),
			new ChineseCalendarHelper.SolarHolidayStruct(11, 12, 0, "孙中山诞辰纪念"),
			new ChineseCalendarHelper.SolarHolidayStruct(12, 1, 0, "世界艾滋病日"),
			new ChineseCalendarHelper.SolarHolidayStruct(12, 3, 0, "世界残疾人日"),
			new ChineseCalendarHelper.SolarHolidayStruct(12, 20, 0, "澳门回归纪念"),
			new ChineseCalendarHelper.SolarHolidayStruct(12, 24, 0, "平安夜"),
			new ChineseCalendarHelper.SolarHolidayStruct(12, 25, 0, "圣诞节"),
			new ChineseCalendarHelper.SolarHolidayStruct(12, 26, 0, "毛主席诞辰纪念")
		};
		private static ChineseCalendarHelper.LunarHolidayStruct[] lHolidayInfo = new ChineseCalendarHelper.LunarHolidayStruct[]
		{
			new ChineseCalendarHelper.LunarHolidayStruct(1, 1, 1, "春节"),
			new ChineseCalendarHelper.LunarHolidayStruct(1, 15, 0, "元宵节"),
			new ChineseCalendarHelper.LunarHolidayStruct(5, 5, 0, "端午节"),
			new ChineseCalendarHelper.LunarHolidayStruct(7, 7, 0, "七夕情人节"),
			new ChineseCalendarHelper.LunarHolidayStruct(7, 15, 0, "中元节 盂兰盆节"),
			new ChineseCalendarHelper.LunarHolidayStruct(8, 15, 0, "中秋节"),
			new ChineseCalendarHelper.LunarHolidayStruct(9, 9, 0, "重阳节"),
			new ChineseCalendarHelper.LunarHolidayStruct(12, 8, 0, "腊八节"),
			new ChineseCalendarHelper.LunarHolidayStruct(12, 23, 0, "北方小年(扫房)"),
			new ChineseCalendarHelper.LunarHolidayStruct(12, 24, 0, "南方小年(掸尘)")
		};
		private static ChineseCalendarHelper.WeekHolidayStruct[] wHolidayInfo = new ChineseCalendarHelper.WeekHolidayStruct[]
		{
			new ChineseCalendarHelper.WeekHolidayStruct(5, 2, 1, "母亲节"),
			new ChineseCalendarHelper.WeekHolidayStruct(5, 3, 1, "全国助残日"),
			new ChineseCalendarHelper.WeekHolidayStruct(6, 3, 1, "父亲节"),
			new ChineseCalendarHelper.WeekHolidayStruct(9, 3, 3, "国际和平日"),
			new ChineseCalendarHelper.WeekHolidayStruct(9, 4, 1, "国际聋人节"),
			new ChineseCalendarHelper.WeekHolidayStruct(10, 1, 2, "国际住房日"),
			new ChineseCalendarHelper.WeekHolidayStruct(10, 1, 4, "国际减轻自然灾害日"),
			new ChineseCalendarHelper.WeekHolidayStruct(11, 4, 5, "感恩节")
		};
		/// <summary>
		/// 计算中国农历节日
		/// </summary>
		public string NewCalendarHoliday
		{
			get
			{
				string result = "";
				if (!this._cIsLeapMonth)
				{
					ChineseCalendarHelper.LunarHolidayStruct[] array = ChineseCalendarHelper.lHolidayInfo;
					for (int i = 0; i < array.Length; i++)
					{
						ChineseCalendarHelper.LunarHolidayStruct lunarHolidayStruct = array[i];
						if (lunarHolidayStruct.Month == this._cMonth && lunarHolidayStruct.Day == this._cDay)
						{
							result = lunarHolidayStruct.HolidayName;
							break;
						}
					}
					if (this._cMonth == 12)
					{
						int chineseMonthDays = this.GetChineseMonthDays(this._cYear, 12);
						if (this._cDay == chineseMonthDays)
						{
							result = "除夕";
						}
					}
				}
				return result;
			}
		}
		/// <summary>
		/// 按某月第几周第几日计算的节日
		/// </summary>
		public string WeekDayHoliday
		{
			get
			{
				string result = "";
				ChineseCalendarHelper.WeekHolidayStruct[] array = ChineseCalendarHelper.wHolidayInfo;
				for (int i = 0; i < array.Length; i++)
				{
					ChineseCalendarHelper.WeekHolidayStruct weekHolidayStruct = array[i];
					if (this.CompareWeekDayHoliday(this._date, weekHolidayStruct.Month, weekHolidayStruct.WeekAtMonth, weekHolidayStruct.WeekDay))
					{
						result = weekHolidayStruct.HolidayName;
						break;
					}
				}
				return result;
			}
		}
		/// <summary>
		/// 按公历日计算的节日
		/// </summary>
		public string DateHoliday
		{
			get
			{
				string result = "";
				ChineseCalendarHelper.SolarHolidayStruct[] array = ChineseCalendarHelper.sHolidayInfo;
				for (int i = 0; i < array.Length; i++)
				{
					ChineseCalendarHelper.SolarHolidayStruct solarHolidayStruct = array[i];
					if (solarHolidayStruct.Month == this._date.Month && solarHolidayStruct.Day == this._date.Day)
					{
						result = solarHolidayStruct.HolidayName;
						break;
					}
				}
				return result;
			}
		}
		/// <summary>
		/// 取对应的公历日期
		/// </summary>
		public DateTime Date
		{
			get
			{
				return this._date;
			}
			set
			{
				this._date = value;
			}
		}
		/// <summary>
		/// 取星期几
		/// </summary>
		public DayOfWeek WeekDay
		{
			get
			{
				return this._date.DayOfWeek;
			}
		}
		/// <summary>
		/// 周几的字符
		/// </summary>
		public string WeekDayStr
		{
			get
			{
				switch (this._date.DayOfWeek)
				{
					case DayOfWeek.Sunday:
						{
							return "星期日";
						}
					case DayOfWeek.Monday:
						{
							return "星期一";
						}
					case DayOfWeek.Tuesday:
						{
							return "星期二";
						}
					case DayOfWeek.Wednesday:
						{
							return "星期三";
						}
					case DayOfWeek.Thursday:
						{
							return "星期四";
						}
					case DayOfWeek.Friday:
						{
							return "星期五";
						}
					default:
						{
							return "星期六";
						}
				}
			}
		}
		/// <summary>
		/// 公历日期中文表示法 如一九九七年七月一日
		/// </summary>
		public string DateString
		{
			get
			{
				return "公元" + this._date.ToLongDateString();
			}
		}
		/// <summary>
		/// 当前是否公历闰年
		/// </summary>
		public bool IsLeapYear
		{
			get
			{
				return DateTime.IsLeapYear(this._date.Year);
			}
		}
		/// <summary>
		/// 28星宿计算
		/// </summary>
		public string ChineseConstellation
		{
			get
			{
				int num = (this._date - ChineseCalendarHelper.ChineseConstellationReferDay).Days % 28;
				if (num < 0)
				{
					return ChineseCalendarHelper._chineseConstellationName[27 + num];
				}
				return ChineseCalendarHelper._chineseConstellationName[num];
			}
		}
		/// <summary>
		/// 时辰
		/// </summary>
		public string ChineseHour
		{
			get
			{
				return this.GetChineseHour(this._datetime);
			}
		}
		/// <summary>
		/// 是否闰月
		/// </summary>
		public bool IsChineseLeapMonth
		{
			get
			{
				return this._cIsLeapMonth;
			}
		}
		/// <summary>
		/// 当年是否有闰月
		/// </summary>
		public bool IsChineseLeapYear
		{
			get
			{
				return this._cIsLeapYear;
			}
		}
		/// <summary>
		/// 农历日
		/// </summary>
		public int ChineseDay
		{
			get
			{
				return this._cDay;
			}
		}
		/// <summary>
		/// 农历日中文表示
		/// </summary>
		public string ChineseDayString
		{
			get
			{
				int cDay = this._cDay;
				if (cDay <= 10)
				{
					if (cDay == 0)
					{
						return "";
					}
					if (cDay == 10)
					{
						return "初十";
					}
				}
				else
				{
					if (cDay == 20)
					{
						return "二十";
					}
					if (cDay == 30)
					{
						return "三十";
					}
				}
				char c = ChineseCalendarHelper.nStr2[this._cDay / 10];
				string arg_70_0 = c.ToString();
				c = ChineseCalendarHelper.nStr1[this._cDay % 10];
				return arg_70_0 + c.ToString();
			}
		}
		/// <summary>
		/// 农历的月份
		/// </summary>
		public int ChineseMonth
		{
			get
			{
				return this._cMonth;
			}
		}
		/// <summary>
		/// 农历月份字符串
		/// </summary>
		public string ChineseMonthString
		{
			get
			{
				return ChineseCalendarHelper._monthString[this._cMonth];
			}
		}
		/// <summary>
		/// 取农历年份
		/// </summary>
		public int ChineseYear
		{
			get
			{
				return this._cYear;
			}
		}
		/// <summary>
		/// 取农历年字符串如，一九九七年
		/// </summary>
		public string ChineseYearString
		{
			get
			{
				string str = "";
				string text = this._cYear.ToString();
				for (int i = 0; i < 4; i++)
				{
					str += this.ConvertNumToChineseNum(text[i]);
				}
				return str + "年";
			}
		}
		/// <summary>
		/// 取农历日期表示法：农历一九九七年正月初五
		/// </summary>
		public string ChineseDateString
		{
			get
			{
				if (this._cIsLeapMonth)
				{
					return string.Concat(new string[]
					{
						"农历",
						this.ChineseYearString,
						"闰",
						this.ChineseMonthString,
						this.ChineseDayString
					});
				}
				return "农历" + this.ChineseYearString + this.ChineseMonthString + this.ChineseDayString;
			}
		}
		/// <summary>
		/// 定气法计算二十四节气,二十四节气是按地球公转来计算的，并非是阴历计算的
		/// </summary>
		/// <remarks>
		/// 节气的定法有两种。古代历法采用的称为"恒气"，即按时间把一年等分为24份，
		/// 每一节气平均得15天有余，所以又称"平气"。现代农历采用的称为"定气"，即
		/// 按地球在轨道上的位置为标准，一周360°，两节气之间相隔15°。由于冬至时地
		/// 球位于近日点附近，运动速度较快，因而太阳在黄道上移动15°的时间不到15天。
		/// 夏至前后的情况正好相反，太阳在黄道上移动较慢，一个节气达16天之多。采用
		/// 定气时可以保证春、秋两分必然在昼夜平分的那两天。
		/// </remarks>
		public string ChineseTwentyFourDay
		{
			get
			{
				DateTime dateTime = new DateTime(1900, 1, 6, 2, 5, 0);
				string result = "";
				int year = this._date.Year;
				for (int i = 1; i <= 24; i++)
				{
					double value = 525948.76 * (double)(year - 1900) + (double)ChineseCalendarHelper.sTermInfo[i - 1];
					if (dateTime.AddMinutes(value).DayOfYear == this._date.DayOfYear)
					{
						result = ChineseCalendarHelper.SolarTerm[i - 1];
						break;
					}
				}
				return result;
			}
		}
		/// <summary>
		/// 当前日期前一个最近节气
		/// </summary>
		public string ChineseTwentyFourPrevDay
		{
			get
			{
				DateTime dateTime = new DateTime(1900, 1, 6, 2, 5, 0);
				string result = "";
				int year = this._date.Year;
				for (int i = 24; i >= 1; i--)
				{
					double value = 525948.76 * (double)(year - 1900) + (double)ChineseCalendarHelper.sTermInfo[i - 1];
					DateTime dateTime2 = dateTime.AddMinutes(value);
					if (dateTime2.DayOfYear < this._date.DayOfYear)
					{
						result = string.Format("{0}[{1}]", ChineseCalendarHelper.SolarTerm[i - 1], dateTime2.ToString("yyyy-MM-dd"));
						break;
					}
				}
				return result;
			}
		}
		/// <summary>
		/// 当前日期后一个最近节气
		/// </summary>
		public string ChineseTwentyFourNextDay
		{
			get
			{
				DateTime dateTime = new DateTime(1900, 1, 6, 2, 5, 0);
				string result = "";
				int year = this._date.Year;
				for (int i = 1; i <= 24; i++)
				{
					double value = 525948.76 * (double)(year - 1900) + (double)ChineseCalendarHelper.sTermInfo[i - 1];
					DateTime dateTime2 = dateTime.AddMinutes(value);
					if (dateTime2.DayOfYear > this._date.DayOfYear)
					{
						result = string.Format("{0}[{1}]", ChineseCalendarHelper.SolarTerm[i - 1], dateTime2.ToString("yyyy-MM-dd"));
						break;
					}
				}
				return result;
			}
		}
		/// <summary>
		/// 计算指定日期的星座序号 
		/// </summary>
		public string Constellation
		{
			get
			{
				int num = 0;
				int num2 = this._date.Year;
				int arg_27_0 = this._date.Month;
				int day = this._date.Day;
				num2 = arg_27_0 * 100 + day;
				if (num2 >= 321 && num2 <= 419)
				{
					num = 0;
				}
				else
				{
					if (num2 >= 420 && num2 <= 520)
					{
						num = 1;
					}
					else
					{
						if (num2 >= 521 && num2 <= 620)
						{
							num = 2;
						}
						else
						{
							if (num2 >= 621 && num2 <= 722)
							{
								num = 3;
							}
							else
							{
								if (num2 >= 723 && num2 <= 822)
								{
									num = 4;
								}
								else
								{
									if (num2 >= 823 && num2 <= 922)
									{
										num = 5;
									}
									else
									{
										if (num2 >= 923 && num2 <= 1022)
										{
											num = 6;
										}
										else
										{
											if (num2 >= 1023 && num2 <= 1121)
											{
												num = 7;
											}
											else
											{
												if (num2 >= 1122 && num2 <= 1221)
												{
													num = 8;
												}
												else
												{
													if (num2 >= 1222 || num2 <= 119)
													{
														num = 9;
													}
													else
													{
														if (num2 >= 120 && num2 <= 218)
														{
															num = 10;
														}
														else
														{
															if (num2 >= 219 && num2 <= 320)
															{
																num = 11;
															}
															else
															{
																num = 0;
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				return ChineseCalendarHelper._constellationName[num];
			}
		}
		/// <summary>
		/// 计算属相的索引，注意虽然属相是以农历年来区别的，但是目前在实际使用中是按公历来计算的 鼠年为1,其它类推
		/// </summary>
		public int Animal
		{
			get
			{
				return (this._date.Year - 1900) % 12 + 1;
			}
		}
		/// <summary>
		/// 取属相字符串
		/// </summary>
		public string AnimalString
		{
			get
			{
				int num = this._date.Year - 1900;
				return ChineseCalendarHelper.animalStr[num % 12].ToString();
			}
		}
		/// <summary>
		/// 取农历年的干支表示法如 乙丑年
		/// </summary>
		public string GanZhiYearString
		{
			get
			{
				int num = (this._cYear - 1864) % 60;
				char c = ChineseCalendarHelper.ganStr[num % 10];
				string arg_41_0 = c.ToString();
				c = ChineseCalendarHelper.zhiStr[num % 12];
				return arg_41_0 + c.ToString() + "年";
			}
		}
		/// <summary>
		/// 取干支的月表示字符串，注意农历的闰月不记干支
		/// </summary>
		public string GanZhiMonthString
		{
			get
			{
				int num;
				if (this._cMonth > 10)
				{
					num = this._cMonth - 10;
				}
				else
				{
					num = this._cMonth + 2;
				}
				char c = ChineseCalendarHelper.zhiStr[num - 1];
				string str = c.ToString();
				int num2 = 1;
				switch ((this._cYear - 1864) % 60 % 10)
				{
					case 0:
						{
							num2 = 3;
							break;
						}
					case 1:
						{
							num2 = 5;
							break;
						}
					case 2:
						{
							num2 = 7;
							break;
						}
					case 3:
						{
							num2 = 9;
							break;
						}
					case 4:
						{
							num2 = 1;
							break;
						}
					case 5:
						{
							num2 = 3;
							break;
						}
					case 6:
						{
							num2 = 5;
							break;
						}
					case 7:
						{
							num2 = 7;
							break;
						}
					case 8:
						{
							num2 = 9;
							break;
						}
					case 9:
						{
							num2 = 1;
							break;
						}
				}
				c = ChineseCalendarHelper.ganStr[(num2 + this._cMonth - 2) % 10];
				return c.ToString() + str + "月";
			}
		}
		/// <summary>
		/// 取干支日表示法
		/// </summary>
		public string GanZhiDayString
		{
			get
			{
				int num = (this._date - ChineseCalendarHelper.GanZhiStartDay).Days % 60;
				char c = ChineseCalendarHelper.ganStr[num % 10];
				string arg_4D_0 = c.ToString();
				c = ChineseCalendarHelper.zhiStr[num % 12];
				return arg_4D_0 + c.ToString() + "日";
			}
		}
		/// <summary>
		/// 取当前日期的干支表示法如 甲子年乙丑月丙庚日
		/// </summary>
		public string GanZhiDateString
		{
			get
			{
				return this.GanZhiYearString + this.GanZhiMonthString + this.GanZhiDayString;
			}
		}
		/// <summary>
		/// 用一个标准的公历日期来初使化
		/// </summary>
		public ChineseCalendarHelper(DateTime dt)
		{
			this.CheckDateLimit(dt);
			this._date = dt.Date;
			this._datetime = dt;
			int num = 0;
			int num2 = (this._date - ChineseCalendarHelper.MinDay).Days;
			int i;
			for (i = 1900; i <= 2050; i++)
			{
				num = this.GetChineseYearDays(i);
				if (num2 - num < 1)
				{
					break;
				}
				num2 -= num;
			}
			this._cYear = i;
			int chineseLeapMonth = this.GetChineseLeapMonth(this._cYear);
			if (chineseLeapMonth > 0)
			{
				this._cIsLeapYear = true;
			}
			else
			{
				this._cIsLeapYear = false;
			}
			this._cIsLeapMonth = false;
			for (i = 1; i <= 12; i++)
			{
				if (chineseLeapMonth > 0 && i == chineseLeapMonth + 1 && !this._cIsLeapMonth)
				{
					this._cIsLeapMonth = true;
					i--;
					num = this.GetChineseLeapMonthDays(this._cYear);
				}
				else
				{
					this._cIsLeapMonth = false;
					num = this.GetChineseMonthDays(this._cYear, i);
				}
				num2 -= num;
				if (num2 <= 0)
				{
					break;
				}
			}
			num2 += num;
			this._cMonth = i;
			this._cDay = num2;
		}
		/// <summary>
		/// 用农历的日期来初使化
		/// </summary>
		/// <param name="cy">农历年</param>
		/// <param name="cm">农历月</param>
		/// <param name="cd">农历日</param>
		/// <param name="leapMonthFlag">闰月标志</param>
		public ChineseCalendarHelper(int cy, int cm, int cd, bool leapMonthFlag)
		{
			this.CheckChineseDateLimit(cy, cm, cd, leapMonthFlag);
			this._cYear = cy;
			this._cMonth = cm;
			this._cDay = cd;
			int num = 0;
			for (int i = 1900; i < cy; i++)
			{
				int num2 = this.GetChineseYearDays(i);
				num += num2;
			}
			int chineseLeapMonth = this.GetChineseLeapMonth(cy);
			if (chineseLeapMonth != 0)
			{
				this._cIsLeapYear = true;
			}
			else
			{
				this._cIsLeapYear = false;
			}
			if (cm != chineseLeapMonth)
			{
				this._cIsLeapMonth = false;
			}
			else
			{
				this._cIsLeapMonth = leapMonthFlag;
			}
			if (!this._cIsLeapYear || cm < chineseLeapMonth)
			{
				for (int i = 1; i < cm; i++)
				{
					int num2 = this.GetChineseMonthDays(cy, i);
					num += num2;
				}
				if (cd > this.GetChineseMonthDays(cy, cm))
				{
					throw new Exception("不合法的农历日期");
				}
				num += cd;
			}
			else
			{
				for (int i = 1; i < cm; i++)
				{
					int num2 = this.GetChineseMonthDays(cy, i);
					num += num2;
				}
				if (cm > chineseLeapMonth)
				{
					int num2 = this.GetChineseLeapMonthDays(cy);
					num += num2;
					if (cd > this.GetChineseMonthDays(cy, cm))
					{
						throw new Exception("不合法的农历日期");
					}
					num += cd;
				}
				else
				{
					if (this._cIsLeapMonth)
					{
						int num2 = this.GetChineseMonthDays(cy, cm);
						num += num2;
					}
					if (cd > this.GetChineseLeapMonthDays(cy))
					{
						throw new Exception("不合法的农历日期");
					}
					num += cd;
				}
			}
			this._date = ChineseCalendarHelper.MinDay.AddDays((double)num);
		}
		/// <summary>
		/// 传回农历y年m月的总天数
		/// </summary>
		private int GetChineseMonthDays(int year, int month)
		{
			if (this.BitTest32(ChineseCalendarHelper.LunarDateArray[year - 1900] & 65535, 16 - month))
			{
				return 30;
			}
			return 29;
		}
		/// <summary>
		/// 传回农历 y年闰哪个月 1-12 , 没闰传回 0
		/// </summary>
		private int GetChineseLeapMonth(int year)
		{
			return ChineseCalendarHelper.LunarDateArray[year - 1900] & 15;
		}
		/// <summary>
		/// 传回农历y年闰月的天数
		/// </summary>
		private int GetChineseLeapMonthDays(int year)
		{
			if (this.GetChineseLeapMonth(year) == 0)
			{
				return 0;
			}
			if ((ChineseCalendarHelper.LunarDateArray[year - 1900] & 65536) != 0)
			{
				return 30;
			}
			return 29;
		}
		/// <summary>
		/// 取农历年一年的天数
		/// </summary>
		private int GetChineseYearDays(int year)
		{
			int num = 348;
			int num2 = 32768;
			int num3 = ChineseCalendarHelper.LunarDateArray[year - 1900] & 65535;
			for (int i = 0; i < 12; i++)
			{
				if ((num3 & num2) != 0)
				{
					num++;
				}
				num2 >>= 1;
			}
			return num + this.GetChineseLeapMonthDays(year);
		}
		/// <summary>
		/// 获得当前时间的时辰
		/// </summary> 
		private string GetChineseHour(DateTime dt)
		{
			int num = dt.Hour;
			if (dt.Minute != 0)
			{
				num++;
			}
			int num2 = num / 2;
			if (num2 >= 12)
			{
				num2 = 0;
			}
			int num3 = (((this._date - ChineseCalendarHelper.GanZhiStartDay).Days % 60 % 10 + 1) * 2 - 1) % 10 - 1;
			char c = (ChineseCalendarHelper.ganStr.Substring(num3) + ChineseCalendarHelper.ganStr.Substring(0, num3 + 2))[num2];
			string arg_8B_0 = c.ToString();
			c = ChineseCalendarHelper.zhiStr[num2];
			return arg_8B_0 + c.ToString();
		}
		/// <summary>
		/// 检查公历日期是否符合要求
		/// </summary>
		private void CheckDateLimit(DateTime dt)
		{
			if (dt < ChineseCalendarHelper.MinDay || dt > ChineseCalendarHelper.MaxDay)
			{
				throw new Exception("超出可转换的日期");
			}
		}
		/// <summary>
		/// 检查农历日期是否合理
		/// </summary>
		private void CheckChineseDateLimit(int year, int month, int day, bool leapMonth)
		{
			if (year < 1900 || year > 2050)
			{
				throw new Exception("非法农历日期");
			}
			if (month < 1 || month > 12)
			{
				throw new Exception("非法农历日期");
			}
			if (day < 1 || day > 30)
			{
				throw new Exception("非法农历日期");
			}
			int chineseLeapMonth = this.GetChineseLeapMonth(year);
			if (leapMonth && month != chineseLeapMonth)
			{
				throw new Exception("非法农历日期");
			}
		}
		/// <summary>
		/// 将0-9转成汉字形式
		/// </summary>
		private string ConvertNumToChineseNum(char n)
		{
			if (n < '0' || n > '9')
			{
				return "";
			}
			switch (n)
			{
				case '0':
					{
						char c = "零一二三四五六七八九"[0];
						return c.ToString();
					}
				case '1':
					{
						char c = "零一二三四五六七八九"[1];
						return c.ToString();
					}
				case '2':
					{
						char c = "零一二三四五六七八九"[2];
						return c.ToString();
					}
				case '3':
					{
						char c = "零一二三四五六七八九"[3];
						return c.ToString();
					}
				case '4':
					{
						char c = "零一二三四五六七八九"[4];
						return c.ToString();
					}
				case '5':
					{
						char c = "零一二三四五六七八九"[5];
						return c.ToString();
					}
				case '6':
					{
						char c = "零一二三四五六七八九"[6];
						return c.ToString();
					}
				case '7':
					{
						char c = "零一二三四五六七八九"[7];
						return c.ToString();
					}
				case '8':
					{
						char c = "零一二三四五六七八九"[8];
						return c.ToString();
					}
				case '9':
					{
						char c = "零一二三四五六七八九"[9];
						return c.ToString();
					}
				default:
					{
						return "";
					}
			}
		}
		/// <summary>
		/// 测试某位是否为真
		/// </summary>
		private bool BitTest32(int num, int bitpostion)
		{
			if (bitpostion > 31 || bitpostion < 0)
			{
				throw new Exception("Error Param: bitpostion[0-31]:" + bitpostion.ToString());
			}
			int num2 = 1 << bitpostion;
			return (num & num2) != 0;
		}
		/// <summary>
		/// 将星期几转成数字表示
		/// </summary>
		private int ConvertDayOfWeek(DayOfWeek dayOfWeek)
		{
			switch (dayOfWeek)
			{
				case DayOfWeek.Sunday:
					{
						return 1;
					}
				case DayOfWeek.Monday:
					{
						return 2;
					}
				case DayOfWeek.Tuesday:
					{
						return 3;
					}
				case DayOfWeek.Wednesday:
					{
						return 4;
					}
				case DayOfWeek.Thursday:
					{
						return 5;
					}
				case DayOfWeek.Friday:
					{
						return 6;
					}
				case DayOfWeek.Saturday:
					{
						return 7;
					}
				default:
					{
						return 0;
					}
			}
		}
		/// <summary>
		/// 比较当天是不是指定的第周几
		/// </summary>
		private bool CompareWeekDayHoliday(DateTime date, int month, int week, int day)
		{
			bool result = false;
			if (date.Month == month && this.ConvertDayOfWeek(date.DayOfWeek) == day)
			{
				DateTime dateTime = new DateTime(date.Year, date.Month, 1);
				int arg_54_0 = this.ConvertDayOfWeek(dateTime.DayOfWeek);
				int num = 7 - this.ConvertDayOfWeek(dateTime.DayOfWeek) + 1;
				if (arg_54_0 > day)
				{
					if ((week - 1) * 7 + day + num == date.Day)
					{
						result = true;
					}
				}
				else
				{
					if (day + num + (week - 2) * 7 == date.Day)
					{
						result = true;
					}
				}
			}
			return result;
		}
	}
}
