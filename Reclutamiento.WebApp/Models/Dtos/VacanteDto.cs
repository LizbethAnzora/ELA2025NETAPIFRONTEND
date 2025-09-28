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
        
        // Propiedad ajustada para reflejar el backend (bool)
        // Usaremos esta propiedad para el listado, creación y edición
        public bool EstaActiva { get; set; }

        // El campo 'AdministradorNombre' se ha ELIMINADO.
        
        // Agregar las propiedades faltantes que el GET devuelve (solo para completar la info)
        public string? Requisitos { get; set; }
        public string? Ubicacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}