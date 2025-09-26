using System.Net.Http;
using System.Threading.Tasks;
using FrontAuth.WebApp.DTOs.UsuarioDTOs;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using ReclutamientoFrontend.WebApp.Services;

namespace Reclutamiento.WebApp.Services
{
    public class AuthService 
    {
        private readonly ApiServiceBase _apiServiceBase;
        public AuthService(ApiServiceBase ApiServiceBase)
        {
            _apiServiceBase = ApiServiceBase;
        
        }

        public async Task<LoginResponseDTO> LoginAsync(UsuarioDto dto)
        {
            return await _apiServiceBase.PostAsync<UsuarioDto, LoginResponseDTO>("/api/Auth/login/admin", dto);

        }
        
        public async Task<LoginResponseDTO> RegisterAsync(UsuarioDto dto)
        {
            return await _apiServiceBase.PostAsync<UsuarioDto, LoginResponseDTO>("auth/register", dto);
        }



        // Llama al backend que a su vez maneja la integración con Supabase
        public async Task<string> LoginWithGithub()
        {
            var id = 0;
            var response = await _apiServiceBase.GetByIdAsync<HttpResponseMessage>("auth/login/github", id);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
