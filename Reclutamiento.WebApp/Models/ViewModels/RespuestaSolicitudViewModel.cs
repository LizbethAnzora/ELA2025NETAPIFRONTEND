using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.ViewModels
{
    public class RespuestaSolicitudViewModel
    {
        // ID de la Solicitud a la que estamos respondiendo (Clave de negocio)
        [Required]
        public int SolicitudId { get; set; } 

        // ID de la Vacante (Necesario para la redirección)
        [Required]
        public int VacanteId { get; set; }

        // El estado que se actualiza (Ej: Aceptada, Rechazada)
        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        public string? NuevoEstado { get; set; } 

        // El mensaje del administrador
        [Required(ErrorMessage = "El mensaje de respuesta es obligatorio.")]
        [StringLength(500, ErrorMessage = "El mensaje no puede exceder 500 caracteres.")]
        public string? MensajeRespuesta { get; set; }
    }
}