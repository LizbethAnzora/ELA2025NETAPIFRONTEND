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

        // 1. INDEX: Ahora usa el método para obtener todas (Admin) y corrige el nombre del método
       public async Task<IActionResult> Index(string titulo) // ACEPTAMOS EL PARÁMETRO DE FILTRO
        {
            // 1. OBTENER TODAS LAS VACANTES DEL ADMINISTRADOR
            var vacantes = await _vacanteService.ObtenerVacantesAdminAsync();
            
            // 2. APLICAR FILTRO SI EL PARÁMETRO 'titulo' ESTÁ PRESENTE
            if (!string.IsNullOrEmpty(titulo))
            {
                // Limpiamos y convertimos a minúsculas para una búsqueda sin distinción de mayúsculas/minúsculas
                var filtro = titulo.Trim().ToLower();
                vacantes = vacantes.Where(v => v.Titulo.ToLower().Contains(filtro)).ToList();
                
                // 3. PASAR EL VALOR DEL FILTRO A LA VISTA para mantenerlo en el input
                ViewData["FiltroActual"] = titulo; 
            }
            
            // 4. DEVOLVER EL MODELO FILTRADO (o el modelo completo si no hay filtro)
            return View(vacantes);
        }

        // 2. DETAILS: Corregimos el nombre del método a 'Details' (convención) y target de la vista
        public async Task<IActionResult> Details(int id) 
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            // El backend devuelve null si no existe
            if (vacante == null) return NotFound(); 
            // La vista a renderizar es Views/Vacantes/Details.cshtml
            return View(vacante); 
        }

        [HttpGet]
        public IActionResult Create() // Corregimos el nombre del método a 'Create' (convención)
        {
            // La vista a renderizar es Views/Vacantes/Create.cshtml
            return View(new VacanteDto { EstaActiva = true }); // Inicializamos el DTO
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Añadido por seguridad
        public async Task<IActionResult> Create(VacanteDto vacante)
        {
            // 3. CREAR POST: Forzamos la vacante a estar activa al crearse
            vacante.EstaActiva = true; 
            
            if (!ModelState.IsValid) 
            {
                // Aseguramos que se devuelva a la vista de Create si hay error de validación
                return View(vacante);
            }

            var ok = await _vacanteService.CrearVacanteAsync(vacante);
            if (ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo crear la vacante.");
            return View(vacante); // Aseguramos que se devuelva a la vista de Create si hay error del Service
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) // Corregimos el nombre del método a 'Edit'
        {
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            if (vacante == null) return NotFound();
            // La vista a renderizar es Views/Vacantes/Edit.cshtml
            return View(vacante); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Añadido por seguridad
        public async Task<IActionResult> Edit(int id, VacanteDto vacante)
        {
             // 4. EDITAR POST: Si el estado se ha quitado de la vista (como pediste), 
             // debemos asegurar que el 'EstaActiva' se mantenga al editar.
             // Para esto, en la vista Edit, deberás usar un campo oculto (hidden)
             // para el valor actual de EstaActiva, o recuperarlo del backend aquí
             // antes de enviar el PUT.
             
             // Si el DTO no es válido, volvemos a la vista de edición
            if (!ModelState.IsValid) return View(vacante); 

            var ok = await _vacanteService.EditarVacanteAsync(id, vacante);
            if (ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo editar la vacante.");
            return View(vacante);
        }

        // 5. DESHABILITAR: Acción específica para cambiar el estado a Inactivo (usando el DELETE del Service)
        // Se llama 'Deshabilitar' para reflejar mejor su propósito
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deshabilitar(int id)
        {
            // Usamos el método renombrado en el service
            var ok = await _vacanteService.DeshabilitarVacanteAsync(id); 
            
            if (!ok) 
            {
                // Puedes usar un TempData para mostrar un error en la vista Index
                TempData["ErrorMessage"] = "No se pudo deshabilitar la vacante.";
            }

            return RedirectToAction(nameof(Index));
        }

        // 5. NUEVA ACCIÓN: HABILITAR VACANTE
        [HttpPost]
        [ActionName("Habilitar")] // Nombre de la acción para el ruteo
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Habilitar(int id)
        {
            // Para Habilitar, debemos obtener la vacante, establecer EstaActiva a true y usar el método de Editar (PUT)
            var vacante = await _vacanteService.ObtenerVacantePorIdAsync(id);
            
            if (vacante == null) return NotFound();

            // Configuramos el estado a activa
            vacante.EstaActiva = true;

            // Usamos el método de editar (PUT) para actualizar el estado a activa
            var ok = await _vacanteService.EditarVacanteAsync(id, vacante);
            
            if (!ok) 
            {
                TempData["ErrorMessage"] = "No se pudo habilitar la vacante.";
            } else {
                TempData["SuccessMessage"] = "La vacante ha sido re-activada correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Solicitudes(int vacanteId)
        {
            var solicitudes = await _vacanteService.ObtenerSolicitudesPorVacanteAsync(vacanteId);
            return View(solicitudes);
        }
    }
}