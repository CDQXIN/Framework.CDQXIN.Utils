using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 静态资源帮助类
	/// </summary>
	public class WebResourceHelper
	{
		/// <summary>
		/// 资源路径
		/// </summary>
		/// <param name="resourcetype">资源类型</param>
		/// <param name="filepath">文件路径</param>
		/// <param name="isVersion">版本号</param>
		/// <remarks>
		/// 务必配置
		///     资源域名[WebResourceNodeName]
		///     节点名称[WebResourceDomain]
		///     版本号[VersionNo]
		/// </remarks>
		/// <returns></returns>
		public static string Url(string filepath, ResourceType resourcetype = ResourceType.This, bool isVersion = true)
		{
			string arg = (resourcetype == ResourceType.Public) ? "public" : ConfigHelper_v1.GetString("WebResourceNodeName");
			string @string = ConfigHelper_v1.GetString("WebResourceDomain");
			string string2 = ConfigHelper_v1.GetString("WebResourceVersionNo");
			return string.Format("{0}/{1}{2}", @string, string.Format("{0}/{1}", arg, filepath), isVersion ? string.Format("?v={0}", string2) : "");
		}
	}
	/// <summary>
	/// 资源类型
	/// </summary>
	public enum ResourceType
	{
		/// <summary>
		/// 共有资源
		/// </summary>
		Public,
		/// <summary>
		/// 站点站点
		/// </summary>
		This
	}
}
