using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class BombaEstadoResumenDto
    {
        public int TotalBombas { get; set; }
        public int BombasEncendidas { get; set; }
        public int BombasApagadas { get; set; }
        public int RelaysActivos { get; set; }
        public int SalvaMotoresActivos { get; set; }
        public int FlujometrosActivos { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}
