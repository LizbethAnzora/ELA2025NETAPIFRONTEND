
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using ReclutamientoFrontend.WebApp.Models.ViewModels; 
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ReclutamientoFrontend.WebApp.Controllers
{

    [Authorize(Roles = "Admin")] 
   public class VacantesController : Controller
    {
        private readonly VacanteService _vacanteService;
        private readonly SolicitudService _solicitudService; 
                public VacantesController(VacanteService vacanteService, SolicitudService solicitudService)
        {
            _vacanteService = vacanteService;
            _solicitudService = solicitudService;
        }

              public async Task<IActionResult> Index(string titulo)
        {
            var vacantes = await _vacanteService.ObtenerVacantesAdminAsync();
            if (!string.IsNullOrEmpty(titulo))
            {
                var filtro = titulo.Trim().ToLower();
                vacantes = vacantes.Where(v => v.Titulo.ToLower().Contains(filtro)).ToList();
                ViewData["FiltroActual"] = titulo;
            }
            return View(vacantes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            if (vacante == null) return NotFound();
            return View(vacante);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new VacanteDto { EstaActiva = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacanteDto vacante)
        {
            vacante.EstaActiva = true;
            if (!ModelState.IsValid) return View(vacante);

            var ok = await _vacanteService.CrearVacanteAsync(vacante);
            if (ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo crear la vacante.");
            return View(vacante);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            if (vacante == null) return NotFound();
            return View(vacante);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VacanteDto vacante)
        {
            if (!ModelState.IsValid) return View(vacante);
            var ok = await _vacanteService.EditarVacanteAsync(id, vacante);
            if (ok) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "No se pudo editar la vacante.");
            return View(vacante);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deshabilitar(int id)
        {
            var ok = await _vacanteService.DeshabilitarVacanteAsync(id);
            if (!ok) TempData["ErrorMessage"] = "No se pudo deshabilitar la vacante.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ActionName("Habilitar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Habilitar(int id)
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            if (vacante == null) return NotFound();
            vacante.EstaActiva = true;
            var ok = await _vacanteService.EditarVacanteAsync(id, vacante);

            if (!ok)
            {
                TempData["ErrorMessage"] = "No se pudo habilitar la vacante.";
            }
            else
            {
                TempData["SuccessMessage"] = "La vacante ha sido re-activada correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }


       
        public async Task<IActionResult> Solicitudes(int vacanteId)
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(vacanteId);
            if (vacante == null)
            {
                TempData["ErrorMessage"] = "La vacante no existe o fue eliminada.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["VacanteTitulo"] = vacante.Titulo;
            ViewData["VacanteId"] = vacanteId;

            
            var solicitudes = await _solicitudService.ObtenerSolicitudesPorVacanteAsync(vacanteId);

            return View(solicitudes);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResponderSolicitud(SolicitudRespuestaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "El mensaje de respuesta no puede estar vacío.";
                return RedirectToAction(nameof(Index));
            }

            var respuestaDto = new RespuestaCreateDto
            {
                ContenidoMensaje = model.ContenidoMensaje
            };


            var ok = await _solicitudService.EnviarRespuestaAsync(model.SolicitudId, respuestaDto);

          
            var solicitudActualizada = await _solicitudService.ObtenerSolicitudPorIdAsync(model.SolicitudId);

            if (ok)
            {
                TempData["SuccessMessage"] = $"Respuesta enviada con éxito al solicitante ID: {model.SolicitudId}.";
            }
            else
            {
                TempData["ErrorMessage"] = "Ocurrió un error al enviar la respuesta. Inténtelo de nuevo.";
            }

          
            if (solicitudActualizada != null && solicitudActualizada.IdVacante > 0)
            {
                return RedirectToAction(nameof(Solicitudes), new { vacanteId = solicitudActualizada.IdVacante });
            }

          
            return RedirectToAction(nameof(Index));
        }
    }
}