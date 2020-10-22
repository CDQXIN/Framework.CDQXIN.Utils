using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>  
	/// 完美随机验证码
	/// Description:随机生成设定验证码，并随机旋转一定角度，字体颜色不同  
	/// </summary>  
	public class CaptchaHelper
	{
		/// <summary>  
		/// 生成随机码  
		/// </summary>  
		/// <param name="length">随机码个数</param>  
		/// <remarks>根据GUID去前几位数字</remarks>
		/// <returns></returns>  
		public static string CreateRandomCode(int length)
		{
			return Guid.NewGuid().ToString("N").Substring(0, length);
		}
		/// <summary>  
		/// 创建随机码图片  
		/// </summary>  
		/// <param name="vcode">验证码</param>
		/// <param name="fontSize">字体大小</param>
		/// <param name="background">背景颜色</param>
		/// <param name="border">边框颜色</param>
		/// <returns>Gif图片二进制流</returns>
		public static byte[] DrawImage(string vcode, float fontSize = 14f, Color background = default(Color), Color border = default(Color))
		{
			byte[] result;
			using (Bitmap bitmap = new Bitmap(vcode.Length * (int)fontSize + 3, (int)fontSize + 10))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.Clear(background);
					graphics.DrawRectangle(new Pen(border, 0f), 999, 0, bitmap.Width - 1, bitmap.Height - 1);
					Random random = new Random();
					Pen pen = new Pen(Color.DarkGray, 0f);
					for (int i = 0; i < 50; i++)
					{
						int x = random.Next(0, bitmap.Width);
						int y = random.Next(0, bitmap.Height);
						graphics.DrawRectangle(pen, x, y, 1, 1);
					}
					char[] array = vcode.ToCharArray();
					StringFormat format = new StringFormat(StringFormatFlags.NoClip)
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					};
					Color[] array2 = new Color[]
					{
						Color.Black,
						Color.DarkBlue,
						Color.Green,
						Color.Orange,
						Color.Brown,
						Color.DarkCyan,
						Color.Purple,
						Color.DarkGoldenrod
					};
					FontStyle[] expr_12F = new FontStyle[4];
					RuntimeHelpers.InitializeArray(expr_12F, ldtoken(2.0,null));
					FontStyle[] array3 = expr_12F;
					string[] array4 = new string[]
					{
						"Verdana",
						"Microsoft Sans Serif",
						"Comic Sans MS",
						"Arial",
						"宋体"
					};
					char[] array5 = array;
					for (int j = 0; j < array5.Length; j++)
					{
						char c = array5[j];
						int num = random.Next(8);
						int num2 = random.Next(5);
						int num3 = random.Next(4);
						Font font = new Font(array4[num2], fontSize, array3[num3]);
						Brush brush = new SolidBrush(array2[num]);
						Point point = new Point(16, 16);
						float num4 = (float)random.Next(-45, 45);
						graphics.TranslateTransform((float)point.X, (float)point.Y);
						graphics.RotateTransform(num4);
						graphics.DrawString(c.ToString(CultureInfo.InvariantCulture), font, brush, 1f, 1f, format);
						graphics.RotateTransform(-num4);
						graphics.TranslateTransform(2f, (float)(-(float)point.Y));
					}
				}
				MemoryStream memoryStream = new MemoryStream();
				bitmap.Save(memoryStream, ImageFormat.Gif);
				result = memoryStream.ToArray();
			}
			return result;
		}

        private static RuntimeFieldHandle ldtoken(double v, object e2B7DF2D15400D2997C7318A0237A5E33D3)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="vcode">验证码字符串</param>
        /// <returns></returns>
        public static byte[] DrawImage(string vcode)
		{
			Bitmap bitmap = new Bitmap((int)Math.Ceiling((double)vcode.Length * 12.0), 22);
			Graphics graphics = Graphics.FromImage(bitmap);
			byte[] result;
			try
			{
				Random random = new Random();
				graphics.Clear(Color.White);
				for (int i = 0; i < 25; i++)
				{
					int x = random.Next(bitmap.Width);
					int x2 = random.Next(bitmap.Width);
					int y = random.Next(bitmap.Height);
					int y2 = random.Next(bitmap.Height);
					graphics.DrawLine(new Pen(Color.Silver), x, y, x2, y2);
				}
				Font font = new Font("Arial", 12f, FontStyle.Bold | FontStyle.Italic);
				LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Color.Blue, Color.DarkRed, 1.2f, true);
				graphics.DrawString(vcode, font, brush, 3f, 2f);
				for (int j = 0; j < 100; j++)
				{
					int x3 = random.Next(bitmap.Width);
					int y3 = random.Next(bitmap.Height);
					bitmap.SetPixel(x3, y3, Color.FromArgb(random.Next()));
				}
				graphics.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1);
				MemoryStream memoryStream = new MemoryStream();
				bitmap.Save(memoryStream, ImageFormat.Jpeg);
				result = memoryStream.ToArray();
			}
			finally
			{
				graphics.Dispose();
				bitmap.Dispose();
			}
			return result;
		}
		/// <summary>
		/// 创建验证码
		/// </summary>
		/// <param name="vcode">验证码数字</param>
		/// <returns></returns>
		public static byte[] DrawImage(out string vcode)
		{
			vcode = Guid.NewGuid().ToString("N").Substring(0, 4);
			byte[] result;
			using (Bitmap bitmap = new Bitmap(70, 26, PixelFormat.Format32bppRgb))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.Clear(Color.FromArgb(46, 136, 180));
					graphics.SmoothingMode = SmoothingMode.AntiAlias;
					graphics.DrawString(vcode, new Font("黑体", 20f, FontStyle.Bold), new SolidBrush(Color.White), new PointF(0f, 0f));
					Random random = new Random();
					for (int i = 0; i < 8; i++)
					{
						int x = random.Next(bitmap.Width + 40);
						int y = random.Next(bitmap.Height + 10);
						int x2 = random.Next(bitmap.Width + 40);
						int y2 = random.Next(bitmap.Height + 10);
						graphics.DrawLine(new Pen(Color.White), x, y, x2, y2);
					}
					for (int j = 0; j < 25; j++)
					{
						int num = random.Next(bitmap.Width - 2);
						int num2 = random.Next(bitmap.Height - 2);
						bitmap.SetPixel(num, num2, Color.White);
						bitmap.SetPixel(num + 1, num2, Color.White);
						bitmap.SetPixel(num, num2 + 1, Color.White);
						bitmap.SetPixel(num + 1, num2 + 1, Color.White);
					}
					MemoryStream memoryStream = new MemoryStream();
					bitmap.Save(memoryStream, ImageFormat.Jpeg);
					result = memoryStream.ToArray();
				}
			}
			return result;
		}
	}
}
