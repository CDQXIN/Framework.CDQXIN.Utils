using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 文件下载帮助类
	/// </summary>
	public class DownloadHelper
	{
		/// <summary>
		/// 参数为虚拟路径
		/// <param name="fileName"></param>
		/// </summary>
		public static string FileNameExtension(string fileName)
		{
			return Path.GetExtension(DownloadHelper.MapPathFile(fileName));
		}
		/// <summary>
		/// 获取物理地址
		/// <param name="fileName"></param>
		/// </summary>
		public static string MapPathFile(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			return HttpContext.Current.Server.MapPath(fileName);
		}
		/// <summary>
		/// 普通下载
		/// </summary>
		/// <param name="fileName">文件虚拟路径</param>
		public static void DownLoadold(string fileName)
		{
			string text = DownloadHelper.MapPathFile(fileName);
			if (File.Exists(text))
			{
				FileInfo fileInfo = new FileInfo(text);
				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.ClearHeaders();
				HttpContext.Current.Response.Buffer = false;
				HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(text), Encoding.UTF8));
				HttpContext.Current.Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
				HttpContext.Current.Response.ContentType = "application/octet-stream";
				HttpContext.Current.Response.WriteFile(text);
				HttpContext.Current.Response.Flush();
				HttpContext.Current.Response.End();
			}
		}
		/// <summary>
		/// 分块下载
		/// </summary>
		/// <param name="fileName">文件虚拟路径</param>
		public static void DownLoad(string fileName)
		{
			string path = DownloadHelper.MapPathFile(fileName);
			long num = 204800L;
			byte[] buffer = new byte[(int)((object)((IntPtr)num))];
			long num2 = 0L;
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				num2 = fileStream.Length;
				HttpContext.Current.Response.ContentType = "application/octet-stream";
				HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(Path.GetFileName(path)));
				HttpContext.Current.Response.AddHeader("Content-Length", num2.ToString());
				while (num2 > 0L)
				{
					if (HttpContext.Current.Response.IsClientConnected)
					{
						int num3 = fileStream.Read(buffer, 0, Convert.ToInt32(num));
						HttpContext.Current.Response.OutputStream.Write(buffer, 0, num3);
						HttpContext.Current.Response.Flush();
						HttpContext.Current.Response.Clear();
						num2 -= (long)num3;
					}
					else
					{
						num2 = -1L;
					}
				}
			}
			catch (Exception ex)
			{
				HttpContext.Current.Response.Write("Error:" + ex.Message);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				HttpContext.Current.Response.Close();
			}
		}
		/// <summary>
		///  输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
		/// </summary>
		/// <param name="request">Page.Request对象</param>
		/// <param name="response">Page.Response对象</param>
		/// <param name="fileName">下载文件名</param>
		/// <param name="fullPath">带文件名下载路径</param>
		/// <param name="speed">每秒允许下载的字节数</param>
		/// <returns>返回是否成功</returns>
		public static bool ResponseFile(HttpRequest request, HttpResponse response, string fileName, string fullPath, long speed)
		{
			try
			{
				FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				try
				{
					response.AddHeader("Accept-Ranges", "bytes");
					response.Buffer = false;
					long length = fileStream.Length;
					long num = 0L;
					int num2 = 10240;
					int millisecondsTimeout = (int)Math.Floor((double)((long)(1000 * num2) / speed)) + 1;
					if (request.Headers["Range"] != null)
					{
						response.StatusCode = 206;
						num = Convert.ToInt64(request.Headers["Range"].Split(new char[]
						{
							'=',
							'-'
						})[1]);
					}
					response.AddHeader("Content-Length", (length - num).ToString());
					if (num != 0L)
					{
						response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", num, length - 1L, length));
					}
					response.AddHeader("Connection", "Keep-Alive");
					response.ContentType = "application/octet-stream";
					response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
					binaryReader.BaseStream.Seek(num, SeekOrigin.Begin);
					int num3 = (int)Math.Floor((double)((length - num) / (long)num2)) + 1;
					for (int i = 0; i < num3; i++)
					{
						if (response.IsClientConnected)
						{
							response.BinaryWrite(binaryReader.ReadBytes(num2));
							Thread.Sleep(millisecondsTimeout);
						}
						else
						{
							i = num3;
						}
					}
				}
				catch
				{
					bool result = false;
					return result;
				}
				finally
				{
					binaryReader.Close();
					fileStream.Close();
				}
			}
			catch
			{
				bool result = false;
				return result;
			}
			return true;
		}
	}
}
