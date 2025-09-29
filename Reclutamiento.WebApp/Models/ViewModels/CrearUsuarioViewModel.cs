using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.ViewModels
{
    public class CrearUsuarioViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Debe asignar un rol")]
        public string? Rol { get; set; } 

        [Required(ErrorMessage = "Debe asignar una contraseña")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}