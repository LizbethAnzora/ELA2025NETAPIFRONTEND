using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Models.Dtos;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class SolicitudService
    {
        private readonly HttpClient _http;

        public SolicitudService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<SolicitudDto>> ObtenerSolicitudesDelUsuarioAsync(int usuarioId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<SolicitudDto>>($"api/usuarios/{usuarioId}/solicitudes");
        }

        public async Task<SolicitudDto> ObtenerSolicitudPorIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<SolicitudDto>($"api/solicitudes/{id}");
        }

        public async Task<bool> CrearSolicitudAsync(SolicitudDto solicitud)
        {
            var response = await _http.PostAsJsonAsync("api/solicitudes", solicitud);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarSolicitudAsync(int id, SolicitudDto solicitud)
        {
            var response = await _http.PutAsJsonAsync($"api/solicitudes/{id}", solicitud);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarSolicitudAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/solicitudes/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
