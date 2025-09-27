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
            try
            {
                var result = await _http.GetFromJsonAsync<IEnumerable<UsuarioDto>>("api/Usuarios/admins");
                return result ?? new List<UsuarioDto>();
            }
            catch (HttpRequestException)
            {
                return new List<UsuarioDto>();
            }
        }

        public async Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<UsuarioDto>($"api/Usuarios/admins/{id}");
                return result ?? null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<bool> CrearUsuarioAsync(UsuarioDto usuario)
        {
            var body = new {
                nombreCompleto = usuario.NombreCompleto,
                correoElectronico = usuario.CorreoElectronico,
                contrasena = usuario.Contrasena
            };
            var response = await _http.PostAsJsonAsync("api/Usuarios/admins", body);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarUsuarioAsync(int id, UsuarioDto usuario)
        {
            var body = new {
                nombreCompleto = usuario.NombreCompleto
            };
            var response = await _http.PutAsJsonAsync($"api/Usuarios/admins/{id}", body);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Usuarios/admins/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
