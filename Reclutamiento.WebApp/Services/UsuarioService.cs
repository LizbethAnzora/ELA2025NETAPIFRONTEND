using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Models.Dtos;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _http;

        public UsuarioService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerUsuariosAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<UsuarioDto>>("api/usuarios");
        }

        public async Task<UsuarioDto> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<UsuarioDto>($"api/usuarios/{id}");
        }

        public async Task<bool> CrearUsuarioAsync(UsuarioDto usuario)
        {
            var response = await _http.PostAsJsonAsync("api/usuarios", usuario);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarUsuarioAsync(int id, UsuarioDto usuario)
        {
            var response = await _http.PutAsJsonAsync($"api/usuarios/{id}", usuario);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/usuarios/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
