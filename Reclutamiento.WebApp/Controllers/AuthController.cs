using FrontAuth.WebApp.DTOs.UsuarioDTOs;
using FrontAuth.WebApp.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Reclutamiento.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
                return View();
            }

            var principal = ClaimsHelper.CrearClaimsPrincipal(result);
            await HttpContext.SignInAsync("AuthCookie", principal);
            return RedirectToAction("Index", "Home");
            
        }


        [HttpGet]
        public IActionResult LoginWithGitHub()
        {
            var redirectUrl = Url.Action("GitHubCallback", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "GitHub");  
        }


        [HttpGet]
        public async Task<IActionResult> GitHubCallback()
        {
            //  Obtener los datos del esquema GitHub
            var authenticateResult = await HttpContext.AuthenticateAsync("GitHub");

            if (!authenticateResult.Succeeded)
                return RedirectToAction("Login");

            var user = authenticateResult.Principal;
            var email = user.FindFirstValue(ClaimTypes.Email);
            var name = user.Identity?.Name ?? "GitHub User";

            // Aquí  registrar o autenticar en la base de datos
            // var usuarioDto = await _authService.LoginOrRegisterExternalAsync(email, name);
            var usuarioDto = new LoginResponseDTO
            {
                Id = 1,
                NombreCompleto = name,
                CorreoElectronico = email,
            };

            var principal = ClaimsHelper.CrearClaimsPrincipal(usuarioDto);
            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Limpiar sesión en frontend
            HttpContext.Session.Clear();
            // Cerrar cookie de autenticación
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Login", "Auth");
        }
    }
}
