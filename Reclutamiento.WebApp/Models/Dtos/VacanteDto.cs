using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class VacanteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        public string? Descripcion { get; set; }

        [Required]
        public string? Estado { get; set; } // Activa / Inactiva

        public string? AdministradorNombre { get; set; }
    }
}