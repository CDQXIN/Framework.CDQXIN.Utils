using Framework.CDQXIN.Utils.ExtensionHelper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
    /// <summary>
    /// Http工具类
    /// </summary>
    public class HttpHelper
    {
        #region GET方式请求远程页面内容

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="url">URL地址</param>
        /// <param name="data">参数列表</param>
        public static string Get(string url, object data)
        {
            var queryString = data != null ? string.Join("&",
                                from p in data.GetType().GetProperties()
                                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString())) : "";

            return Get(url, queryString, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="url">Url地址</param>
        /// <param name="data">参数对象</param>
        /// <returns></returns>
        public static T Get<T>(string url, object data) where T : class
        {
            var res = Get(url, data);

            if (!string.IsNullOrEmpty(res))
            {
                return res.ToObject<T>();
            }

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="data">参数列表</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string Get(string url, object data, Encoding encoding)
        {
            var queryString = data != null ? string.Join("&",
                                from p in data.GetType().GetProperties()
                                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString())) : "";

            return Get(url, queryString, encoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="url">Url地址</param>
        /// <param name="data">参数列表</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static T Get<T>(string url, object data, Encoding encoding) where T : class
        {
            var res = Get(url, data, encoding);

            if (!string.IsNullOrEmpty(res))
            {
                return res.ToObject<T>();
            }
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="queryString">参数列表</param>
        /// <returns>响应资源体</returns>
        public static string Get(string url, string queryString)
        {
            return Get(url, queryString, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="queryString">参数列表</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Get(string url, string queryString, Encoding encoding)
        {
            string res = string.Empty;
            try
            {
                var request = WebRequest.Create(string.Format("{0}{1}{2}", url, url.Contains("?") ? "&" : "?", queryString)) as HttpWebRequest;
                if (request == null) return string.Empty;
                request.Timeout = 19600;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                var response = (HttpWebResponse)request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream, encoding);
                        var sb = new StringBuilder();
                        while (-1 != reader.Peek())
                        {
                            sb.Append(reader.ReadLine() + Environment.NewLine);
                        }
                        res = sb.ToString();
                    }
                }
                response.Close();
            }
            catch (Exception ee)
            {
                res = ee.Message;
            }
            return res;
        }

        #endregion

        #region POST方式获取远程内容
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="data">参数列表</param>
        /// <returns></returns>
        public static string Post(string url, object data)
        {
            return Post(url, data, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="url">Url地址</param>
        /// <param name="data">参数对象</param>
        /// <returns></returns>
        public static T Post<T>(string url, object data) where T : class
        {
            var res = Post(url, data);

            if (!string.IsNullOrEmpty(res))
            {
                return res.ToObject<T>();
            }

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="data">参数列表</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Post(string url, object data, Encoding encoding)
        {
            var queryString = data != null ? string.Join("&",
                                from p in data.GetType().GetProperties()
                                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString())) : "";

            return Post(url, queryString, encoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="url">Url地址</param>
        /// <param name="data">参数对象</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static T Post<T>(string url, object data, Encoding encoding) where T : class
        {
            var res = Post(url, data, encoding);
            if (!string.IsNullOrEmpty(res))
            {
                return res.ToObject<T>();
            }

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="querySting">参数列表</param>
        /// <returns></returns>
        public static string Post(string url, string querySting)
        {
            return Post(url, querySting, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="url">Url地址</param>
        /// <param name="queryString">参数</param>
        /// <returns></returns>
        public static T Post<T>(string url, string queryString) where T : class
        {
            var res = Post(url, queryString);

            if (!string.IsNullOrEmpty(res))
            {
                return res.ToObject<T>();
            }

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="querySting">参数列表</param>
        /// <param name="encoding">字符编码</param>
        public static string Post(string url, string querySting, Encoding encoding)
        {
            string strResult = string.Empty;
            try
            {
                var postData = encoding.GetBytes(querySting);
                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request == null) return string.Empty;
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(postData, 0, postData.Length); //设置POST
                newStream.Close();
                var response = request.GetResponse() as HttpWebResponse;
                if (response == null) return string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream, encoding);
                        strResult = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="url">Url地址</param>
        /// <param name="hashtable">Handler 默认为null</param>
        /// <param name="content">参数对象</param>
        /// <param name="mediaType">要用于该内容的媒体。</param>
        /// <returns></returns>
        public static T Post<T>(string url, object content, Hashtable hashtable = null, string mediaType = "application/json")
        {
            try
            {
                var handler = new RequestHeaderHandler(hashtable)
                {
                    InnerHandler = new HttpClientHandler()
                };

                var httpClient = new HttpClient(handler);
                var httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, mediaType);
                var post = httpClient.PostAsync(url, httpContent);
                post.Wait();
                var response = post.Result.Content.ReadAsStringAsync();
                response.Wait();
                return JsonConvert.DeserializeObject<T>(response.Result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="hashtable">Handler 默认为null</param>
        /// <param name="content">参数对象</param>
        /// <param name="mediaType">要用于该内容的媒体。</param>
        /// <returns></returns>
        public static string Post(string url, object content, Hashtable hashtable = null, string mediaType = "application/json")
        {
            try
            {
                var handler = new RequestHeaderHandler(hashtable)
                {
                    InnerHandler = new HttpClientHandler()
                };

                var httpClient = new HttpClient(handler);
                var httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, mediaType);
                var post = httpClient.PostAsync(url, httpContent);
                post.Wait();
                var response = post.Result.Content.ReadAsStringAsync();
                response.Wait();

                return response.Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// POST方式使用HttpClient类获取页面内容
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="content">参数对象</param>
        /// <returns></returns>
        public static string PostByHc(string url, object content)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
                var post = httpClient.PostAsync(url, httpContent);
                post.Wait();
                var response = post.Result.Content.ReadAsStringAsync();
                response.Wait();
                return response.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取远程数据流 +static Stream GetStream(string url, object data)
        /// <summary>
        /// 获取远程数据流
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="data">参数列表</param>
        /// <returns>数据流</returns>
        public static Stream GetStream(string url, object data)
        {
            var queryString = string.Join("&",
                               from p in data.GetType().GetProperties()
                               select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString()));
            return GetStream(url, queryString);
        }
        #endregion

        #region 获取远程数据流 +static Stream GetStream(string url, string queryString)
        /// <summary>
        /// 获取远程数据流
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="queryString">参数列表</param>
        /// <returns>数据流</returns>
        public static Stream GetStream(string url, string queryString)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //if (data.Contains("{"))
                //    data = data.TrimStart('{').TrimEnd('}').Replace(":", "=").Replace(",", "&").Replace(" ", string.Empty);
                request = WebRequest.Create(string.Format("{0}?{1}", url, queryString)) as HttpWebRequest;
                if (request == null) return null;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ServicePoint.ConnectionLimit = 300;
                request.Referer = url;
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;
                return response == null ? null : response.GetResponseStream();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (request != null)
                    request.Abort();

                if (response != null)
                    response.Close();
            }
        }
        #endregion

        #region 获取资源大小
        /// <summary>
        /// 获取资源大小
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <returns></returns>
        public static long GetContentLength(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return 0;
            }

            try
            {
                var length = 0L;

                var request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                request.Method = "HEAD";
                using (var res = (HttpWebResponse)request.GetResponse())
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        length = res.ContentLength;
                    }
                }

                request.Abort();

                return length;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    public class RequestHeaderHandler : DelegatingHandler
    {

        /// <summary>
        /// 
        /// </summary>
        public Hashtable Hashtable { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashtable"></param>
        public RequestHeaderHandler(Hashtable hashtable)
            : base()
        {
            Hashtable = hashtable;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach (var key in Hashtable.Keys)
            {
                request.Headers.Add(key.ToString(), Hashtable[key].ToString());
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
