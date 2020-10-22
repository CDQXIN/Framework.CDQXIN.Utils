using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 系统操作相关的公共类
	/// </summary>    
	public class SysHelper
	{
	
	/// <summary>
	/// 当前应用程序路径
	/// </summary>
	/// <returns></returns>
	public static string BaseDirectory
	{
		get
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}
	}
	/// <summary>
	/// 换行字符
	/// </summary>
	public static string NewLine
	{
		get
		{
			return Environment.NewLine;
		}
	}
	/// <summary>
	/// 当前应用程序域
	/// </summary>
	public static AppDomain CurrentAppDomain
	{
		get
		{
			return Thread.GetDomain();
		}
	}
	/// <summary>
	/// 本地使用的IP地址
	/// </summary>
	/// <returns></returns>
	//public static IPAddress LocalIpAddress
	//{
	//	get
	//	{
	//		IEnumerable<IPAddress> arg_2E_0 = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
	//		Func<IPAddress, bool> arg_2E_1;
	//		if ((arg_2E_1 = SysHelper.<> c.<> 9__9_0) == null)
	//		{
	//			arg_2E_1 = (SysHelper.<> c.<> 9__9_0 = new Func<IPAddress, bool>(SysHelper.<> c.<> 9.< get_LocalIpAddress > b__9_0));
	//		}
	//		return arg_2E_0.FirstOrDefault(arg_2E_1);
	//	}
	//}
	/// <summary>
	/// 获取文件相对路径映射的物理路径
	/// </summary>
	/// <param name="virtualPath">文件的相对路径</param>        
	public static string GetPath(string virtualPath)
	{
		return HttpContext.Current.Server.MapPath(virtualPath);
	}
	/// <summary>
	/// 获取指定调用层级的方法名
	/// </summary>
	/// <param name="level">调用的层数</param>        
	public static string GetMethodName(int level)
	{
		return new StackTrace().GetFrame(level).GetMethod().Name;
	}
}
}
