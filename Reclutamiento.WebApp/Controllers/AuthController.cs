using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Reclutamiento.WebApp.Services;
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

        [HttpGet]
        public IActionResult Login()
        {
            // Pasamos la configuración de Supabase a la vista
            ViewData["SupabaseUrl"] = _config["Supabase:Url"];
            ViewData["SupabaseAnonKey"] = _config["Supabase:AnonKey"];
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoginWithGithub()
        {
            // Llamamos al servicio que conecta con el backend (Supabase via API)
            var result = await _authService.LoginWithGithub();

            if (!string.IsNullOrEmpty(result))
            {
                // Guardar token en sesión o cookie (dependiendo de tu backend)
                HttpContext.Session.SetString("AuthToken", result);
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "No se pudo iniciar sesión con GitHub.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Avisamos al backend que cierre sesión
            await _authService.Logout();

            // Limpiar sesión en frontend
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
