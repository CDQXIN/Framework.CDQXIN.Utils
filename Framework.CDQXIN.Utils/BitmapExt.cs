using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// Bitmap 扩展
	/// </summary>
	public static class BitmapExt
	{
	//	[CompilerGenerated]
	//	[Serializable]
	//	private sealed class <>c
	//	{
	//		public static readonly BitmapExt.<>c<>9 = new BitmapExt.<>c();
	//	public static Func<ImageCodecInfo, bool> <>9__13_0;
	//		internal bool <Compress>b__13_0(ImageCodecInfo t)
	//	{
	//		return t.FormatDescription.Equals("JPEG");
	//	}
	//}
	/// <summary>
	/// 调整光暗
	/// </summary>
	/// <param name="mybm">原始图片</param>
	/// <param name="val">增加或减少的光暗值（-255----255）</param>
	/// <returns></returns>
	public static Bitmap LdPic(this Bitmap mybm, int val)
	{
		Bitmap bitmap = new Bitmap(mybm.Width, mybm.Height);
		for (int i = 0; i < bitmap.Width; i++)
		{
			for (int j = 0; j < bitmap.Height; j++)
			{
				Color pixel = mybm.GetPixel(i, j);
				bitmap.SetPixel(i, j, Color.FromArgb(BitmapExt.GetColor((int)pixel.R + val), BitmapExt.GetColor((int)pixel.G + val), BitmapExt.GetColor((int)pixel.B + val)));
			}
		}
		return bitmap;
	}
	/// <summary>
	/// 反色处理
	/// </summary>
	/// <param name="mybm">原始图片</param>
	/// <returns></returns>
	public static Bitmap RePic(this Bitmap mybm)
	{
		Bitmap bitmap = new Bitmap(mybm.Width, mybm.Height);
		for (int i = 0; i < bitmap.Width; i++)
		{
			for (int j = 0; j < bitmap.Height; j++)
			{
				Color pixel = mybm.GetPixel(i, j);
				bitmap.SetPixel(i, j, Color.FromArgb(BitmapExt.GetColor((int)(255 - pixel.R)), BitmapExt.GetColor((int)(255 - pixel.G)), BitmapExt.GetColor((int)(255 - pixel.B))));
			}
		}
		return bitmap;
	}
	/// <summary>
	/// 浮雕处理
	/// </summary>
	/// <param name="oldBitmap">原始图片</param>
	public static Bitmap Fd(this Bitmap oldBitmap)
	{
		Bitmap bitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
		for (int i = 0; i < bitmap.Width - 1; i++)
		{
			for (int j = 0; j < bitmap.Height - 1; j++)
			{
				Color pixel = oldBitmap.GetPixel(i, j);
				Color pixel2 = oldBitmap.GetPixel(i + 1, j + 1);
				int color = BitmapExt.GetColor(Math.Abs((int)(pixel.R - pixel2.R + 128)));
				int color2 = BitmapExt.GetColor(Math.Abs((int)(pixel.G - pixel2.G + 128)));
				int color3 = BitmapExt.GetColor(Math.Abs((int)(pixel.B - pixel2.B + 128)));
				bitmap.SetPixel(i, j, Color.FromArgb(color, color2, color3));
			}
		}
		return bitmap;
	}
	/// <summary>
	/// 拉伸图片
	/// </summary>
	/// <param name="bmp">原始图片</param>
	public static Bitmap ResizeImage(this Bitmap bmp)
	{
		Bitmap bitmap = new Bitmap(bmp.Width, bmp.Height);
		Graphics expr_18 = Graphics.FromImage(bitmap);
		expr_18.InterpolationMode = InterpolationMode.HighQualityBicubic;
		expr_18.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
		expr_18.Dispose();
		return bitmap;
	}
	/// <summary>
	/// 滤色处理
	/// </summary>
	/// <param name="mybm">原始图片</param>
	public static Bitmap FilPic(this Bitmap mybm)
	{
		Bitmap bitmap = new Bitmap(mybm.Width, mybm.Height);
		for (int i = 0; i < bitmap.Width; i++)
		{
			for (int j = 0; j < bitmap.Height; j++)
			{
				Color pixel = mybm.GetPixel(i, j);
				bitmap.SetPixel(i, j, Color.FromArgb(0, (int)pixel.G, (int)pixel.B));
			}
		}
		return bitmap;
	}
	/// <summary>
	/// 左右翻转
	/// </summary>
	/// <param name="mybm">原始图片</param>
	public static Bitmap RevPicLr(this Bitmap mybm)
	{
		Bitmap bitmap = new Bitmap(mybm.Width, mybm.Height);
		for (int i = bitmap.Height - 1; i >= 0; i--)
		{
			int j = bitmap.Width - 1;
			int num = 0;
			while (j >= 0)
			{
				Color pixel = mybm.GetPixel(j, i);
				bitmap.SetPixel(num++, i, Color.FromArgb((int)pixel.R, (int)pixel.G, (int)pixel.B));
				j--;
			}
		}
		return bitmap;
	}
	/// <summary>
	/// 上下翻转
	/// </summary>
	/// <param name="mybm">原始图片</param>
	public static Bitmap RevPicUd(this Bitmap mybm)
	{
		Bitmap bitmap = new Bitmap(mybm.Width, mybm.Height);
		for (int i = 0; i < bitmap.Width; i++)
		{
			int j = bitmap.Height - 1;
			int num = 0;
			while (j >= 0)
			{
				Color pixel = mybm.GetPixel(i, j);
				bitmap.SetPixel(i, num++, Color.FromArgb((int)pixel.R, (int)pixel.G, (int)pixel.B));
				j--;
			}
		}
		return bitmap;
	}
	/// <summary>
	/// 转换为黑白图片
	/// </summary>
	/// <param name="mybm">要进行处理的图片</param>
	public static Bitmap BwPic(this Bitmap mybm)
	{
		Bitmap bitmap = new Bitmap(mybm.Width, mybm.Height);
		for (int i = 0; i < bitmap.Width; i++)
		{
			for (int j = 0; j < bitmap.Height; j++)
			{
				Color pixel = mybm.GetPixel(i, j);
				int num = (int)((pixel.R + pixel.G + pixel.B) / 3);
				bitmap.SetPixel(i, j, Color.FromArgb(num, num, num));
			}
		}
		return bitmap;
	}
	/// <summary>
	/// 生成缩略图
	/// </summary>
	/// <param name="originalImage">源图</param>
	/// <param name="width">缩略图宽度</param>
	/// <param name="height">缩略图高度</param>
	/// <param name="mode">生成缩略图的方式
	/// <example>
	/// HW:指定高宽缩放（可能变形）；
	/// W:指定宽，高按比例 ；
	/// H:指定高，宽按比例 ；
	/// Cut:指定高宽裁减（不变形）；
	/// </example>
	/// </param>  
	/// <returns>缩略图</returns>
	public static Image MakeThumbnail(this Image originalImage, int width, int height, string mode)
	{
		int num = width;
		int num2 = height;
		int x = 0;
		int y = 0;
		int num3 = originalImage.Width;
		int num4 = originalImage.Height;
		if (!(mode == "HW"))
		{
			if (!(mode == "W"))
			{
				if (!(mode == "H"))
				{
					if (mode == "Cut")
					{
						if ((double)originalImage.Width / (double)originalImage.Height > (double)num / (double)num2)
						{
							num4 = originalImage.Height;
							num3 = originalImage.Height * num / num2;
							y = 0;
							x = (originalImage.Width - num3) / 2;
						}
						else
						{
							num3 = originalImage.Width;
							num4 = originalImage.Width * height / num;
							x = 0;
							y = (originalImage.Height - num4) / 2;
						}
					}
				}
				else
				{
					num = originalImage.Width * height / originalImage.Height;
				}
			}
			else
			{
				num2 = originalImage.Height * width / originalImage.Width;
			}
		}
		Bitmap expr_DB = new Bitmap(num, num2);
		Graphics expr_E1 = Graphics.FromImage(expr_DB);
		expr_E1.InterpolationMode = InterpolationMode.High;
		expr_E1.SmoothingMode = SmoothingMode.HighQuality;
		expr_E1.Clear(Color.Transparent);
		expr_E1.DrawImage(originalImage, new Rectangle(0, 0, num, num2), new Rectangle(x, y, num3, num4), GraphicsUnit.Pixel);
		return expr_DB;
	}
	/// <summary>
	/// 图片水印处理方法
	/// </summary>
	/// <param name="img">需要加载水印的图片</param>
	/// <param name="waterimg">水印图片</param>
	/// <param name="location">水印位置
	/// <example> LT\T\RT\LC\C\RC\LB\B\</example>
	/// </param>
	/// <returns></returns>
	public static Image ImageWatermark(this Image img, Image waterimg, string location)
	{
		Graphics arg_0F_0 = Graphics.FromImage(img);
		ArrayList location2 = BitmapExt.GetLocation(location, img, waterimg);
		arg_0F_0.DrawImage(waterimg, new Rectangle(int.Parse(location2[0].ToString()), int.Parse(location2[1].ToString()), waterimg.Width, waterimg.Height));
		waterimg.Dispose();
		arg_0F_0.Dispose();
		return img;
	}
	/// <summary>
	/// 图片水印位置处理方法
	/// </summary>
	/// <param name="location">水印位置</param>
	/// <param name="img">需要添加水印的图片</param>
	/// <param name="waterimg">水印图片</param>
	private static ArrayList GetLocation(string location, Image img, Image waterimg)
	{
		ArrayList arg_16D_0 = new ArrayList();
		int num = 0;
		int num2 = 0;
		if (location == "LT")
		{
			num = 10;
			num2 = 10;
		}
		else
		{
			if (location == "T")
			{
				num = img.Width / 2 - waterimg.Width / 2;
				num2 = img.Height - waterimg.Height;
			}
			else
			{
				if (location == "RT")
				{
					num = img.Width - waterimg.Width;
					num2 = 10;
				}
				else
				{
					if (location == "LC")
					{
						num = 10;
						num2 = img.Height / 2 - waterimg.Height / 2;
					}
					else
					{
						if (location == "C")
						{
							num = img.Width / 2 - waterimg.Width / 2;
							num2 = img.Height / 2 - waterimg.Height / 2;
						}
						else
						{
							if (location == "RC")
							{
								num = img.Width - waterimg.Width;
								num2 = img.Height / 2 - waterimg.Height / 2;
							}
							else
							{
								if (location == "LB")
								{
									num = 10;
									num2 = img.Height - waterimg.Height;
								}
								else
								{
									if (location == "B")
									{
										num = img.Width / 2 - waterimg.Width / 2;
										num2 = img.Height - waterimg.Height;
									}
									else
									{
										num = img.Width - waterimg.Width;
										num2 = img.Height - waterimg.Height;
									}
								}
							}
						}
					}
				}
			}
		}
		arg_16D_0.Add(num);
		arg_16D_0.Add(num2);
		return arg_16D_0;
	}
	/// <summary>
	/// 文字水印处理方法
	/// </summary>
	/// <param name="img">需要加水印的图片</param>
	/// <param name="size">字体大小</param>
	/// <param name="letter">水印文字</param>
	/// <param name="color">颜色</param>
	/// <param name="location">水印位置
	/// <example> LT\T\RT\LC\C\RC\LB\B\</example>
	/// </param>
	/// <returns></returns>
	public static Image LetterWatermark(this Image img, int size, string letter, Color color, string location)
	{
		Graphics arg_2A_0 = Graphics.FromImage(img);
		ArrayList location2 = BitmapExt.GetLocation(location, img, size, letter.Length);
		Font font = new Font("宋体", (float)size);
		Brush brush = new SolidBrush(color);
		arg_2A_0.DrawString(letter, font, brush, float.Parse(location2[0].ToString()), float.Parse(location2[1].ToString()));
		arg_2A_0.Dispose();
		return img;
	}
	/// <summary>
	/// 文字水印位置的方法
	/// </summary>
	/// <param name="location">位置代码</param>
	/// <param name="img">图片对象</param>
	/// <param name="width">宽(当水印类型为文字时,传过来的就是字体的大小)</param>
	/// <param name="height">高(当水印类型为文字时,传过来的就是字符的长度)</param>
	private static ArrayList GetLocation(string location, Image img, int width, int height)
	{
		ArrayList arrayList = new ArrayList();
		float num = 10f;
		float num2 = 10f;
		if (location == "LT")
		{
			arrayList.Add(num);
			arrayList.Add(num2);
		}
		else
		{
			if (location == "T")
			{
				num = (float)(img.Width / 2 - width * height / 2);
				arrayList.Add(num);
				arrayList.Add(num2);
			}
			else
			{
				if (location == "RT")
				{
					num = (float)(img.Width - width * height);
				}
				else
				{
					if (location == "LC")
					{
						num2 = (float)(img.Height / 2);
					}
					else
					{
						if (location == "C")
						{
							num = (float)(img.Width / 2 - width * height / 2);
							num2 = (float)(img.Height / 2);
						}
						else
						{
							if (location == "RC")
							{
								num = (float)(img.Width - height);
								num2 = (float)(img.Height / 2);
							}
							else
							{
								if (location == "LB")
								{
									num2 = (float)(img.Height - width - 5);
								}
								else
								{
									if (location == "B")
									{
										num = (float)(img.Width / 2 - width * height / 2);
										num2 = (float)(img.Height - width - 5);
									}
									else
									{
										num = (float)(img.Width - width * height);
										num2 = (float)(img.Height - width - 5);
									}
								}
							}
						}
					}
				}
			}
		}
		arrayList.Add(num);
		arrayList.Add(num2);
		return arrayList;
	}
	/// <summary>
	/// 压缩指定尺寸，如果写的和图片大小一样表示大小不变，只是把图片压缩下一些
	/// </summary>
	/// <param name="img">原图片</param>
	/// <param name="width">长</param>
	/// <param name="height">高</param>
	/// <returns>新图片</returns>
	//public static Image Compress(this Image img, int width = 0, int height = 0)
	//{
	//	width = ((width <= 0) ? img.Width : width);
	//	height = ((height <= 0) ? img.Height : height);
	//	Size size = new Size(width, height);
	//	Bitmap bitmap = new Bitmap(size.Width, size.Height);
	//	Graphics expr_41 = Graphics.FromImage(bitmap);
	//	expr_41.CompositingQuality = CompositingQuality.HighQuality;
	//	expr_41.SmoothingMode = SmoothingMode.HighQuality;
	//	expr_41.InterpolationMode = InterpolationMode.HighQualityBicubic;
	//	expr_41.DrawImage(img, new Rectangle(0, 0, size.Width, size.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
	//	expr_41.Dispose();
	//	EncoderParameters arg_A4_0 = new EncoderParameters();
	//	long[] value = new long[]
	//	{
	//			100L
	//	};
	//	EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, value);
	//	arg_A4_0.Param[0] = encoderParameter;
	//	IEnumerable<ImageCodecInfo> arg_D0_0 = ImageCodecInfo.GetImageEncoders();
	//	Func<ImageCodecInfo, bool> arg_D0_1;
	//	if ((arg_D0_1 = BitmapExt.<> c.<> 9__13_0) == null)
	//	{
	//		arg_D0_1 = (BitmapExt.<> c.<> 9__13_0 = new Func<ImageCodecInfo, bool>(BitmapExt.<> c.<> 9.< Compress > b__13_0));
	//	}
	//	if (arg_D0_0.FirstOrDefault(arg_D0_1).IsNotNull())
	//	{
	//		return bitmap;
	//	}
	//	return img;
	//}
	/// <summary>
	/// 获取GIF图片中的帧
	/// </summary>
	/// <param name="gif">GIF图片</param>
	/// <returns></returns>
	public static List<Image> GetFrames(this Image gif)
	{
		List<Image> list = new List<Image>();
		FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
		int frameCount = gif.GetFrameCount(dimension);
		for (int i = 0; i < frameCount; i++)
		{
			gif.SelectActiveFrame(dimension, i);
			list.Add(new Bitmap(gif));
		}
		return list;
	}
	/// <summary>
	/// 获取有效颜色值
	/// </summary>
	/// <param name="val"></param>
	/// <returns></returns>
	private static int GetColor(int val)
	{
		if (val < 0)
		{
			return 0;
		}
		if (val > 255)
		{
			return 255;
		}
		return val;
	}
}
}
