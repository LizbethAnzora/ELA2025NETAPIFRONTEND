using Microsoft.AspNetCore.Mvc;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly SolicitudService _solicitudService;

        public SolicitudesController(SolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        public async Task<IActionResult> Index()
        {
            var solicitudes = await _solicitudService.ObtenerSolicitudesDelUsuarioAsync();
            return View(solicitudes);
        }

        public IActionResult Create(int vacanteId)
        {
            var model = new SolicitudDto { VacanteId = vacanteId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SolicitudDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var creada = await _solicitudService.CrearSolicitudAsync(dto);
            if (creada)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo enviar la solicitud.");
            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var solicitud = await _solicitudService.ObtenerSolicitudPorIdAsync(id);
            if (solicitud == null) return NotFound();

            return View(solicitud);
        }
    }
}