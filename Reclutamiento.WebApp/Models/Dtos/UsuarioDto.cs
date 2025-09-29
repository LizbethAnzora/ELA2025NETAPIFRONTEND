using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class UsuarioDto
    {
        
        public int Id { get; set; } 

        
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder los 100 caracteres")]
        public string? NombreCompleto { get; set; }

     
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
        public string? CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string? Contrasena { get; set; } 
        
        
        public object? Rol { get; set; } 
        public DateTime? FechaCreacion { get; set; } 
        public DateTime? FechaActualizacion { get; set; }
    }
}