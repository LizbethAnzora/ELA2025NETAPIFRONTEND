using System.Net.Http;
using System.Threading.Tasks;

namespace Reclutamiento.WebApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Llama al backend que a su vez maneja la integración con Supabase
        public async Task<string> LoginWithGithub()
        {
            var response = await _httpClient.GetAsync("auth/login/github");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task Logout()
        {
            var response = await _httpClient.PostAsync("auth/logout", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
