using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Mmcoy.Framework.WebAPIAuth
{
    public class MFWebAPIHttpClient
    {
        /// <summary>
        /// 将参数附加到Url上
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramObj"></param>
        /// <returns></returns>
        private static string BuildUrl(string url, object paramObj)
        {
            var dict = ObjectToDictionary(paramObj);
            var strBuild = new StringBuilder();
            foreach (var key in dict.Keys)
            {
                strBuild.Append("&").AppendFormat("{0}={1}", key, dict[key]);
            }
            if (strBuild.Length > 0)
            {
                if (url.IndexOf("?") == -1)
                {
                    strBuild = strBuild.Remove(0, 1);
                    url += "?" + strBuild.ToString();
                }
                else
                {
                    if (url.Last() == '&')
                        url += strBuild.ToString();
                }
            }
            return url;
        }

        /// <summary>
        /// 未包装返回值的WebAPI的Get接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="url">WebAPI的请求地址</param>
        /// <returns></returns>
        public static T GetWebAPIValueNotBackVal<T>(string url, object paramObj = null)
        {
            if (!paramObj.IsNullOrEmpty())
            {
                url = BuildUrl(url, paramObj);
            }
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var jsonModel = client.GetStringAsync(url).Result;
            var backVal = JsonConvert.DeserializeObject<T>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// 异步WebAPI的Get接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="url">WebAPI的请求地址</param>
        /// <returns></returns>
        public static async Task<MFReturnValue<T>> GetWebAPIValueAsync<T>(string url, object paramObj = null)
        {
            if (!paramObj.IsNullOrEmpty())
            {
                url = BuildUrl(url, paramObj);
            }
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var jsonModel = await client.GetStringAsync(url);
            var backVal = await JsonConvert.DeserializeObjectAsync<MFReturnValue<T>>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// WebAPI的Get接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <returns></returns>
        public static MFReturnValue<T> GetWebAPIValue<T>(string url, object paramObj = null)
        {
            if (!paramObj.IsNullOrEmpty())
            {
                url = BuildUrl(url, paramObj);
            }
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var jsonModel = client.GetStringAsync(url).Result;
            var backVal = JsonConvert.DeserializeObject<MFReturnValue<T>>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// WebAPI异步的Post接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <param name="param">请求参数字典</param>
        /// <returns></returns>
        public static async Task<MFReturnValue<T>> PostWebAPIValueAsync<T>(string url, object param)
        {
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var postVal = new JObject();
            var paramDict = ObjectToDictionary(param);
            foreach (var key in paramDict.Keys)
            {
                postVal.Add(key, JToken.FromObject(paramDict[key]));
            }
            var paramContent = new StringContent(JsonConvert.SerializeObject(postVal));
            paramContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var respMsg = await client.PostAsync(url, paramContent);
            var jsonModel = await respMsg.Content.ReadAsStringAsync();
            var backVal = await JsonConvert.DeserializeObjectAsync<MFReturnValue<T>>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// 未包装返回值的WebAPI的Post接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <param name="param">请求参数字典</param>
        /// <returns></returns>
        public static T PostWebAPIValueNotBackVal<T>(string url, object param)
        {
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var postVal = new JObject();
            var paramDict = ObjectToDictionary(param);
            foreach (var key in paramDict.Keys)
            {
                postVal.Add(key, JToken.FromObject(paramDict[key]));
            }
            var paramContent = new StringContent(JsonConvert.SerializeObject(postVal));
            paramContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var jsonModel = client.PostAsync(url, paramContent).Result.Content.ReadAsStringAsync().Result;
            var backVal = JsonConvert.DeserializeObject<T>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// WebAPI的Post接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <param name="param">请求参数字典</param>
        /// <returns></returns>
        public static MFReturnValue<T> PostWebAPIValue<T>(string url, object param)
        {
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var postVal = new JObject();
            var paramDict = ObjectToDictionary(param);
            foreach (var key in paramDict.Keys)
            {
                postVal.Add(key, JToken.FromObject(paramDict[key]));
            }
            var paramContent = new StringContent(JsonConvert.SerializeObject(postVal));
            paramContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var jsonModel = client.PostAsync(url, paramContent).Result.Content.ReadAsStringAsync().Result;
            var backVal = JsonConvert.DeserializeObject<MFReturnValue<T>>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// WebAPI异步无返回值的Post接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        public static void PostWebAPIAsync(string url, object param)
        {
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var postVal = new JObject();
            var paramDict = ObjectToDictionary(param);
            foreach (var key in paramDict.Keys)
            {
                postVal.Add(key, JToken.FromObject(paramDict[key]));
            }
            var paramContent = new StringContent(JsonConvert.SerializeObject(postVal));
            paramContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            Task.Run(() => client.PostAsync(url, paramContent));
        }

        /// <summary>
        /// WebAPI异步的Put接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <param name="param">请求参数字典</param>
        /// <returns></returns>
        public static async Task<MFReturnValue<T>> PutWebAPIValueAsync<T>(string url, object param)
        {
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var postVal = new JObject();
            var paramDict = ObjectToDictionary(param);
            foreach (var key in paramDict.Keys)
            {
                postVal.Add(key, JToken.FromObject(paramDict[key]));
            }
            var paramContent = new StringContent(JsonConvert.SerializeObject(postVal));
            paramContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var respMsg = await client.PutAsync(url, paramContent);
            var jsonModel = await respMsg.Content.ReadAsStringAsync();
            var backVal = await JsonConvert.DeserializeObjectAsync<MFReturnValue<T>>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// WebAPI的Put接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <param name="param">请求参数字典</param>
        /// <returns></returns>
        public static MFReturnValue<T> PutWebAPIValue<T>(string url, object param)
        {
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var postVal = new JObject();
            var paramDict = ObjectToDictionary(param);
            foreach (var key in paramDict.Keys)
            {
                postVal.Add(key, JToken.FromObject(paramDict[key]));
            }
            var paramContent = new StringContent(JsonConvert.SerializeObject(postVal));
            paramContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var jsonModel = client.PutAsync(url, paramContent).Result.Content.ReadAsStringAsync().Result;
            var backVal = JsonConvert.DeserializeObject<MFReturnValue<T>>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// 未包装返回值的WebAPI的Put接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <param name="param">请求参数字典</param>
        /// <returns></returns>
        public static T PutWebAPIValueNotBackVal<T>(string url, object param)
        {
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var postVal = new JObject();
            var paramDict = ObjectToDictionary(param);
            foreach (var key in paramDict.Keys)
            {
                postVal.Add(key, JToken.FromObject(paramDict[key]));
            }
            var paramContent = new StringContent(JsonConvert.SerializeObject(postVal));
            paramContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var jsonModel = client.PutAsync(url, paramContent).Result.Content.ReadAsStringAsync().Result;
            var backVal = JsonConvert.DeserializeObject<T>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// WebAPI异步无返回值的Put接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        public static void PutWebAPIAsync(string url, object param)
        {
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var postVal = new JObject();
            var paramDict = ObjectToDictionary(param);
            foreach (var key in paramDict.Keys)
            {
                postVal.Add(key, JToken.FromObject(paramDict[key]));
            }
            var paramContent = new StringContent(JsonConvert.SerializeObject(postVal));
            paramContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            Task.Run(() => client.PutAsync(url, paramContent));
        }

        /// <summary>
        /// WebAPI异步的Delete接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <returns></returns>
        public static async Task<MFReturnValue<T>> DeleteWebAPIValueAsync<T>(string url, object paramObj = null)
        {
            if (!paramObj.IsNullOrEmpty())
            {
                url = BuildUrl(url, paramObj);
            }
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var respMsg = await client.DeleteAsync(url);
            var jsonModel = await respMsg.Content.ReadAsStringAsync();
            var backVal = await JsonConvert.DeserializeObjectAsync<MFReturnValue<T>>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// WebAPI的Delete接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <returns></returns>
        public static MFReturnValue<T> DeleteWebAPIValue<T>(string url, object paramObj = null)
        {
            if (!paramObj.IsNullOrEmpty())
            {
                url = BuildUrl(url, paramObj);
            }
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var respMsg = client.DeleteAsync(url).Result;
            var jsonModel = respMsg.Content.ReadAsStringAsync().Result;
            var backVal = JsonConvert.DeserializeObject<MFReturnValue<T>>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// 未包装返回值的WebAPI的Delete接口
        /// </summary>
        /// <typeparam name="T">返回Json格式的数据对象(JArray或JObject)</typeparam>
        /// <param name="controller"></param>
        /// <param name="url">WebAPI的请求地址</param>
        /// <returns></returns>
        public static T DeleteWebAPIValueNotBackVal<T>(string url, object paramObj = null)
        {
            if (!paramObj.IsNullOrEmpty())
            {
                url = BuildUrl(url, paramObj);
            }
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            var respMsg = client.DeleteAsync(url).Result;
            var jsonModel = respMsg.Content.ReadAsStringAsync().Result;
            var backVal = JsonConvert.DeserializeObject<T>(jsonModel);
            return backVal;
        }

        /// <summary>
        /// WebAPI异步无返回值的Delete接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramObj"></param>
        public static async void DeleteWebAPIAsync(string url, object paramObj = null)
        {
            if (!paramObj.IsNullOrEmpty())
            {
                url = BuildUrl(url, paramObj);
            }
            var client = new HttpClient(new MFClientAuthRequestHandler() { InnerHandler = new HttpClientHandler() });
            await client.DeleteAsync(url);
        }

        /// <summary>
        /// 将字典或匿名类转换成字典类型
        /// </summary>
        /// <param name="paramObj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ObjectToDictionary(object paramObj)
        {
            IDictionary<string, object> valueDictionary = null;
            if (paramObj is IDictionary<string, object>)
                valueDictionary = paramObj as IDictionary<string, object>;
            else
                valueDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(paramObj);
            return valueDictionary;
        }
    }
}