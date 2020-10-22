using Framework.CDQXIN.Utils.ExtensionHelper;
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

	/// <summary>
	/// WebApi帮助类
	/// </summary>
	public class WebApiHelper
	{
		/// <summary>
		/// Get获取接口数据
		/// </summary>
		/// <typeparam name="T">接口数据类型</typeparam>
		/// <param name="url">Url地址</param>
		/// <param name="encryptKey">秘钥</param>
		/// <param name="hasEncrypt">已经加密</param>
		/// <param name="timeout">超时时间</param>
		/// <returns></returns>
		public static ApiResultInfo<T> WebApi_Get<T>(string url, string encryptKey = "", bool hasEncrypt = false, int timeout = 0) where T : class
		{
			ApiResultInfo<T> apiResultInfo = new ApiResultInfo<T>
			{
				HasSuccess = true,
				RetCode = 0,
				RetMsg = string.Empty
			};
			string text = "";
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					if (timeout > 0)
					{
						httpClient.Timeout = TimeSpan.FromMilliseconds((double)timeout);
					}
					if (hasEncrypt)
					{
						url = WebApiHelper.AppendEncryptToWebApi(url, encryptKey);
					}
					text = httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
					apiResultInfo.InfoObj = text.ToObject<T>();
				}
			}
			catch (Exception ex)
			{
				apiResultInfo.HasSuccess = false;
				apiResultInfo.RetCode = -1;
				apiResultInfo.RetMsg = ex.ToString() + "\r\n\r\n" + text;
			}
			return apiResultInfo;
		}
		/// <summary>
		/// Get获取接口数据
		/// </summary>
		/// <typeparam name="T">接口数据类型</typeparam>
		/// <param name="url">Url地址</param>
		/// <param name="encryptKey">秘钥</param>
		/// <param name="hasEncrypt">已经加密</param>
		/// <param name="timeout">超时时间</param>
		/// <returns></returns>
		public static ApiResultInfo<T> ApiGate_Get<T>(string url, string encryptKey = "", bool hasEncrypt = false, int timeout = 0) where T : class
		{
			ApiResultInfo<T> apiResultInfo = new ApiResultInfo<T>
			{
				HasSuccess = true,
				RetCode = 0,
				RetMsg = string.Empty
			};
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					if (timeout > 0)
					{
						httpClient.Timeout = TimeSpan.FromMilliseconds((double)timeout);
					}
					if (hasEncrypt)
					{
						url = WebApiHelper.AppendEncryptToApiGate(url, encryptKey);
					}
					ApiGateResult apiGateResult = httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result.ToObject<ApiGateResult>();
					if (apiGateResult.RetCode == 0)
					{
						apiResultInfo.InfoObj = apiGateResult.Message.ToObject<T>();
					}
					else
					{
						apiResultInfo.HasSuccess = false;
						apiResultInfo.RetCode = apiGateResult.RetCode;
						apiResultInfo.RetMsg = apiGateResult.RetMsg;
					}
				}
			}
			catch (Exception ex)
			{
				apiResultInfo.HasSuccess = false;
				apiResultInfo.RetCode = -1;
				apiResultInfo.RetMsg = ex.ToString();
			}
			return apiResultInfo;
		}
		/// <summary>
		/// Post获取接口数据
		/// </summary>
		/// <typeparam name="T">数据类型</typeparam>
		/// <param name="url">Url地址</param>
		/// <param name="requestObj"></param>
		/// <param name="encryptKey">秘钥</param>
		/// <param name="hasEncrypt">已经加密</param>
		/// <param name="timeout">超时时间</param>
		/// <returns></returns>
		public static ApiResultInfo<T> ApiGate_Post<T>(string url, object requestObj, string encryptKey = "", bool hasEncrypt = false, int timeout = 0) where T : class
		{
			ApiResultInfo<T> apiResultInfo = new ApiResultInfo<T>
			{
				HasSuccess = true,
				RetCode = 0,
				RetMsg = string.Empty
			};
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					if (timeout > 0)
					{
						httpClient.Timeout = TimeSpan.FromMilliseconds((double)timeout);
					}
					StringContent content = new StringContent(requestObj.ToJson(), Encoding.UTF8, "application/json");
					if (hasEncrypt)
					{
						url = WebApiHelper.AppendEncryptToApiGate(url, requestObj.ToJson(), encryptKey);
					}
					ApiGateResult apiGateResult = httpClient.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result.ToObject<ApiGateResult>();
					if (apiGateResult.RetCode == 0)
					{
						apiResultInfo.InfoObj = apiGateResult.Message.ToObject<T>();
					}
					else
					{
						apiResultInfo.HasSuccess = false;
						apiResultInfo.RetCode = apiGateResult.RetCode;
						apiResultInfo.RetMsg = apiGateResult.RetMsg;
					}
				}
			}
			catch (Exception ex)
			{
				apiResultInfo.HasSuccess = false;
				apiResultInfo.RetCode = -1;
				apiResultInfo.RetMsg = ex.ToString();
			}
			return apiResultInfo;
		}
		/// <summary>
		/// 获取接口数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="url">Url地址</param>
		/// <param name="timeout">超时时间</param>
		/// <returns></returns>
		public static T Get<T>(string url, int timeout = 0) where T : class
		{
			T result = default(T);
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					if (timeout > 0)
					{
						httpClient.Timeout = TimeSpan.FromMilliseconds((double)timeout);
					}
					result = httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result.ToObject<T>();
				}
			}
			catch (Exception arg_5E_0)
			{
				throw arg_5E_0;
			}
			return result;
		}
		/// <summary>
		/// 获取接口数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="url">Url地址</param>
		/// <param name="requestObj"></param>
		/// <param name="timeout">超时时间</param>
		/// <returns></returns>
		public static T Post<T>(string url, object requestObj, int timeout = 0) where T : class
		{
			T result = default(T);
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					if (timeout > 0)
					{
						httpClient.Timeout = TimeSpan.FromMilliseconds((double)timeout);
					}
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					StringContent content = new StringContent(requestObj.ToJson(), Encoding.UTF8, "application/json");
					result = httpClient.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result.ToObject<T>();
				}
			}
			catch (Exception arg_8F_0)
			{
				throw arg_8F_0;
			}
			return result;
		}
		/// <summary>
		/// 拼接加密字符串到Url
		/// </summary>
		/// <param name="url">url</param>
		/// <param name="encryptKey">秘钥</param>
		/// <returns></returns>
		public static string AppendEncryptToWebApi(string url, string encryptKey)
		{
			Uri uri;
			if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
			{
				return url;
			}
			List<string> expr_34 = uri.Query.Split(new string[]
			{
				"?",
				"&"
			}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
			expr_34.Sort();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in expr_34)
			{
				stringBuilder.Append(current);
			}
			string str = stringBuilder.ToString() + encryptKey;
			return url + (url.Contains("?") ? "&" : "?") + "encrypt=" + str.GetMd5(32);
		}
		/// <summary>
		/// 拼接加密字符串到Url
		/// </summary>
		/// <param name="url"></param>
		/// <param name="md5Key"></param>
		/// <returns></returns>
		private static string AppendEncryptToApiGate(string url, string md5Key)
		{
			Uri uri;
			if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
			{
				return url;
			}
			List<string> expr_34 = uri.Query.Split(new string[]
			{
				"?",
				"&"
			}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
			expr_34.Sort();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in expr_34)
			{
				stringBuilder.Append(current);
			}
			string str = stringBuilder.ToString() + md5Key;
			return url + (url.Contains("?") ? "&" : "?") + "gate_encrypt=" + str.GetMd5(32);
		}
		/// <summary>
		/// 拼接加密字符串到Url
		/// </summary>
		/// <param name="url"></param>
		/// <param name="requestBody"></param>
		/// <param name="md5Key"></param>
		/// <returns></returns>
		private static string AppendEncryptToApiGate(string url, string requestBody, string md5Key)
		{
			Uri uri;
			if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
			{
				return url;
			}
			List<string> expr_34 = uri.Query.Split(new string[]
			{
				"?",
				"&"
			}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
			expr_34.Sort();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in expr_34)
			{
				stringBuilder.Append(current);
			}
			stringBuilder.Append(requestBody);
			string str = stringBuilder.ToString() + md5Key;
			return url + (url.Contains("?") ? "&" : "?") + "gate_encrypt=" + str.GetMd5(32);
		}
	}
}
