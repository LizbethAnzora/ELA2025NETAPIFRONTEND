using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class RespuestaCreateDto
    {
        [Required(ErrorMessage = "El mensaje de respuesta es obligatorio")]
        [StringLength(2000, ErrorMessage = "El mensaje no puede exceder los 2000 caracteres")]
        public string? ContenidoMensaje { get; set; }
    }
}