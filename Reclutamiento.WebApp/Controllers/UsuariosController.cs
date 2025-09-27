using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;

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
            return View("Edit", usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, UsuarioDto usuario)
        {
            if (!ModelState.IsValid) return View("Edit", usuario);

            var ok = await _usuarioService.EditarUsuarioAsync(id, usuario);
            if (ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo editar el usuario.");
            return View("Edit", usuario);
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
