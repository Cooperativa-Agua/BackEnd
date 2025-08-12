using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class CreateTanqueDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "La capacidad máxima debe ser mayor a 0")]
        public double CapacidadMaxima { get; set; }

        [Range(0, 100, ErrorMessage = "El nivel inicial debe estar entre 0 y 100")]
        public double NivelInicial { get; set; } = 0;
    }
}