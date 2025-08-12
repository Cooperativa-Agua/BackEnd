using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class BombaEstadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool EstaEncendida { get; set; }
        public string TipoFalla { get; set; } = string.Empty;
        public int Prioridad { get; set; }
        public int HorasOperacion { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}
