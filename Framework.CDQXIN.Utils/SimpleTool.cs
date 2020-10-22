using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 简单工具
	/// </summary>
	public class SimpleTool
	{
		/// <summary>
		/// 获取执行时间
		/// </summary>
		/// <param name="doHandle"></param>
		/// <returns></returns>
		public static long GetExcuteTime(Action doHandle)
		{
			Stopwatch expr_05 = new Stopwatch();
			expr_05.Start();
			doHandle();
			expr_05.Stop();
			return expr_05.ElapsedMilliseconds;
		}
	}
}
