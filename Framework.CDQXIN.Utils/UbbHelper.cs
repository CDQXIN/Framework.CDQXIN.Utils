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
	/// UBB代码助手类
	/// </summary>
	public class UbbHelper
	{
		private static readonly RegexOptions options = RegexOptions.Compiled | RegexOptions.Singleline;
		/// <summary>
		/// 解析Ubb代码为Html代码
		/// </summary>
		/// <param name="ubbCode">Ubb代码</param>
		/// <param name="withNoFollow"></param>
		/// <returns>解析得到的Html代码</returns>
		public static string Decode(string ubbCode, bool withNoFollow = true)
		{
			if (string.IsNullOrEmpty(ubbCode))
			{
				return string.Empty;
			}
			string ubb = HttpUtility.HtmlEncode(ubbCode);
			ubb = UbbHelper.DecodeStyle(ubb);
			ubb = UbbHelper.DecodeFont(ubb);
			ubb = UbbHelper.DecodeColor(ubb);
			ubb = UbbHelper.DecodeImage(ubb);
			ubb = (withNoFollow ? UbbHelper.DecodeLinks(ubb) : UbbHelper.DecodeLinksWithNoFollow(ubb));
			ubb = UbbHelper.DecodeQuote(ubb);
			ubb = UbbHelper.DecodeAlign(ubb);
			ubb = UbbHelper.DecodeList(ubb);
			ubb = UbbHelper.DecodeHeading(ubb);
			return UbbHelper.DecodeBlank(ubb);
		}
		private static string DecodeHeading(string ubb)
		{
			return Regex.Replace(ubb, "\\[h(\\d)\\](.*?)\\[/h\\1\\]", "<h$1>$2</h$1>", UbbHelper.options);
		}
		private static string DecodeList(string ubb)
		{
			string format = "<ol style=\"list-style:{0};\">$1</ol>";
			return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(ubb, "\\[\\*\\]([^\\[]*)", "<li>$1</li>", UbbHelper.options), "\\[list\\]\\s*(.*?)\\[/list\\]", "<ul>$1</ul>", UbbHelper.options), "\\[list=1\\]\\s*(.*?)\\[/list\\]", string.Format(format, "decimal"), UbbHelper.options), "\\[list=i\\]\\s*(.*?)\\[/list\\]", string.Format(format, "lower-roman"), UbbHelper.options), "\\[list=I\\]\\s*(.*?)\\[/list\\]", string.Format(format, "upper-roman"), UbbHelper.options), "\\[list=a\\]\\s*(.*?)\\[/list\\]", string.Format(format, "lower-alpha"), UbbHelper.options), "\\[list=A\\]\\s*(.*?)\\[/list\\]", string.Format(format, "upper-alpha"), UbbHelper.options);
		}
		private static string DecodeBlank(string ubb)
		{
			string text = ubb;
			text = Regex.Replace(text, "(?<= ) | (?= )", " ", UbbHelper.options);
			text = Regex.Replace(text, "\\r\\n", "<br />");
			string[] array = new string[]
			{
				"h[1-6]",
				"li",
				"list",
				"div",
				"p",
				"ul"
			};
			for (int i = 0; i < array.Length; i++)
			{
				string str = array[i];
				text = new Regex("<br />(<" + str + ")", UbbHelper.options).Replace(text, "$1");
				text = new Regex("<br />(</" + str + ")", UbbHelper.options).Replace(text, "$1");
			}
			return text;
		}
		private static string DecodeAlign(string ubb)
		{
			return Regex.Replace(Regex.Replace(Regex.Replace(ubb, "\\[left\\](.*?)\\[/left\\]", "<div style=\"text-align:left\">$1</div>", UbbHelper.options), "\\[right\\](.*?)\\[/right\\]", "<div style=\"text-align:right\">$1</div>", UbbHelper.options), "\\[center\\](.*?)\\[/center\\]", "<div style=\"text-align:center\">$1</div>", UbbHelper.options);
		}
		private static string DecodeQuote(string ubb)
		{
			return Regex.Replace(Regex.Replace(ubb, "\\[quote\\]", "<blockquote><div>", UbbHelper.options), "\\[/quote\\]", "</div></blockquote>", UbbHelper.options);
		}
		private static string DecodeFont(string ubb)
		{
			return Regex.Replace(Regex.Replace(ubb, "\\[size=([-\\d]+).*?\\](.*?)\\[/size\\]", "<span style=\"font-size:$1%\">$2</span>", UbbHelper.options), "\\[font=(.*?)\\](.*?)\\[/font\\]", "<span style=\"font-family:$1\">$2</span>", UbbHelper.options);
		}
		private static string DecodeLinks(string ubb)
		{
			return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(ubb, "\\[url\\]www\\.(.*?)\\[/url\\]", "<a href=\"http://www.$1\">$1</a>", UbbHelper.options), "\\[url\\](.*?)\\[/url\\]", "<a href=\"$1\">$1</a>", UbbHelper.options), "\\[url=(.*?)\\](.*?)\\[/url\\]", "<a href=\"$1\" title=\"$2\">$2</a>", UbbHelper.options), "\\[email\\](.*?)\\[/email\\]", "<a href=\"mailto:$1\">$1</a>", UbbHelper.options);
		}
		private static string DecodeLinksWithNoFollow(string ubb)
		{
			return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(ubb, "\\[url\\]www\\.(.*?)\\[/url\\]", "<a rel=\"nofollow\" href=\"http://www.$1\">$1</a>", UbbHelper.options), "\\[url\\](.*?)\\[/url\\]", "<a rel=\"nofollow\" href=\"$1\">$1</a>", UbbHelper.options), "\\[url=(.*?)\\](.*?)\\[/url\\]", "<a rel=\"nofollow\" href=\"$1\" title=\"$2\">$2</a>", UbbHelper.options), "\\[email\\](.*?)\\[/email\\]", "<a href=\"mailto:$1\">$1</a>", UbbHelper.options);
		}
		private static string DecodeImage(string ubb)
		{
			return Regex.Replace(Regex.Replace(Regex.Replace(ubb, "\\[hr\\]", "<hr />", UbbHelper.options), "\\[img\\](.+?)\\[/img\\]", "<img src=\"$1\" alt=\"\" />", UbbHelper.options), "\\[img=(\\d+)x(\\d+)\\](.+?)\\[/img\\]", "<img src=\"$3\" style=\"width:$1px;height:$2px\" alt=\"\" />", UbbHelper.options);
		}
		private static string DecodeColor(string ubb)
		{
			return Regex.Replace(ubb, "\\[color=(#?\\w+?)\\](.+?)\\[/color\\]", "<span style=\"color:$1\">$2</span>", UbbHelper.options);
		}
		private static string DecodeStyle(string ubb)
		{
			return Regex.Replace(Regex.Replace(Regex.Replace(ubb, "\\[[b]\\](.*?)\\[/[b]\\]", "<strong>$1</strong>", UbbHelper.options), "\\[[u]\\](.*?)\\[/[u]\\]", "<span style=\"text-decoration:underline\">$1</span>", UbbHelper.options), "\\[[i]\\](.*?)\\[/[i]\\]", "<i>$1</i>", UbbHelper.options);
		}
	}
}
