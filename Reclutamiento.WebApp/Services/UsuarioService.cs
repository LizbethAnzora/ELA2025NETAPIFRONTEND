using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Services
{
    public class UsuarioService : ApiServiceBase
    {
        public UsuarioService(HttpClient httpClient) : base(httpClient) { }

        public Task<List<UsuarioDto>> GetUsuariosAsync()
            => GetAsync<List<UsuarioDto>>("api/usuarios");

        public Task<UsuarioDto> GetUsuarioByIdAsync(int id)
            => GetAsync<UsuarioDto>($"api/usuarios/{id}");

        public Task<UsuarioDto> CrearUsuarioAsync(UsuarioDto usuario)
            => PostAsync<UsuarioDto>("api/usuarios", usuario);

        public Task<UsuarioDto> ActualizarUsuarioAsync(int id, UsuarioDto usuario)
            => PutAsync<UsuarioDto>($"api/usuarios/{id}", usuario);

        public Task EliminarUsuarioAsync(int id)
            => DeleteAsync($"api/usuarios/{id}");
    }
}
