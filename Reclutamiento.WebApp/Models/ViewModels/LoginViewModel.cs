using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool Recordar { get; set; }
    }
}