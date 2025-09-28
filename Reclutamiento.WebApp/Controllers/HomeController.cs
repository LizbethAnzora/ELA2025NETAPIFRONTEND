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
            // Nota: Ya que [Authorize] está arriba, no necesitamos verificar IsAuthenticated aquí.
            // Si el usuario llega aquí, está autenticado como Admin o Solicitante.
            
            if (User.IsInRole("Admin"))
            {
                // Si es Admin, el Home muestra el mensaje de bienvenida
                // (Se asume que la vista Home Index maneja este caso con un modelo vacío o nulo)
                return View(new List<VacanteDto>()); 
            }
            else
            {
                // Si es Solicitante, utilizamos el método para obtener SOLO las vacantes activas (públicas)
                var vacantes = await _vacanteService.ObtenerVacantesPublicasAsync(); 
                return View(vacantes);
            }
        }
    }
}