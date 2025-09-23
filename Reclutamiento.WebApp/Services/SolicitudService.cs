using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class SolicitudService : ApiServiceBase
    {
        public SolicitudService(HttpClient httpClient) : base(httpClient) { }

        public Task<List<SolicitudDto>> GetSolicitudesAsync()
            => GetAsync<List<SolicitudDto>>("api/solicitudes");

        public Task<SolicitudDto> GetSolicitudByIdAsync(int id)
            => GetAsync<SolicitudDto>($"api/solicitudes/{id}");

        public Task<SolicitudDto> CrearSolicitudAsync(SolicitudDto solicitud)
            => PostAsync<SolicitudDto>("api/solicitudes", solicitud);

        public Task<SolicitudDto> ActualizarSolicitudAsync(int id, SolicitudDto solicitud)
            => PutAsync<SolicitudDto>($"api/solicitudes/{id}", solicitud);

        public Task EliminarSolicitudAsync(int id)
            => DeleteAsync($"api/solicitudes/{id}");
    }
}
