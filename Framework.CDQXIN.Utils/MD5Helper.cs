using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public class MD5Helper
    {
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)), 4, 8);
            t2 = t2.Replace("-", string.Empty);
            return t2;
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string password = "")
        {
            string pwd = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password))
                {
                    MD5 md5 = MD5.Create(); //实例化一个md5对像
                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                    byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                    // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                    foreach (var item in s)
                    {
                        // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                        pwd = string.Concat(pwd, item.ToString("X"));
                    }
                }
            }
            catch
            {
                throw new Exception($"错误的 password 字符串:【{password}】");
            }
            return pwd;
        }

        /// <summary>
        /// 64位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string password)
        {
            // 实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(s);
        }


        #region MD5
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同
        /// </summary>
        /// <param name="strSource">待加密字串</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public static string Encrypt(string source, int length = 32)
        {
            HashAlgorithm provider = CryptoConfig.CreateFromName("MD5") as HashAlgorithm;
            if (string.IsNullOrEmpty(source)) return string.Empty;

            byte[] bytes = Encoding.ASCII.GetBytes(source);
            byte[] hashValue = provider.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (length)
            {
                case 16://16位密文是32位密文的9到24位字符
                    for (int i = 4; i < 12; i++)
                        sb.Append(hashValue[i].ToString("x2"));
                    break;
                case 32:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }
        #endregion MD5

    }
}
