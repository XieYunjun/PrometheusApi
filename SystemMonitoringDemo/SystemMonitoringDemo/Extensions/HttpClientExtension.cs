using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SystemMonitoringDemo.Extensions
{
    /// <summary>
    /// IHttpClientExtension
    /// </summary>
    public interface IHttpClientExtension
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T?> GetAsync<T>(string url, Dictionary<string, string> dic);

        /// <summary>
        /// 检测地址是否能访问
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<bool> CheckAddressAsync(string url);
    }

    /// <summary>
    /// HttpClientExtension
    /// </summary>
    public class HttpClientExtension : IHttpClientExtension
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// HttpClientExtension
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public HttpClientExtension(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T?> GetAsync<T>(string url, Dictionary<string, string> dic)
        {
            var query = QueryData(dic);

            url += query;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = _httpClientFactory.CreateClient();

            var httpReponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpReponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpReponseMessage.Content.ReadAsStreamAsync();

                var entity = await JsonSerializer.DeserializeAsync<T>(contentStream);

                return entity;
            }

            return default(T);
        }

        private string QueryData(Dictionary<string, string> dic)
        {
            var res = new StringBuilder();

            foreach (var key in dic.Keys)
            {
                if (dic.TryGetValue(key, out string? value))
                {
                    var val = System.Web.HttpUtility.UrlEncode(value, System.Text.Encoding.UTF8);
                    res.Append($"{key}={val}&");
                }
            }

            return res.ToString().Trim('&');
        }

        /// <summary>
        /// 检测ip地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> CheckAddressAsync(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = _httpClientFactory.CreateClient();

            var httpReponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpReponseMessage.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
