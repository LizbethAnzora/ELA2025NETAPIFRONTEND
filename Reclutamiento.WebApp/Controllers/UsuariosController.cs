using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using ReclutamientoFrontend.WebApp.Models.ViewModels;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<IActionResult> Index(string? nombre, string? correo)
        {
            var usuarios = await _usuarioService.ObtenerUsuariosAsync();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                usuarios = usuarios.Where(u => u.NombreCompleto != null && u.NombreCompleto.Contains(nombre, System.StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(correo))
            {
                usuarios = usuarios.Where(u => u.CorreoElectronico != null && u.CorreoElectronico.Contains(correo, System.StringComparison.OrdinalIgnoreCase));
            }
            return View(usuarios);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return View("Details", usuario);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioDto usuario)
        {
            if (!ModelState.IsValid) return View("Create", usuario);

            var ok = await _usuarioService.CrearUsuarioAsync(usuario);
            if (ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo crear el usuario.");
            return View("Create", usuario);
        }

        [HttpGet]
public async Task<IActionResult> Editar(int id)
{
    // 1. Obtener el DTO del servicio
    var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
    if (usuario == null) return NotFound();
    
    // 2. Mapear el UsuarioDto al EditarUsuarioViewModel
    //    Esto solo toma los campos necesarios para la edición.
    var viewModel = new EditarUsuarioViewModel 
    {
        Id = usuario.Id,
        NombreCompleto = usuario.NombreCompleto
    };
    
    // 3. Pasar el Correo (no editable) a través del ViewBag para mostrarlo en la vista
    ViewBag.CorreoActual = usuario.CorreoElectronico; 
    
    // 4. Pasar el ViewModel (tipo correcto) a la vista
    return View("Edit", viewModel); 
}

        [HttpPost]
public async Task<IActionResult> Editar(int id, EditarUsuarioViewModel model) // <--- 1. Must accept the correct ViewModel
{
    // Check if the ViewModel's validation passed (e.g., NombreCompleto is not empty)
    if (!ModelState.IsValid)
    {
        // 2. If validation fails, we need the CorreoElectronico back for the view's layout.
        //    We must re-fetch the DTO to get the email before re-rendering the view.
        var usuarioOriginal = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
        if (usuarioOriginal != null)
        {
            ViewBag.CorreoActual = usuarioOriginal.CorreoElectronico;
        }
        else
        {
            // Should be rare, but handles if the user was deleted while editing.
            return RedirectToAction(nameof(Index)); 
        }

        // 3. Return the view with the original (failing) ViewModel to show errors.
        return View(model); // <--- Passes EditarUsuarioViewModel, which the view expects.
    }

    // --- SUCCESS PATH ---
    
    // 4. Map the ViewModel back to a DTO for the Service layer call (only include the Name)
    var usuarioDtoParaActualizar = new UsuarioDto
    {
        NombreCompleto = model.NombreCompleto
        // Note: CorreoElectronico and Contrasena are intentionally omitted.
    };
    
    // 5. Call the service (Service will use only NombreCompleto from the DTO payload)
    var ok = await _usuarioService.EditarUsuarioAsync(id, usuarioDtoParaActualizar);
    
    if (ok)
    {
        // Success! Redirect to the list view.
        return RedirectToAction(nameof(Index));
    }
    
    // 6. Handle API failure (e.g., display a generic error message)
    ModelState.AddModelError(string.Empty, "Error al guardar los cambios en la API. Intente nuevamente.");
    
    // Re-fetch email for re-display after API failure
    var usuarioFallo = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
    if (usuarioFallo != null)
    {
        ViewBag.CorreoActual = usuarioFallo.CorreoElectronico;
    }

    return View(model);
}

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return View("Delete", usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var ok = await _usuarioService.EliminarUsuarioAsync(id);
            if (!ok) return BadRequest();
            return RedirectToAction(nameof(Index));
        }
    }
}
