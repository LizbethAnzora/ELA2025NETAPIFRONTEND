using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class SolicitudCreateDto
    {
       
        [Required(ErrorMessage = "El ID de la vacante es obligatorio")]
        public int IdVacante { get; set; }

        
        [Required]
        public int SolicitanteId { get; set; }

       
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string? NombreCompleto { get; set; }

       
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        public string? CorreoElectronico { get; set; }

        
        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido")]
        public string? NumeroTelefono { get; set; }

        
        [Required(ErrorMessage = "La foto es obligatoria")]
        public string? Foto { get; set; }

        [Required(ErrorMessage = "El campo de experiencia es obligatorio")]
        public string? CamposPersonalizados { get; set; }
    }
}