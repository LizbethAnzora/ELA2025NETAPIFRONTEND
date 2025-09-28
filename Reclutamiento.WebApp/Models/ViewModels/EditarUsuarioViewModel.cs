using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.ViewModels
{
    public class EditarUsuarioViewModel
    {
        // Se usa para el input hidden en la vista
        public int Id { get; set; } 
        
        // Único campo editable y requerido por el endpoint PUT
        [Required(ErrorMessage = "El Nombre Completo es obligatorio.")]
        [Display(Name = "Nuevo Nombre Completo")]
        public string NombreCompleto { get; set; }
    }
}