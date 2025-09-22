using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["SupabaseUrl"] = _config["Supabase:Url"];
            ViewData["SupabaseAnonKey"] = _config["Supabase:AnonKey"];
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Eliminar cookies de sesión
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}