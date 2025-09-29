using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Security.Claims; 
using System.Linq; 
using Microsoft.AspNetCore.Authorization;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    [Authorize] // Solo usuarios logueados (Solicitantes o Admins) pueden acceder aquí.
    public class SolicitudesController : Controller
    {
        private readonly SolicitudService _solicitudService;
        private readonly VacanteService _vacanteService; 

        // Constructor modificado
        public SolicitudesController(SolicitudService solicitudService, VacanteService vacanteService)
        {
            _solicitudService = solicitudService;
            _vacanteService = vacanteService;
        }

        // ----------------------------------------------------
        // ACCIONES DEL SOLICITANTE (Mis Solicitudes)
        // ----------------------------------------------------

        // La vista 'Mis Solicitudes' se mapea típicamente a Index.
        // Se asume que solo los solicitantes acceden a esta ruta.
        public async Task<IActionResult> Index()
        {
            // 1. Obtener el ID del solicitante
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int solicitanteId))
            {
                // Si no se encuentra el ID, no podemos obtener las solicitudes.
                // Manejar como un error o una lista vacía.
                TempData["ErrorMessage"] = "No se pudo identificar al usuario solicitante.";
                return View(new List<SolicitudDto>());
            }
            
            // 2. Obtener las solicitudes del usuario
            var solicitudes = await _solicitudService.ObtenerSolicitudesDelUsuarioAsync(solicitanteId);
            return View(solicitudes);
        }

        // ----------------------------------------------------
        // CREACIÓN (Aplicar a una Vacante)
        // ----------------------------------------------------
        
        [HttpGet]
        public async Task<IActionResult> Create(int vacanteId) // Recibe el ID de la vacante desde Home/Index
        {
            // 1. Obtener la vacante para autocompletar el título
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(vacanteId);

            if (vacante == null)
            {
                TempData["ErrorMessage"] = "La vacante especificada no existe.";
                return RedirectToAction("Index", "Home");
            }
            
            // 2. Crear el DTO y precargar el ID de la vacante
            var model = new SolicitudCreateDto 
            {
                IdVacante = vacanteId
            };
            
            // Usamos ViewBag para pasar el Título (no es parte del DTO de creación)
            ViewBag.VacanteTitulo = vacante.Titulo;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Buena práctica de seguridad
        public async Task<IActionResult> Create(SolicitudCreateDto solicitud) // ⚠️ Usamos SolicitudCreateDto
        {
            // El Título de la vacante se usa para la vista, no para el POST, 
            // pero lo necesitamos si la validación falla.
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(solicitud.IdVacante);
            if (vacante != null)
            {
                ViewBag.VacanteTitulo = vacante.Titulo;
            }

            if (!ModelState.IsValid) 
            {
                return View(solicitud);
            }

            var ok = await _solicitudService.CrearSolicitudAsync(solicitud);
            
            if (ok) 
            {
                TempData["SuccessMessage"] = "¡Solicitud enviada con éxito! Revisa tu correo electrónico para futuras respuestas.";
                // Redirigir a la index de home, como solicitaste.
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Ocurrió un error al guardar y enviar la solicitud. Inténtalo de nuevo.");
            return View(solicitud);
        }

        // ----------------------------------------------------
        // DETALLES (Para Solicitantes y Admins)
        // ----------------------------------------------------

        public async Task<IActionResult> Details(int id) // Renombrado a Details para convención MVC
        {
            var solicitud = await _solicitudService.ObtenerSolicitudPorIdAsync(id);
            if (solicitud == null) return NotFound();
            
            // Nota: Aquí, el SolicitudDto es usado para mostrar el detalle.
            
            return View(solicitud);
        }
        
        // ----------------------------------------------------
        // ACCIONES DE ADMINISTRADOR (Mantener, pero no son el foco)
        // ----------------------------------------------------
        
        // Se mantienen las acciones de Editar y Eliminar, usando SolicitudDto,
        // asumiendo que el administrador usa un formulario distinto para esto.

        public async Task<IActionResult> Editar(int id)
        {
            // ... (Lógica mantenida) ...
            var solicitud = await _solicitudService.ObtenerSolicitudPorIdAsync(id);
            if (solicitud == null) return NotFound();
            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, SolicitudDto solicitud)
        {
            // ... (Lógica mantenida) ...
            if (!ModelState.IsValid) return View(solicitud);
            var ok = await _solicitudService.EditarSolicitudAsync(id, solicitud);
            if (ok) return RedirectToAction("Index", "Vacantes");
            ModelState.AddModelError("", "No se pudo editar la solicitud.");
            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            // ... (Lógica mantenida) ...
            var ok = await _solicitudService.EliminarSolicitudAsync(id);
            if (!ok) return BadRequest();
            return RedirectToAction("Index", "Vacantes");
        }
    }
}