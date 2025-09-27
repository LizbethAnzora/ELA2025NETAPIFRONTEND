using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReclutamientoFrontend.WebApp.Services;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly VacanteService _vacanteService;

        public HomeController(VacanteService vacanteService)
        {
            _vacanteService = vacanteService;
        }

        public async Task<IActionResult> Index()
        {
            if (!(User?.Identity?.IsAuthenticated ?? false))
            {
                return RedirectToAction("Login", "Auth");
            }
            var vacantes = await _vacanteService.ObtenerVacantesAsync();
            return View(vacantes);
        }
    }
}