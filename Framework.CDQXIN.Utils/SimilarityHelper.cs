using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	/// 相似度帮助类
	/// </summary>
	public class SimilarityHelper
	{
		/// <summary>
		/// 计算编辑距离相似度
		/// </summary>
		/// <param name="str1">字符串1</param>
		/// <param name="str2">字符串2</param>
		/// <returns></returns>
		public static decimal CalDistance(string str1, string str2)
		{
			int length = str1.Length;
			int length2 = str2.Length;
			if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
			{
				return decimal.Zero;
			}
			if (length == length2 && str1 == str2)
			{
				return decimal.One;
			}
			char[] array = str1.ToCharArray();
			char[] array2 = str2.ToCharArray();
			int num = array2.Length + 1;
			int num2 = array.Length + 1;
			int[,] array3 = new int[num2, num];
			for (int i = 0; i < num; i++)
			{
				array3[0, i] = i;
			}
			for (int j = 0; j < num2; j++)
			{
				array3[j, 0] = j;
			}
			for (int k = 1; k < num2; k++)
			{
				for (int l = 1; l < num; l++)
				{
					int num3 = (array[k - 1] == array2[l - 1]) ? 0 : 1;
					array3[k, l] = Math.Min(Math.Min(array3[k - 1, l] + 1, array3[k, l - 1] + 1), array3[k - 1, l - 1] + num3);
				}
			}
			int value = (num2 > num) ? num2 : num;
			return decimal.One - array3[num2 - 1, num - 1] / value;
		}
	}
}
