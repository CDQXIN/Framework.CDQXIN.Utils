using Framework.CDQXIN.Utils.EncryptionHelper;
using Framework.CDQXIN.Utils.ExtensionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// Request Helper Class
	/// </summary>
	public class RequestHelper
	{
		/// <summary>
		/// 获取远程客户端的 IP 主机地址。
		/// </summary>
		/// <returns></returns>
		public static string UserHostAddress
		{
			get
			{
				return HttpContext.Current.Request.UserHostAddress;
			}
		}
		/// <summary>
		/// Request Client Ip Address
		/// </summary>
		public static string Ip
		{
			get
			{
				string result;
				if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
				{
					result = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
				}
				else
				{
					if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
					{
						result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
					}
					else
					{
						result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
					}
				}
				return result;
			}
		}
		/// <summary>
		/// 城市
		/// <remarks>
		/// 从http://www.ip138.com/ips1388.asp 获取Ip归属地
		/// </remarks>
		/// </summary>
		/// <returns></returns>
		public static string City
		{
			get
			{
				return RequestHelper.GetCityAdressByIp(RequestHelper.Ip);
			}
		}
		/// <summary>
		/// 浏览器信息
		/// </summary>
		/// <returns></returns>
		public static string BrowserInfo
		{
			get
			{
				return HttpContext.Current.Request.Browser.Capabilities[""].IsNullToString();
			}
		}
		/// <summary>
		/// 指纹
		/// <remarks>客户端Ip地址+浏览器信息，进行pbkdf</remarks>
		/// </summary>
		public static string Fingerprint
		{
			get
			{
				return Pbkdf2Security.CreateHash(RequestHelper.Ip + RequestHelper.BrowserInfo);
			}
		}
		/// <summary>
		/// HTTP 请求是否为 AJAX 请求
		/// </summary>
		public static bool IsAjaxRequest
		{
			get
			{
				if (HttpContext.Current.Request == null)
				{
					throw new ArgumentNullException("request");
				}
				return HttpContext.Current.Request["X-Requested-With"] == "XMLHttpRequest" || HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
			}
		}
		/// <summary>
		/// 获取IP地址的归属地<example>从http://www.ip138.com/ips1388.asp 获取Ip归属地</example>
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public static string GetCityAdressByIp(string ip)
		{
			if (!RegexHelper.IsMatch(ip, EnumRegex.Ip地址))
			{
				return ip;
			}
			string result;
			try
			{
				string input = HttpHelper.Get("http://www.ip138.com/ips1388.asp", string.Format("ip={0}&action=2", ip), Encoding.Default);
				result = new Regex("\"ul1\">([\\w\\W]*)</ul>").Match(input).Groups[1].ToString().Replace("<li>", "").Split(new string[]
				{
					"</li>"
				}, StringSplitOptions.RemoveEmptyEntries)[0].Replace("本站数据：", "");
			}
			catch
			{
				result = "未知地址";
			}
			return result;
		}
		/// <summary>
		/// 检查指纹
		/// </summary>
		/// <param name="fingerprint">指纹</param>
		/// <returns></returns>
		public static bool CheckFingerprint(string fingerprint)
		{
			return Pbkdf2Security.ValidatePassword(RequestHelper.Ip + RequestHelper.BrowserInfo, RequestHelper.Fingerprint);
		}
	}
}
