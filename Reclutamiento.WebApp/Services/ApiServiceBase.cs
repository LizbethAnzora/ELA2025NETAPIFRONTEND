using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ReclutamientoFrontend.WebApp.Services
{
    public abstract class ApiServiceBase
    {
        protected readonly HttpClient _httpClient;

        protected ApiServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task<T> PostAsync<T>(string url, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task<T> PutAsync<T>(string url, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
