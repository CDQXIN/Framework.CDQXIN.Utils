using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 视频资源帮助类
	/// </summary>
	public class VideoHelper : Page
	{
		private readonly string[] _strArrMencoder = new string[]
		{
			"wmv",
			"rmvb",
			"rm"
		};
		private readonly string[] _strArrFfmpeg = new string[]
		{
			"asf",
			"avi",
			"mpg",
			"3gp",
			"mov"
		};
		private static readonly string Ffmpegtool = ConfigHelper_v1.GetString("ffmpeg");
		private static readonly string Mencodertool = ConfigHelper_v1.GetString("mencoder");
		private static readonly string Savefile = ConfigHelper_v1.GetString("savefile") + "/";
		private static readonly string SizeOfImg = ConfigHelper_v1.GetString("CatchFlvImgSize");
		private static readonly string WidthOfFile = ConfigHelper_v1.GetString("widthSize");
		private static readonly string HeightOfFile = ConfigHelper_v1.GetString("heightSize");
		/// <summary>
		/// 获取文件的名字
		/// <param name="fileName">文件路径</param>
		/// </summary>
		public static string GetFileName(string fileName)
		{
			int startIndex = fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1;
			return fileName.Substring(startIndex);
		}
		/// <summary>
		/// 获取文件扩展名
		/// <param name="fileName"></param>
		/// </summary>
		public static string GetExtension(string fileName)
		{
			int startIndex = fileName.LastIndexOf(".", StringComparison.Ordinal) + 1;
			return fileName.Substring(startIndex);
		}
		/// <summary>
		/// 获取文件类型
		/// <param name="extension"></param>
		/// </summary>
		public string CheckExtension(string extension)
		{
			string text = "";
			if (this._strArrFfmpeg.Any((string var) => var == extension))
			{
				text = "ffmpeg";
			}
			if (text == "" && this._strArrMencoder.Any((string var) => var == extension))
			{
				text = "mencoder";
			}
			return text;
		}
		/// <summary>
		/// 视频格式转为Flv
		/// </summary>
		/// <param name="vFileName">原视频文件地址</param>
		/// <param name="exportName">生成后的Flv文件地址</param>
		public bool ConvertFlv(string vFileName, string exportName)
		{
			if (!File.Exists(VideoHelper.Ffmpegtool) || !File.Exists(HttpContext.Current.Server.MapPath(vFileName)))
			{
				return false;
			}
			vFileName = HttpContext.Current.Server.MapPath(vFileName);
			exportName = HttpContext.Current.Server.MapPath(exportName);
			string arguments = string.Concat(new string[]
			{
				" -i \"",
				vFileName,
				"\" -y -ab 32 -ar 22050 -b 800000 -s  480*360 \"",
				exportName,
				"\""
			});
			Process expr_7A = new Process();
			expr_7A.StartInfo.FileName = VideoHelper.Ffmpegtool;
			expr_7A.StartInfo.Arguments = arguments;
			expr_7A.StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath("~/tools/");
			expr_7A.StartInfo.UseShellExecute = false;
			expr_7A.StartInfo.RedirectStandardInput = true;
			expr_7A.StartInfo.RedirectStandardOutput = true;
			expr_7A.StartInfo.RedirectStandardError = true;
			expr_7A.StartInfo.CreateNoWindow = false;
			expr_7A.Start();
			expr_7A.BeginErrorReadLine();
			expr_7A.WaitForExit();
			expr_7A.Close();
			expr_7A.Dispose();
			return true;
		}
		/// <summary>
		/// 生成Flv视频的缩略图
		/// </summary>
		/// <param name="vFileName">视频文件地址</param>
		public string CatchImg(string vFileName)
		{
			if (!File.Exists(VideoHelper.Ffmpegtool) || !File.Exists(HttpContext.Current.Server.MapPath(vFileName)))
			{
				return "";
			}
			string result;
			try
			{
				string text = vFileName.Substring(0, vFileName.Length - 4) + ".jpg";
				string arguments = string.Concat(new string[]
				{
					" -i ",
					HttpContext.Current.Server.MapPath(vFileName),
					" -y -f image2 -t 0.1 -s ",
					VideoHelper.SizeOfImg,
					" ",
					HttpContext.Current.Server.MapPath(text)
				});
				Process process = new Process
				{
					StartInfo =
					{
						FileName = VideoHelper.Ffmpegtool,
						Arguments = arguments,
						WindowStyle = ProcessWindowStyle.Normal
					}
				};
				try
				{
					process.Start();
				}
				catch
				{
					result = "";
					return result;
				}
				finally
				{
					process.Close();
					process.Dispose();
				}
				Thread.Sleep(4000);
				if (File.Exists(HttpContext.Current.Server.MapPath(text)))
				{
					result = text;
				}
				else
				{
					result = "";
				}
			}
			catch
			{
				result = "";
			}
			return result;
		}
		/// <summary>
		/// 转换文件并保存在指定文件夹下
		/// </summary>
		/// <param name="fileName">上传视频文件的路径（原文件）</param>
		/// <param name="playFile">转换后的文件的路径（网络播放文件）</param>
		/// <param name="imgFile">从视频文件中抓取的图片路径</param>
		/// <returns>成功:返回图片虚拟地址;失败:返回空字符串</returns>
		public string ChangeFilePhy(string fileName, string playFile, string imgFile)
		{
			string text = base.Server.MapPath(VideoHelper.Ffmpegtool);
			if (!File.Exists(text) || !File.Exists(fileName))
			{
				return "";
			}
			string text2 = Path.ChangeExtension(playFile, ".flv");
			ProcessStartInfo startInfo = new ProcessStartInfo(text)
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = string.Concat(new string[]
				{
					" -i ",
					fileName,
					" -ab 56 -ar 22050 -b 500 -r 15 -s ",
					VideoHelper.WidthOfFile,
					"x",
					VideoHelper.HeightOfFile,
					" ",
					text2
				})
			};
			try
			{
				Process.Start(startInfo);
				this.CatchImg(fileName, imgFile);
			}
			catch
			{
				return "";
			}
			return "";
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="imgFile"></param>
		/// <returns></returns>
		public string CatchImg(string fileName, string imgFile)
		{
			string arg_22_0 = base.Server.MapPath(VideoHelper.Ffmpegtool);
			string text = imgFile + ".jpg";
			string sizeOfImg = VideoHelper.SizeOfImg;
			ProcessStartInfo startInfo = new ProcessStartInfo(arg_22_0)
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = string.Concat(new string[]
				{
					"   -i   ",
					fileName,
					"  -y  -f  image2   -ss 2 -vframes 1  -s   ",
					sizeOfImg,
					"   ",
					text
				})
			};
			try
			{
				Process.Start(startInfo);
			}
			catch
			{
				string result = "";
				return result;
			}
			if (!File.Exists(text))
			{
				return "";
			}
			return text;
		}
		/// <summary>
		/// 转换文件并保存在指定文件夹下
		/// </summary>
		/// <param name="fileName">上传视频文件的路径（原文件）</param>
		/// <param name="playFile">转换后的文件的路径（网络播放文件）</param>
		/// <param name="imgFile">从视频文件中抓取的图片路径</param>
		/// <returns>成功:返回图片虚拟地址;失败:返回空字符串</returns>
		public string ChangeFileVir(string fileName, string playFile, string imgFile)
		{
			string text = base.Server.MapPath(VideoHelper.Ffmpegtool);
			if (!File.Exists(text) || !File.Exists(fileName))
			{
				return "";
			}
			string text2 = Path.ChangeExtension(base.Server.MapPath(imgFile), ".jpg");
			string text3 = Path.ChangeExtension(base.Server.MapPath(playFile), ".flv");
			string sizeOfImg = VideoHelper.SizeOfImg;
			ProcessStartInfo startInfo = new ProcessStartInfo(text)
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = string.Concat(new string[]
				{
					"   -i   ",
					fileName,
					"   -y   -f   image2   -t   0.001   -s   ",
					sizeOfImg,
					"   ",
					text2
				})
			};
			ProcessStartInfo startInfo2 = new ProcessStartInfo(text)
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = string.Concat(new string[]
				{
					" -i ",
					fileName,
					" -ab 56 -ar 22050 -b 500 -r 15 -s ",
					VideoHelper.WidthOfFile,
					"x",
					VideoHelper.HeightOfFile,
					" ",
					text3
				})
			};
			try
			{
				Process.Start(startInfo2);
				Process.Start(startInfo);
			}
			catch
			{
				string result = "";
				return result;
			}
			if (!File.Exists(text2))
			{
				return "";
			}
			return text2;
		}
		/// <summary>
		/// 运行mencoder的视频解码器转换
		/// <param name="vFileName"></param>
		/// <param name="playFile"></param>
		/// <param name="imgFile"></param>
		/// </summary>
		public string MChangeFilePhy(string vFileName, string playFile, string imgFile)
		{
			string text = base.Server.MapPath(VideoHelper.Mencodertool);
			if (!File.Exists(text) || !File.Exists(vFileName))
			{
				return "";
			}
			string text2 = Path.ChangeExtension(playFile, ".flv");
			ProcessStartInfo startInfo = new ProcessStartInfo(text)
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = string.Concat(new string[]
				{
					" ",
					vFileName,
					" -o ",
					text2,
					" -of lavf -lavfopts i_certify_that_my_video_stream_does_not_use_b_frames -oac mp3lame -lameopts abr:br=56 -ovc lavc -lavcopts vcodec=flv:vbitrate=200:mbd=2:mv0:trell:v4mv:cbp:last_pred=1:dia=-1:cmp=0:vb_strategy=1 -vf scale=",
					VideoHelper.WidthOfFile,
					":",
					VideoHelper.HeightOfFile,
					" -ofps 12 -srate 22050"
				})
			};
			try
			{
				Process.Start(startInfo);
				this.CatchImg(text2, imgFile);
			}
			catch
			{
				return "";
			}
			return "";
		}
	}
}
