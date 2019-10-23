using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
    public class PDFHelper
    {
        public static string CreatePdf(string pdfPath, string webUrl)
        {
            try
            {
                //Log.LogInfo("pdfPath:" + pdfPath + "webUrl:" + webUrl);
                string path = System.Web.HttpContext.Current.Server.MapPath("~" + pdfPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                WebClient wc = new WebClient();
                //从网址下载Html字串
                wc.Encoding = System.Text.Encoding.UTF8;
                string htmlText = getWebContent(webUrl);
                //Log.LogInfo("html信息：" + htmlText);
                byte[] pdfFile = ConvertHtmlTextToPDF(htmlText);
                string fileId = "/file_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                System.IO.File.WriteAllBytes(path + fileId, pdfFile);
                return pdfPath + fileId;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        /// <summary>
        /// 下载文件，支持大文件、续传、速度限制。支持续传的响应头Accept-Ranges、ETag，请求头Range 。
        /// Accept-Ranges：响应头，向客户端指明，此进程支持可恢复下载.实现后台智能传输服务（BITS），值为：bytes；
        /// ETag：响应头，用于对客户端的初始（200）响应，以及来自客户端的恢复请求，
        /// 必须为每个文件提供一个唯一的ETag值（可由文件名和文件最后被修改的日期组成），这使客户端软件能够验证它们已经下载的字节块是否仍然是最新的。
        /// Range：续传的起始位置，即已经下载到客户端的字节数，值如：bytes=1474560- 。
        /// 另外：UrlEncode编码后会把文件名中的空格转换中+（+转换为%2b），但是浏览器是不能理解加号为空格的，所以在浏览器下载得到的文件，空格就变成了加号；
        /// 解决办法：UrlEncode 之后, 将 "+" 替换成 "%20"，因为浏览器将%20转换为空格
        /// </summary>
        /// <param name="httpContext">当前请求的HttpContext</param>
        /// <param name="filePath">下载文件的物理路径，含路径、文件名</param>
        /// <param name="speed">下载速度：每秒允许下载的字节数</param>
        /// <returns>true下载成功，false下载失败</returns>
        public static bool DownloadFile(HttpContext httpContext, string filePath, long speed, string fileSaveName)
        {
            httpContext.Response.Clear();
            bool ret = true;
            try
            {
                #region --验证：HttpMethod，请求的文件是否存在#region
                switch (httpContext.Request.HttpMethod.ToUpper())
                { //目前只支持GET和HEAD方法
                    case "GET":
                    case "POST":
                    case "HEAD":
                        break;
                    default:
                        httpContext.Response.StatusCode = 501;
                        return false;
                }
                filePath = System.Web.HttpContext.Current.Server.MapPath("~" + filePath);
                FileInfo file = new FileInfo(filePath);
                if (!file.Exists)
                {
                    httpContext.Response.StatusCode = 404;
                    return false;
                }
                #endregion

                #region 定义局部变量#region 定义局部变量
                long startBytes = 0;
                long stopBytes = 0;
                int packSize = 1024 * 10000; //分块读取，每块10K bytes
                string fileName = fileSaveName;
                FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                long fileLength = myFile.Length;

                int sleep = (int)Math.Ceiling(1000.0 * packSize / speed);//毫秒数：读取下一数据块的时间间隔
                string lastUpdateTiemStr = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string eTag = HttpUtility.UrlEncode(fileName, Encoding.UTF8) + lastUpdateTiemStr;//便于恢复下载时提取请求头;
                #endregion

                #region --验证：文件是否太大，是否是续传，且在上次被请求的日期之后是否被修改过
                if (myFile.Length > long.MaxValue)
                {//-------文件太大了-------
                    httpContext.Response.StatusCode = 413;//请求实体太大
                    return false;
                }

                if (httpContext.Request.Headers["If-Range"] != null)//对应响应头ETag：文件名+文件最后修改时间
                {
                    //----------上次被请求的日期之后被修改过--------------
                    if (httpContext.Request.Headers["If-Range"].Replace("\"", "") != eTag)
                    {//文件修改过
                        httpContext.Response.StatusCode = 412;//预处理失败
                        return false;
                    }
                }
                #endregion

                try
                {
                    #region -------添加重要响应头、解析请求头、相关验证
                    httpContext.Response.Clear();

                    if (httpContext.Request.Headers["Range"] != null)
                    {//------如果是续传请求，则获取续传的起始位置，即已经下载到客户端的字节数------
                        httpContext.Response.StatusCode = 206;//重要：续传必须，表示局部范围响应。初始下载时默认为200
                        string[] range = httpContext.Request.Headers["Range"].Split(new char[] { '=', '-' });//"bytes=1474560-"
                        startBytes = Convert.ToInt64(range[1]);//已经下载的字节数，即本次下载的开始位置  
                        if (startBytes < 0 || startBytes >= fileLength)
                        {//无效的起始位置
                            return false;
                        }
                        if (range.Length == 3)
                        {
                            stopBytes = Convert.ToInt64(range[2]);//结束下载的字节数，即本次下载的结束位置  
                            if (startBytes < 0 || startBytes >= fileLength)
                            {
                                return false;
                            }
                        }
                    }

                    httpContext.Response.Buffer = false;
                    //httpContext.Response.AddHeader("Content-MD5", FileHash.MD5File(filePath));//用于验证文件
                    httpContext.Response.AddHeader("Accept-Ranges", "bytes");//重要：续传必须
                    httpContext.Response.AppendHeader("ETag", "\"" + eTag + "\"");//重要：续传必须
                    httpContext.Response.AppendHeader("Last-Modified", lastUpdateTiemStr);//把最后修改日期写入响应                
                    httpContext.Response.ContentType = "application/octet-stream";//MIME类型：匹配任意文件类型
                    httpContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).Replace("+", "%20"));
                    httpContext.Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    httpContext.Response.AddHeader("Connection", "Keep-Alive");
                    httpContext.Response.ContentEncoding = Encoding.UTF8;
                    if (startBytes > 0)
                    {//------如果是续传请求，告诉客户端本次的开始字节数，总长度，以便客户端将续传数据追加到startBytes位置后----------
                        httpContext.Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    #endregion

                    #region -------向客户端发送数据块-------------------
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Ceiling((fileLength - startBytes + 0.0) / packSize);//分块下载，剩余部分可分成的块数
                    for (int i = 0; i < maxCount && httpContext.Response.IsClientConnected; i++)
                    {//客户端中断连接，则暂停
                        httpContext.Response.BinaryWrite(br.ReadBytes(packSize));
                        httpContext.Response.Flush();
                        if (sleep > 1) Thread.Sleep(sleep);
                    }
                    #endregion
                }
                catch
                {
                    ret = false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                    myFile.Dispose();
                    br.Dispose();
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 判断是否有乱码
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool isMessyCode(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);            //239 191 189            
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }
        /// <summary>
        /// 获取网站内容，包含了 HTML+CSS+JS
        /// </summary>
        /// <returns>String返回网页信息</returns>
        public static string getWebContent(string webUrl)
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                //获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                Byte[] pageData = MyWebClient.DownloadData(webUrl);
                //从指定网站下载数据
                string pageHtml = Encoding.UTF8.GetString(pageData);
                //如果获取网站页面采用的是GB2312，则使用这句       
                bool isBool = isMessyCode(pageHtml);//判断使用哪种编码 读取网页信息
                if (!isBool)
                {
                    string pageHtml1 = Encoding.UTF8.GetString(pageData);
                    pageHtml = pageHtml1;
                }
                else
                {
                    string pageHtml2 = Encoding.Default.GetString(pageData);
                    pageHtml = pageHtml2;
                }
                return pageHtml;
            }

            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
                return webEx.Message;
            }
        }


        /// <summary>
        /// 将Html文字 输出到PDF档里
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static byte[] ConvertHtmlTextToPDF(string htmlText)
        {
            try
            {
                if (string.IsNullOrEmpty(htmlText))
                {
                    return null;
                }
                //避免当htmlText无任何html tag标签的纯文字时，转PDF时会挂掉，所以一律加上<p>标签
                htmlText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + htmlText + "";
                //htmlText = "<p>" + htmlText + "</p>";
                MemoryStream outputStream = new MemoryStream();//要把PDF写到哪个串流
                byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串转成byte[]
                MemoryStream msInput = new MemoryStream(data);
                Document doc = new Document();//要写PDF的文件，建构子没填的话预设直式A4
                PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
                //指定文件预设开档时的缩放为100%

                PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
                //开启Document文件 
                doc.Open();

                //使用XMLWorkerHelper把Html parse到PDF档里
                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8);

                //将pdfDest设定的资料写到PDF档
                PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                writer.SetOpenAction(action);
                doc.Close();
                msInput.Close();
                outputStream.Close();
                //回传PDF档案 
                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //设置字体类
        public class UnicodeFontFactory : FontFactoryImp
        {
            private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
      "arialuni.ttf");//arial unicode MS是完整的unicode字型。
            private static readonly string 标楷体Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            "KAIU.TTF");//标楷体


            public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
            {
                BaseFont bfChiness = BaseFont.CreateFont(@"C:\Windows\Fonts\SIMSUN.TTC,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                //可用Arial或标楷体，自己选一个
                BaseFont baseFont = BaseFont.CreateFont(标楷体Path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                return new Font(bfChiness, size, style, color);
            }
        }
    }
}
