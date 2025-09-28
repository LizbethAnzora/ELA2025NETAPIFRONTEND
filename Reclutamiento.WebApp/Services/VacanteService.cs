using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Models.Dtos;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class VacanteService
    {
        private readonly HttpClient _http;

        public VacanteService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<VacanteDto>> ObtenerVacantesAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<VacanteDto>>("api/Vacantes");
        }

        public async Task<VacanteDto> ObtenerVacantePorIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<VacanteDto>($"api/Vacantes/{id}");
        }

        public async Task<bool> CrearVacanteAsync(VacanteDto vacante)
        {
          
            var response = await _http.PostAsJsonAsync("api/Vacantes", vacante);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarVacanteAsync(int id, VacanteDto vacante)
        {
            var response = await _http.PutAsJsonAsync($"api/Vacantes/{id}", vacante);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarVacanteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Vacantes/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<SolicitudDto>> ObtenerSolicitudesPorVacanteAsync(int vacanteId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<SolicitudDto>>($"api/Vacantes/{vacanteId}/solicitudes");
        }
    }
}