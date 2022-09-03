using newMantis.Configuration;
using System.Net.Http.Headers;

namespace newMantis
{
    public class AccessServer : IDisposable
    {
        private HttpClient _httpClient = new HttpClient();
        private readonly IMantisConfigManager _configuration;

        public AccessServer(IMantisConfigManager configuration)
        {
            this._configuration = configuration;
            string baseUrl = configuration.Uri;
            string authorization = configuration.Authorization;
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", authorization);
            _httpClient.DefaultRequestHeaders.Add("User-agent", "Celios 0.1");
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<String> callMethodAsync(string urlParameters)
        {
            HttpResponseMessage res = await _httpClient.GetAsync(urlParameters);
            String jsonString;
            if (res.IsSuccessStatusCode)
            {
                jsonString = await res.Content.ReadAsStringAsync();
                return jsonString;
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _httpClient = null;
        }
    }
}