using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SystemMonitoringDemo.Extensions
{

    public interface IHttpClientExtension
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<T?> GetAsync<T>(string url, Dictionary<string, string> dic);
    }

    public class HttpClientExtension : IHttpClientExtension
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientExtension(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


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
    }
}
