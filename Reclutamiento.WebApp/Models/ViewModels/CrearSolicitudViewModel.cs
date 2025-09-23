using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.ViewModels
{
    public class CrearSolicitudViewModel
    {
        public int VacanteId { get; set; }

        public string? VacanteTitulo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? NombreSolicitante { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string? EmailSolicitante { get; set; }

        [Required(ErrorMessage = "Debe describir su experiencia")]
        [StringLength(1000)]
        public string? Experiencia { get; set; }
    }
}