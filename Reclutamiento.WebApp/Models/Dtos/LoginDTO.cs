
using System.ComponentModel.DataAnnotations;
using Reclutamiento.WebApp.Models.Dtos;


namespace ReclutamientoFrontend.WebApp.Models.Dtos
{
    public class LoginDto
    {
        public int Id { get; set; }


        public string? NombreCompleto { get; set; }

        public string? CorreoElectronico { get; set; }

    public int? Rol { get; set; } 
         public string? Token { get; set; }

    }
}