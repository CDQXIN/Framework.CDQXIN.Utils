using Framework.CDQXIN.Utils.ExtensionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 随机帮助类
	/// </summary>
	public class RandomHelper
	{
		private static readonly Random _random = new Random();
		/// <summary>
		/// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
		/// </summary>
		/// <param name="minNum">最小值</param>
		/// <param name="maxNum">最大值</param>
		public static int NextInt(int minNum, int maxNum)
		{
			return RandomHelper._random.Next(minNum, maxNum);
		}
		/// <summary>
		/// 生成一个0.0到1.0的随机小数
		/// </summary>
		public static double NextDouble()
		{
			return RandomHelper._random.NextDouble();
		}
		/// <summary>
		/// 对一个数组进行随机排序
		/// </summary>
		/// <typeparam name="T">数组的类型</typeparam>
		/// <param name="arr">需要随机排序的数组</param>
		/// <param name="count">指定交换次数默认0长度做交换次数</param>
		public static void ArraySort<T>(T[] arr, int count = 0)
		{
			count = ((count == 0) ? arr.Length : count);
			for (int i = 0; i < count; i++)
			{
				int num = RandomHelper.NextInt(0, arr.Length);
				int num2 = RandomHelper.NextInt(0, arr.Length);
				T t = arr[num];
				arr[num] = arr[num2];
				arr[num2] = t;
			}
		}
		/// <summary>
		/// 随机生成不重复数字字符串  
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string NextNumStr(int length)
		{
			string text = string.Empty;
			for (int i = 0; i < length; i++)
			{
				int num = RandomHelper._random.Next();
				text += ((char)(48 + (ushort)(num % 10))).ToString();
			}
			return text;
		}
		/// <summary>
		/// 随机生成字符串（数字和字母混和）
		/// </summary>
		/// <param name="length">长度</param>
		/// <param name="modetype">类型（1:数字、大小写字母）（2：数字、小写字母）（3：数字、大写字母）（4、大小写字母）</param>
		/// <returns></returns>
		public static string NextString(int length, int modetype = 1)
		{
			string allChar = "";
			switch (modetype)
			{
				case 1:
					{
						allChar = "0123456789abcdefjhijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
						break;
					}
				case 2:
					{
						allChar = "0123456789abcdefjhijklmnopqrstuvwxyz";
						break;
					}
				case 3:
					{
						allChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
						break;
					}
				case 4:
					{
						allChar = "abcdefjhijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
						break;
					}
			}
			return RandomHelper.NextString(length, allChar);
		}
		/// <summary>
		/// 从字符串里随机得到，规定个数的字符串.
		/// </summary>
		/// <param name="allChar"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string NextString(int length, string allChar)
		{
			if (allChar.IsNullOrEmpty())
			{
				return "";
			}
			char[] array = allChar.ToCharArray();
			string text = string.Empty;
			for (int i = 0; i < length; i++)
			{
				text += array[RandomHelper._random.Next(0, array.Length - 1)].ToString();
			}
			return text;
		}
	}
}
