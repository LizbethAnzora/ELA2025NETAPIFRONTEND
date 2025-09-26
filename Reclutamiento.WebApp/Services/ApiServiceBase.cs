using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class ApiServiceBase
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOption;
        public ApiServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<List<T>> GetAllAsync<T>(string endpoint, string token = null)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<T>>(json, _jsonOption);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string token = null)
        {
            AddAuthorizationHeader(token);
            var content = new StringContent(JsonSerializer.Serialize(data, _jsonOption), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(json, _jsonOption);
        }

        public async Task<T> GetByIdAsync<T>(string endpoint, int id, string token = null)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{endpoint}/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOption);
        }
        
        public async Task<T> PutAsync<T>(string endpoint, T data, string token = null)
        {
            AddAuthorizationHeader(token);
            var content = new StringContent(JsonSerializer.Serialize(data, _jsonOption), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOption);
        }

        public async Task DeleteAsync(string endpoint, string token = null)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }

        private void AddAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            
        }
    }
}
