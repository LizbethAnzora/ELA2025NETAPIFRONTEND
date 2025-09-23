using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class RespuestaService : ApiServiceBase
    {
        public RespuestaService(HttpClient httpClient) : base(httpClient) { }

        public Task<List<RespuestaDto>> GetRespuestasAsync()
            => GetAsync<List<RespuestaDto>>("api/respuestas");

        public Task<RespuestaDto> GetRespuestaByIdAsync(int id)
            => GetAsync<RespuestaDto>($"api/respuestas/{id}");

        public Task<RespuestaDto> CrearRespuestaAsync(RespuestaDto respuesta)
            => PostAsync<RespuestaDto>("api/respuestas", respuesta);

        public Task<RespuestaDto> ActualizarRespuestaAsync(int id, RespuestaDto respuesta)
            => PutAsync<RespuestaDto>($"api/respuestas/{id}", respuesta);

        public Task EliminarRespuestaAsync(int id)
            => DeleteAsync($"api/respuestas/{id}");
    }
}
