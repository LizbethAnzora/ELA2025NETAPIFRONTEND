using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class SolicitudDto
    {
        public int Id { get; set; }
        
        // Coincide con "idVacante" del endpoint
        [Required(ErrorMessage = "El ID de la vacante es obligatorio")]
        public int IdVacante { get; set; }

        // Necesario para asignar el ID del usuario autenticado
        [Required]
        public int SolicitanteId { get; set; }

        // Coincide con "nombreCompleto" del endpoint
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string? NombreCompleto { get; set; }

        // Coincide con "correoElectronico" del endpoint
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        public string? CorreoElectronico { get; set; }

        // Coincide con "numeroTelefono" del endpoint
        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido")]
        public string? NumeroTelefono { get; set; }

        // Coincide con "foto" del endpoint (será la URL o el base64 de la imagen)
        [Required(ErrorMessage = "La foto es obligatoria")]
        public string? Foto { get; set; }

        // Coincide con "camposPersonalizados" del endpoint (usaremos este campo para 'Experiencia')
        // Basándome en tu SolicitudDto, asumo que "Experiencia" es uno de los campos personalizados.
        [Required(ErrorMessage = "El campo de experiencia es obligatorio")]
        public string? CamposPersonalizados { get; set; }
    }
}