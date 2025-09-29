// SolicitudService.cs (Proyecto Frontend)

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

        // Obtiene todas las solicitudes enviadas por un usuario solicitante específico.
        public async Task<IEnumerable<SolicitudDto>> ObtenerSolicitudesDelUsuarioAsync(int usuarioId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<SolicitudDto>>($"api/usuarios/{usuarioId}/Solicitudes");
        }
        
        // ✅ MÉTODO COMPLETO: Llama al endpoint confirmado del backend API
        public async Task<IEnumerable<SolicitudDto>> ObtenerSolicitudesPorVacanteAsync(int vacanteId)
        {
            // El endpoint correcto es: api/Vacantes/{vacanteId}/solicitudes
            return await _http.GetFromJsonAsync<IEnumerable<SolicitudDto>>($"api/Vacantes/{vacanteId}/solicitudes");
        }
        
        // Obtiene los detalles de una solicitud específica.
        public async Task<SolicitudDto> ObtenerSolicitudPorIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<SolicitudDto>($"api/Solicitudes/{id}");
        }

        // El DTO de creación contiene los campos que el endpoint POST espera.
        public async Task<bool> CrearSolicitudAsync(SolicitudCreateDto solicitud)
        {
            var response = await _http.PostAsJsonAsync("api/Solicitudes", solicitud);
            return response.IsSuccessStatusCode;
        }

        // Método para enviar respuesta (nombre y endpoint correctos).
        public async Task<bool> EnviarRespuestaAsync(int solicitudId, RespuestaCreateDto respuesta)
        {
            var response = await _http.PostAsJsonAsync($"api/Solicitudes/{solicitudId}/respuesta", respuesta);
            return response.IsSuccessStatusCode;
        }

        // Se mantienen las funciones de edición y eliminación, aunque no se usarán directamente en los requisitos actuales.
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