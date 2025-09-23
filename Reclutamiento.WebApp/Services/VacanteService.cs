using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class VacanteService : ApiServiceBase
    {
        public VacanteService(HttpClient httpClient) : base(httpClient) { }

        public Task<List<VacanteDto>> GetVacantesAsync()
            => GetAsync<List<VacanteDto>>("api/vacantes");

        public Task<VacanteDto> GetVacanteByIdAsync(int id)
            => GetAsync<VacanteDto>($"api/vacantes/{id}");

        public Task<VacanteDto> CrearVacanteAsync(VacanteDto vacante)
            => PostAsync<VacanteDto>("api/vacantes", vacante);

        public Task<VacanteDto> ActualizarVacanteAsync(int id, VacanteDto vacante)
            => PutAsync<VacanteDto>($"api/vacantes/{id}", vacante);

        public Task EliminarVacanteAsync(int id)
            => DeleteAsync($"api/vacantes/{id}");
    }
}
