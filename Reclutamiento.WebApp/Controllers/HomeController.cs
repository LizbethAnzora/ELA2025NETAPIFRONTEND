using Microsoft.AspNetCore.Mvc;
using ReclutamientoFrontend.WebApp.Services;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly VacanteService _vacanteService;

        public HomeController(VacanteService vacanteService)
        {
            _vacanteService = vacanteService;
        }

        public async Task<IActionResult> Index()
        {
            var vacantes = await _vacanteService.ObtenerVacantesAsync();
            return View(vacantes);
        }
    }
}