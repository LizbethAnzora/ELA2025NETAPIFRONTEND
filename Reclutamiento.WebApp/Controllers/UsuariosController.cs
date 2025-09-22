using Microsoft.AspNetCore.Mvc;
using ReclutamientoFrontend.WebApp.Services;
using ReclutamientoFrontend.WebApp.Models.Dtos;
using System.Threading.Tasks;

namespace ReclutamientoFrontend.WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<IActionResult> Index(string filtroNombre)
        {
            var usuarios = await _usuarioService.ObtenerUsuariosAsync(filtroNombre);
            return View(usuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var creado = await _usuarioService.CrearUsuarioAsync(dto);
            if (creado)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo crear el usuario.");
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UsuarioDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var actualizado = await _usuarioService.EditarUsuarioAsync(id, dto);
            if (actualizado)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo actualizar el usuario.");
            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eliminado = await _usuarioService.EliminarUsuarioAsync(id);
            if (eliminado)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo eliminar el usuario.");
            return View();
        }
    }
}