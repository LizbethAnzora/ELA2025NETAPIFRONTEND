using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class VacantesController : Controller
    {
        private readonly VacanteService _vacanteService;

        public VacantesController(VacanteService vacanteService)
        {
            _vacanteService = vacanteService;
        }

        public async Task<IActionResult> Index()
        {
            var vacantes = await _vacanteService.ObtenerVacantesAsync();
            return View(vacantes);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            if (vacante == null) return NotFound();
            return View(vacante);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(VacanteDto vacante)
        {
            if (!ModelState.IsValid) return View("Create",vacante);

            var ok = await _vacanteService.CrearVacanteAsync(vacante);
            if (ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo crear la vacante.");
            return View("Create",vacante);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            if (vacante == null) return NotFound();
            return View("Edit",vacante);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, VacanteDto vacante)
        {
            if (!ModelState.IsValid) return View("Edit",vacante);

            var ok = await _vacanteService.EditarVacanteAsync(id, vacante);
            if (ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo editar la vacante.");
            return View("Edit",vacante);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var ok = await _vacanteService.EliminarVacanteAsync(id);
            if (!ok) return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Solicitudes(int vacanteId)
        {
            var solicitudes = await _vacanteService.ObtenerSolicitudesPorVacanteAsync(vacanteId);
            return View(solicitudes);
        }
    }
}