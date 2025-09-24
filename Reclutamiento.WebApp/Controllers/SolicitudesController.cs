using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly SolicitudService _solicitudService;

        public SolicitudesController(SolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var solicitud = await _solicitudService.ObtenerSolicitudPorIdAsync(id);
            if (solicitud == null) return NotFound();
            return View(solicitud);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SolicitudDto solicitud)
        {
            if (!ModelState.IsValid) return View(solicitud);

            var ok = await _solicitudService.CrearSolicitudAsync(solicitud);
            if (ok) return RedirectToAction("Index", "Vacantes");

            ModelState.AddModelError("", "No se pudo crear la solicitud.");
            return View(solicitud);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var solicitud = await _solicitudService.ObtenerSolicitudPorIdAsync(id);
            if (solicitud == null) return NotFound();
            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, SolicitudDto solicitud)
        {
            if (!ModelState.IsValid) return View(solicitud);

            var ok = await _solicitudService.EditarSolicitudAsync(id, solicitud);
            if (ok) return RedirectToAction("Index", "Vacantes");

            ModelState.AddModelError("", "No se pudo editar la solicitud.");
            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var ok = await _solicitudService.EliminarSolicitudAsync(id);
            if (!ok) return BadRequest();

            return RedirectToAction("Index", "Vacantes");
        }

        public async Task<IActionResult> MisSolicitudes(int usuarioId)
        {
            var solicitudes = await _solicitudService.ObtenerSolicitudesDelUsuarioAsync(usuarioId);
            return View(solicitudes);
        }
    }
}
