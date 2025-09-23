using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class SolicitudDto
    {
        public int Id { get; set; }

        [Required]
        public int VacanteId { get; set; }

        public string VacanteTitulo { get; set; }

        [Required(ErrorMessage = "El nombre del solicitante es obligatorio")]
        public string NombreSolicitante { get; set; }

        [Required(ErrorMessage = "El correo del solicitante es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string EmailSolicitante { get; set; }

        [Required(ErrorMessage = "El campo de experiencia es obligatorio")]
        public string Experiencia { get; set; }
    }
}