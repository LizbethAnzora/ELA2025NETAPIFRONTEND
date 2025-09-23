using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public string? Rol { get; set; } // Ejemplo: "Admin" o "Solicitante"
    }
}