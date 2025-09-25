using FrontAuth.WebApp.DTOs.UsuarioDTOs;
using FrontAuth.WebApp.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Reclutamiento.WebApp.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration config, IAuthService authService)
        {
            _config = config;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Login()
        {
            // Pasamos la configuración de Supabase a la vista
            ViewData["SupabaseUrl"] = _config["Supabase:Url"];
            ViewData["SupabaseAnonKey"] = _config["Supabase:AnonKey"];
            return View();
        }


        [HttpPost]
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
                Nombre = name,
                Email = email
            };

            var principal = ClaimsHelper.CrearClaimsPrincipal(usuarioDto);
            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Home/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Avisamos al backend que cierre sesión
            await _authService.Logout();

            // Limpiar sesión en frontend
            HttpContext.Session.Clear();

            return RedirectToAction("Home/Index");
        }
    }
}
