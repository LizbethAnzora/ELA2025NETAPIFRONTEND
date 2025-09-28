using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Models.Dtos; // Usando el VacanteDto ajustado

namespace ReclutamientoFrontend.WebApp.Services
{
    public class VacanteService
    {
        private readonly HttpClient _http;

        public VacanteService(HttpClient http)
        {
            _http = http;
        }

        // 1. Obtiene TODAS las vacantes (para la Index del Administrador)
        public async Task<IEnumerable<VacanteDto>> ObtenerVacantesAdminAsync()
        {
            // Endpoint: /api/Vacantes (GET)
            return await _http.GetFromJsonAsync<IEnumerable<VacanteDto>>("api/Vacantes");
        }
        
        // 2. NUEVO: Obtiene SOLO las vacantes activas (para la Home del Solicitante)
        public async Task<IEnumerable<VacanteDto>> ObtenerVacantesPublicasAsync()
        {
            // Endpoint: /api/Vacantes/publico (GET)
            // Este endpoint debe devolver solo vacantes donde EstaActiva = true
            return await _http.GetFromJsonAsync<IEnumerable<VacanteDto>>("api/Vacantes/publico");
        }

        public async Task<VacanteDto> ObtenerVacantePorIdAsync(int id)
        {
            // Endpoint: /api/Vacantes/{id} (GET)
            return await _http.GetFromJsonAsync<VacanteDto>($"api/Vacantes/{id}");
        }

        public async Task<bool> CrearVacanteAsync(VacanteDto vacante)
        {
            // Endpoint: /api/Vacantes (POST)
            // En el Controller nos aseguraremos de setear EstaActiva = true por defecto al crear.
            var response = await _http.PostAsJsonAsync("api/Vacantes", vacante);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarVacanteAsync(int id, VacanteDto vacante)
        {
            // Endpoint: /api/Vacantes/{id} (PUT)
            var response = await _http.PutAsJsonAsync($"api/Vacantes/{id}", vacante);
            return response.IsSuccessStatusCode;
        }

        // 3. RENOMBRADO/AJUSTADO: Usa el DELETE del backend para Deshabilitar (poner en Inactivo)
        public async Task<bool> DeshabilitarVacanteAsync(int id)
        {
            // Endpoint: /api/Vacantes/{id} (DELETE)
            var response = await _http.DeleteAsync($"api/Vacantes/{id}");
            return response.IsSuccessStatusCode;
        }
        
        // 4. NUEVO: Permite Habilitar una vacante usando el PUT, si el backend lo requiere.
        // Si tu backend permite Habilitar una vacante con un PUT enviando estaActiva: true, 
        // puedes usar este método. Sin embargo, dado que el DELETE deshabilita, 
        // puede que el backend espere que el PUT sea usado para volver a ACTIVAR.
        // Asumiremos que el PUT (EditarVacanteAsync) puede usarse para re-activar si se envía EstaActiva = true.
        // Por simplicidad, nos enfocaremos en Deshabilitar por ahora, que es lo que pediste explícitamente.

        public async Task<IEnumerable<SolicitudDto>> ObtenerSolicitudesPorVacanteAsync(int vacanteId)
        {
            // Endpoint: /api/Vacantes/{vacanteId}/solicitudes (GET)
            // NOTA: Asegúrate de tener la clase SolicitudDto
            return await _http.GetFromJsonAsync<IEnumerable<SolicitudDto>>($"api/Vacantes/{vacanteId}/solicitudes");
        }
    }
}