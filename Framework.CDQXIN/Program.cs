using Framework.CDQXIN.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework.CDQXIN
{
    class Program
    {
        static void Main(string[] args)
        {
            //PDFHelper.CreatePdf(@"E:\File", @"F:\Net\Framework.CDQXIN.Utils\Framework.CDQXIN\UploadFiles\myself.html");
            var match = Regex.Match("<td rowspan='1' colspan='1' row='3' col='0'>名称1年后名称1年ss</td>", @"<td[\s\S]*?rowspan='(?<row>[\s\S]*?)'[\s\S]*?colspan='(?<col>[\s\S]*?)'[\s\S]*?row='(?<row1>[\s\S]*?)'[\s\S]*?col='(?<col1>[\s\S]*?)'>(?<value>[\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            //var match = Regex.Match("<td rowspan='1' colspan='1' row='3' col='0'>名称1年后\n</td>", "<td.*?rowspan='(?<row>.*?)'.*?colspan='(?<col>.*?)'.*?row='(?<row1>.*?)'.*?col='(?<col1>.*?)'>(?<value>.*?)<\\/td>", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            Console.WriteLine(match.Success);
            Console.WriteLine("处理结束");
            Console.ReadLine();
        }
    }
}
