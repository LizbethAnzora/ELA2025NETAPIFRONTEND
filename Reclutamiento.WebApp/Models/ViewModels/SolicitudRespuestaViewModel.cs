using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.ViewModels
{
    public class SolicitudRespuestaViewModel
    {
       
        public int SolicitudId { get; set; }

        [Required(ErrorMessage = "El mensaje de respuesta es obligatorio")]
        [StringLength(2000, ErrorMessage = "La respuesta no puede exceder los 2000 caracteres")]
        public string? ContenidoMensaje { get; set; }
    }
}