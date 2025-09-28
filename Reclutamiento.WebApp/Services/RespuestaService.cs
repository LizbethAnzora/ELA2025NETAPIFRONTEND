using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class RespuestaService
    {
        private readonly ApiServiceBase _apiServiceBase;
        public RespuestaService(ApiServiceBase apiServiceBase)
        { 
            _apiServiceBase = apiServiceBase;
        }

        public Task<List<RespuestaDto>> GetRespuestasAsync()
            => _apiServiceBase.GetAllAsync<RespuestaDto>("api/respuestas");

        public Task<RespuestaDto> GetRespuestaByIdAsync(int id)
            => _apiServiceBase.GetByIdAsync<RespuestaDto>($"api/respuestas/{id}", id, null);

        public Task<RespuestaDto> CrearRespuestaAsync(RespuestaDto respuesta)
            => _apiServiceBase.PostAsync<RespuestaDto, RespuestaDto>("api/respuestas", respuesta, null);

        public Task<RespuestaDto> ActualizarRespuestaAsync(int id, RespuestaDto respuesta)
            => _apiServiceBase.PutAsync<RespuestaDto>($"api/respuestas/{id}", respuesta);

        public Task EliminarRespuestaAsync(int id)
            => _apiServiceBase.DeleteAsync($"api/respuestas/{id}");
    }
}