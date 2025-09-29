using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    // DTO usado para mostrar listados y detalles de Solicitudes.
    public class SolicitudDto
    {
        public int Id { get; set; }

        public int VacanteId { get; set; }

        // Necesario para asignar el ID del usuario autenticado
        [Required]
        public int SolicitanteId { get; set; }

        public string? VacanteTitulo { get; set; }

        [Required(ErrorMessage = "El nombre del solicitante es obligatorio")]
        public string? NombreSolicitante { get; set; }

        [Required(ErrorMessage = "El correo del solicitante es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string? EmailSolicitante { get; set; }
        
        // El campo EstadoSolicitud se ha eliminado.

        [Required(ErrorMessage = "El campo de experiencia es obligatorio")]
        public string? Experiencia { get; set; }
        // NOTA: Asumimos que también podrías necesitar el teléfono y la foto para el detalle.
        // Si el GET /api/Solicitudes/{id} devuelve más campos (como Foto y Teléfono),
        // deberías agregarlos aquí.
    }
}