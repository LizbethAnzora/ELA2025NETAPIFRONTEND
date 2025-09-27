using FrontAuth.WebApp.DTOs.UsuarioDTOs;
using FrontAuth.WebApp.Helpers;
using System.Security.Claims;

namespace FrontAuth.WebApp.Helpers
{
    public static class ClaimsHelper
    {
        public static ClaimsPrincipal CrearClaimsPrincipal(LoginResponseDTO usuario)
        {
            // Código de depuración eliminado para evitar errores en ASP.NET Core
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? "Usuario"), // Nombre completo
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico ?? string.Empty),
                new Claim(ClaimTypes.Role, RoleHelper.GetRoleName(usuario.Rol)),
                new Claim("Token", usuario.Token ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            return new ClaimsPrincipal(identity);
        }
    }
}
