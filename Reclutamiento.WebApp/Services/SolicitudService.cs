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
            return await _http.GetFromJsonAsync<IEnumerable<SolicitudDto>>($"api/usuarios/{usuarioId}/Solicitudes");
        }
        

        public async Task<IEnumerable<SolicitudDto>> ObtenerSolicitudesPorVacanteAsync(int vacanteId)
        {
            
            return await _http.GetFromJsonAsync<IEnumerable<SolicitudDto>>($"api/Vacantes/{vacanteId}/solicitudes");
        }
        
       
        public async Task<SolicitudDto> ObtenerSolicitudPorIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<SolicitudDto>($"api/Solicitudes/{id}");
        }

        public async Task<bool> CrearSolicitudAsync(SolicitudCreateDto solicitud)
        {
            var response = await _http.PostAsJsonAsync("api/Solicitudes", solicitud);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> EnviarRespuestaAsync(int solicitudId, RespuestaCreateDto respuesta)
        {
            var response = await _http.PostAsJsonAsync($"api/Solicitudes/{solicitudId}/respuesta", respuesta);
            return response.IsSuccessStatusCode;
        }

       
        public async Task<bool> EditarSolicitudAsync(int id, SolicitudDto solicitud)
        {
            var response = await _http.PutAsJsonAsync($"api/Solicitudes/{id}", solicitud);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarSolicitudAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Solicitudes/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}