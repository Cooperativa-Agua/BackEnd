using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TanqueDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double NivelAgua { get; set; }
        public double CapacidadMaxima { get; set; }
        public bool EstaActivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public double LitrosActuales { get; set; }
        public string EstadoNivel { get; set; } = string.Empty;
    }
}
