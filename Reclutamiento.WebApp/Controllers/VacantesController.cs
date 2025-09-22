using Microsoft.AspNetCore.Mvc;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class VacantesController : Controller
    {
        private readonly VacanteService _vacanteService;

        public VacantesController(VacanteService vacanteService)
        {
            _vacanteService = vacanteService;
        }

        public async Task<IActionResult> Index(string filtroTitulo, string filtroAdmin)
        {
            var vacantes = await _vacanteService.ObtenerVacantesAsync(filtroTitulo, filtroAdmin);
            return View(vacantes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VacanteDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var creado = await _vacanteService.CrearVacanteAsync(dto);
            if (creado)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo crear la vacante.");
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            if (vacante == null) return NotFound();

            return View(vacante);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, VacanteDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var actualizado = await _vacanteService.EditarVacanteAsync(id, dto);
            if (actualizado)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo actualizar la vacante.");
            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            if (vacante == null) return NotFound();

            return View(vacante);
        }

        public async Task<IActionResult> Solicitudes(int id)
        {
            var solicitudes = await _vacanteService.ObtenerSolicitudesPorVacanteAsync(id);
            return View(solicitudes);
        }
    }
}