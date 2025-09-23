using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class RespuestaDto
    {
        public int Id { get; set; }

        [Required]
        public int SolicitudId { get; set; }

        public string VacanteTitulo { get; set; }

        public string NombreSolicitante { get; set; }

        [Required(ErrorMessage = "El mensaje de respuesta es obligatorio")]
        [StringLength(2000, ErrorMessage = "La respuesta no puede exceder los 2000 caracteres")]
        public string Mensaje { get; set; }

        public string AdministradorNombre { get; set; }
    }
}