using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public class WebApiTools
    {
        /// <summary>
        /// post方式请求webapi
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <param name="serviceUrl">请求url</param>
        /// <returns></returns>
        public static T Post<T>(object requestParams, string serviceUrl)
            where T : class
        {
            string requestJson = JsonConvert.SerializeObject(requestParams);
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //请求要出发增加数据验证
            //string PartnerId = YaoChuFaConfig.YaoChuFaPartnerId;
            //httpContent.Headers.Add("PartnerId", PartnerId);
            //string Access_Token = YaoChuFaConfig.YaoChuFaAccess_Token;
            //httpContent.Headers.Add("Access_Token", Access_Token);
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            httpContent.Headers.Add("TimeStamp", timeStamp);
            httpContent.Headers.Add("Version", "1.0");
            //string Secret = YaoChuFaConfig.YaoChuFaSecret;
            //string Signed = Tools01.GetMD5(Access_Token + timeStamp + Secret + requestJson);
            //httpContent.Headers.Add("Signed", Signed);
            var httpClient = new HttpClient();
            string responseJson = httpClient.PostAsync(serviceUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();
            httpContent.Dispose();
            return JsonConvert.DeserializeObject(responseJson, typeof(T)) as T;
        }



        /// <summary>
        /// get方式请求webapi
        /// </summary>
        /// <param name="ServiceUrl">请求url</param>
        /// <returns></returns>
        public static T Get<T>(string serviceUrl)
            where T : class
        {
            string responseJson = string.Empty;
            var httpClient = new HttpClient();

            responseJson = httpClient.GetAsync(serviceUrl).Result.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();

            return JsonConvert.DeserializeObject(responseJson, typeof(T)) as T;
        }

        /// <summary>
        /// get方式请求webapi
        /// </summary>
        /// <param name="ServiceUrl">请求url</param>
        /// <returns></returns>
        public static string Get(string serviceUrl)
        {
            string responseJson = string.Empty;
            var httpClient = new HttpClient();

            responseJson = httpClient.GetAsync(serviceUrl).Result.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();
            if (string.IsNullOrWhiteSpace(responseJson))
            {
                responseJson = "";
            }
            return responseJson;
        }
    }
}
