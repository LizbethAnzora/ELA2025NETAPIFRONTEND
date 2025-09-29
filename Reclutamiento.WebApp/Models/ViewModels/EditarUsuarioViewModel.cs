using System.ComponentModel.DataAnnotations;

namespace ReclutamientoFrontend.WebApp.Models.ViewModels
{
    public class EditarUsuarioViewModel
    {
        
        public int Id { get; set; } 
        
       
        [Required(ErrorMessage = "El Nombre Completo es obligatorio.")]
        [Display(Name = "Nuevo Nombre Completo")]
        public string NombreCompleto { get; set; }
    }
}