
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using ReclutamientoFrontend.WebApp.Services;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    [Authorize] // Se mantiene la autorización general, confiando en tu lógica de Login
    public class HomeController : Controller
    {
        private readonly VacanteService _vacanteService;

        public HomeController(VacanteService vacanteService)
        {
            _vacanteService = vacanteService;
        }

        public async Task<IActionResult> Index()
        {
            
            if (User.IsInRole("Admin"))
            {
                
                return View(new List<VacanteDto>()); 
            }
            else
            {
                
                var vacantes = await _vacanteService.ObtenerVacantesPublicasAsync(); 
                return View(vacantes);
            }
        }
    }
}