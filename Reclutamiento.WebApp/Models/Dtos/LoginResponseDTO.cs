using ReclutamientoFrontend.WebApp.Models.Dtos;

namespace FrontAuth.WebApp.DTOs.UsuarioDTOs
{
    public class LoginResponseDTO : UsuarioDto
    {
    public string? Token { get; set; }
    }
}
