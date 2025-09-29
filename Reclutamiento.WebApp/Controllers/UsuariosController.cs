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
          
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();

          
            var viewModel = new EditarUsuarioViewModel
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto
            };

   
            ViewBag.CorreoActual = usuario.CorreoElectronico;

         
            return View("Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, EditarUsuarioViewModel model) 
        {
            
            if (!ModelState.IsValid)
            {
             
                var usuarioOriginal = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
                if (usuarioOriginal != null)
                {
                    ViewBag.CorreoActual = usuarioOriginal.CorreoElectronico;
                }
                else
                {
                    
                    return RedirectToAction(nameof(Index));
                }

            
                return View(model); 
            }

            
            var usuarioDtoParaActualizar = new UsuarioDto
            {
                NombreCompleto = model.NombreCompleto
                
            };

        
            var ok = await _usuarioService.EditarUsuarioAsync(id, usuarioDtoParaActualizar);

            if (ok)
            {
               
                return RedirectToAction(nameof(Index));
            }

      
            ModelState.AddModelError(string.Empty, "Error al guardar los cambios en la API. Intente nuevamente.");

            
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
